using System.Collections;
using System.Collections.Generic;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using FishNet.Transporting;
using UnityEngine;

namespace cowsins
{
    public class HealthManager : NetworkBehaviour
    {
        [SyncVar(Channel = Channel.Reliable, ReadPermissions = ReadPermission.Observers, WritePermissions = WritePermission.ServerOnly)] public float health;
        
        public float maxHealth;
        
        private void Start()
        {
            ResetHealthServerRpc();
        }

        [ServerRpc(RequireOwnership = false)]
        public void TakeDamageServerRpc(float damage)
        {
            health -= damage;
        }
        
        [ServerRpc(RequireOwnership = false)]
        public void ResetHealthServerRpc()
        {
            health = maxHealth;
            
            Debug.Log("Health Reset");
        }
        
        void Update()
        {
            if (health <= 0)
            {
                Die();
            }
        }
        
        private void Die()
        {
            Debug.Log("Player Died");
            
            ResetHealthServerRpc();
        }
    }
}
