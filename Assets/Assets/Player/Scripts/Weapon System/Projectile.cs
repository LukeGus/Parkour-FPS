using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("References")]
    [HideInInspector] public Rigidbody rb;
    [HideInInspector] public GameObject bulletHolePrefab;
    [HideInInspector] public AudioSource audioSource;

    [Header("Stats")]
    [HideInInspector] public float damage;
    [HideInInspector] public float damageRange;
    [HideInInspector] public float speed;

    [Header("Other")]
    [HideInInspector] public float lifeTime;
    [HideInInspector] public bool useGravity;
    
    [Header("Sound Effects")]
    [HideInInspector] public AudioClip naturalSound;
    [HideInInspector] public AudioClip hitSound;
    
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * speed;
        
        damage = Random.Range(damage - damageRange, damage + damageRange);
        
        if (useGravity)
        {
            rb.useGravity = true;
        } else
        {
            rb.useGravity = false;
        }

        if (naturalSound != null) audioSource.PlayOneShot(naturalSound);
    }
    
    public IEnumerator DestroyProjectile()
    {
        if (gameObject != null) yield return new WaitForSeconds(lifeTime);
        if(this != null) Destroy(gameObject);
    }
    
    private void OnTriggerEnter(Collider colider)
    {
        if (colider.tag != "LocalPlayer" && colider.tag != "RemotePlayer")
        {
            StopCoroutine(DestroyProjectile());
            
            if(hitSound != null) audioSource.PlayOneShot(hitSound);

            Destroy(gameObject);
        }

        if (colider.tag == "RemotePlayer")
        {
            colider.GetComponentInChildren<HealthManager>().TakeDamage(damage);
            
            if (hitSound != null) audioSource.PlayOneShot(hitSound);
            
            Destroy(gameObject);
        }
    }
}
