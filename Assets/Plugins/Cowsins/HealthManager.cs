using System.Collections;
using System.Collections.Generic;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using UnityEngine;

namespace cowsins
{
    public class HealthManager : NetworkBehaviour
    {
        [SyncVar] public float health = 100f;

        public void TakeDamage(float damage)
        {
            health -= damage;

            if (health <= 0)
            {
                Die();
            }
        }

        private void Die()
        {
            Debug.Log("Player died");
        }
    }
}
