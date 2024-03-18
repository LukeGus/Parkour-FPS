using System.Collections;
using System.Collections.Generic;
using cowsins;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using FishNet;
using FishNet.Transporting;
using QFSW.QC.Actions;
using UnityEngine;

namespace cowsins
{
    public class Projectile : NetworkBehaviour
    {
        [SyncVar(Channel = Channel.Reliable, ReadPermissions = ReadPermission.Observers, WritePermissions = WritePermission.ServerOnly)] public float projectileDamage;
        [SyncVar(Channel = Channel.Reliable, ReadPermissions = ReadPermission.Observers, WritePermissions = WritePermission.ServerOnly)] public float bulletLife;
        [SyncVar(Channel = Channel.Reliable, ReadPermissions = ReadPermission.Observers, WritePermissions = WritePermission.ServerOnly)] public float speed;
        [SyncVar(Channel = Channel.Reliable, ReadPermissions = ReadPermission.Observers, WritePermissions = WritePermission.ServerOnly)] public Vector3 dir;
    
        private Rigidbody rb;

        private void Start()
        {
            StartCoroutine(DisableAfterDelay(bulletLife));

            rb = GetComponent<Rigidbody>();
        }

        private void OnTriggerEnter(Collider collision)
        {
            if (collision.gameObject.CompareTag("RemotePlayer"))
            {
                collision.gameObject.GetComponent<HealthManager>().TakeDamageServerRpc(projectileDamage);
                
                Debug.Log("Player Hit");
                DespawnProjectileServerRpc();
            }
        }

        private IEnumerator DisableAfterDelay(float delay)
        {
            if(bulletLife == 0)
                yield return new WaitForSeconds(1);
            
            yield return new WaitForSeconds(delay);

            DespawnProjectileServerRpc();
        }

        void FixedUpdate()
        {
            Propel();
        }

        void Propel()
        {
            rb.AddForce(dir * speed, ForceMode.Force);
        }

        [ServerRpc(RequireOwnership = false)]
        private void DespawnProjectileServerRpc()
        {
            InstanceFinder.ServerManager.Despawn(this.gameObject);
        }
    }
}
 