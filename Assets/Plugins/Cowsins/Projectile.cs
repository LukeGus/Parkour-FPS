using System.Collections;
using System.Collections.Generic;
using cowsins;
using FishNet;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using UnityEngine;

namespace cowsins
{
    public class Projectile : NetworkBehaviour
    {
        [SyncVar] public float projectileDamage;

        [SyncVar] public float bulletLife;

        private void OnEnable()
        {
            StartCoroutine(DisableAfterDelay(bulletLife));

            Debug.Log("5");
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("RemotePlayer")) ;
            {
                collision.gameObject.GetComponent<HealthManager>().TakeDamage(projectileDamage);
            }
        }

        private IEnumerator DisableAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);

            // if network object exists, despawn it
            if (this.NetworkObject != null)
            {
                InstanceFinder.ServerManager.Spawn(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
 