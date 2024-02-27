using System.Collections;
using System.Collections.Generic;
using FishNet;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using UnityEngine;

public class Projectile : NetworkBehaviour
{
    [HideInInspector] [SyncVar] public float projectileSpeed;
    [HideInInspector] [SyncVar] public float projectileDamage;
    
    private float bulletLife = 10f;

    private void OnEnable()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        Vector2 moveDirection = new Vector2(Mathf.Cos(transform.rotation.eulerAngles.z * Mathf.Deg2Rad), Mathf.Sin(transform.rotation.eulerAngles.z * Mathf.Deg2Rad));
        rb.velocity = moveDirection.normalized * projectileSpeed;
        
        StartCoroutine(DisableAfterDelay(bulletLife));
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("RemotePlayer") || collision.gameObject.CompareTag("LocalPlayer"))
        {
            collision.gameObject.GetComponentInChildren<HealthManager>().TakeDamage(projectileDamage);
        }
    }
    
    private IEnumerator DisableAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        
        NetworkObject.Despawn();
    }
}
 