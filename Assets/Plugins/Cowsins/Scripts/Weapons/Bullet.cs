/// <summary>
/// This script belongs to cowsins� as a part of the cowsins� FPS Engine. All rights reserved. 
/// </summary>
using UnityEngine;
using FishNet.Object;
using FishNet.Object.Synchronizing;


namespace cowsins
{
    public class Bullet : NetworkBehaviour
    {
        [HideInInspector] [SyncVar] public float speed, damage;

        [HideInInspector] [SyncVar] public Vector3 destination;

        [HideInInspector] [SyncVar] public bool gravity;

        [HideInInspector] [SyncVar] public Transform player;

        [HideInInspector] [SyncVar] public bool hurtsPlayer;

        [HideInInspector] [SyncVar] public bool explosionOnHit;

        [HideInInspector] [SyncVar] public GameObject explosionVFX;

        [HideInInspector] [SyncVar] public float explosionRadius, explosionForce;

        [HideInInspector] [SyncVar] public float criticalMultiplier;

        [HideInInspector] [SyncVar] public float duration;

        private void Start()
        {
            transform.LookAt(destination);
            Invoke("DestroyProjectile", duration);

            Invoke("Spawn", 0.1f);
        }
        
        private void Spawn()
        {
            ServerManager.Spawn(gameObject);
            Debug.Log("Spawned");
        }
        
        private void Update() => transform.Translate(0.0f, 0.0f, speed * Time.deltaTime);

        private bool projectileHasAlreadyHit = false; // Prevent from double hitting issues

        private void OnTriggerEnter(Collider other)
        {
            if (projectileHasAlreadyHit) return;
            if (other.CompareTag("Critical"))
            {
                CowsinsUtilities.GatherDamageableParent(other.transform).Damage(damage * criticalMultiplier);
                DestroyProjectile();
                projectileHasAlreadyHit = true;
                return;
            }
            else if (other.CompareTag("BodyShot"))
            {
                CowsinsUtilities.GatherDamageableParent(other.transform).Damage(damage);
                DestroyProjectile();
                projectileHasAlreadyHit = true;
                return;
            }
            else if (other.GetComponent<IDamageable>() != null && !other.CompareTag("Player"))
            {
                other.GetComponent<IDamageable>().Damage(damage);
                DestroyProjectile();
                projectileHasAlreadyHit = true;
                return;
            }
            if (other.gameObject.layer == 3 || other.gameObject.layer == 8 || other.gameObject.layer == 10
            || other.gameObject.layer == 11 || other.gameObject.layer == 12 || other.gameObject.layer == 13 || other.gameObject.layer == 7) DestroyProjectile(); // Whenever it touches ground / object layer
        }


        private void DestroyProjectile()
        {
            if (explosionOnHit)
            {
                if (explosionVFX != null)
                {
                    Vector3 contact = GetComponent<Collider>().ClosestPoint(transform.position);
                    GameObject impact = Instantiate(explosionVFX, contact, Quaternion.identity);
                    ServerManager.Spawn(impact);
                    impact.transform.rotation = Quaternion.LookRotation(player.position);
                }
                Collider[] cols = Physics.OverlapSphere(transform.position, explosionRadius);

                foreach (Collider c in cols)
                {
                    IDamageable damageable = c.GetComponent<IDamageable>();
                    PlayerMovement playerMovement = c.GetComponent<PlayerMovement>();
                    Rigidbody rigidbody = c.GetComponent<Rigidbody>();

                    if (damageable != null)
                    {
                        if (c.CompareTag("Player") && hurtsPlayer)
                        {
                            float dmg = damage * Mathf.Clamp01(1 - Vector3.Distance(c.transform.position, transform.position) / explosionRadius);
                            damageable.Damage(dmg);
                        }
                        if (!c.CompareTag("Player"))
                        {
                            float dmg = damage * Mathf.Clamp01(1 - Vector3.Distance(c.transform.position, transform.position) / explosionRadius);
                            damageable.Damage(dmg);
                        }
                    }
                    if (playerMovement != null)
                    {
                        CamShake.instance.ExplosionShake(Vector3.Distance(CamShake.instance.gameObject.transform.position, transform.position));
                    }
                    if (rigidbody != null && c != this)
                    {
                        rigidbody.AddExplosionForce(explosionForce, transform.position, explosionRadius, 5, ForceMode.Force);
                    }
                }
            }

            NetworkObject.Despawn(gameObject);
        }
    }
}