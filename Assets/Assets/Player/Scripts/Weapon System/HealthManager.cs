using System.Collections;
using System.Collections.Generic;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using FishNet.Transporting;
using UnityEngine;

public class HealthManager : NetworkBehaviour
{
    // make these network vars
    
    [SyncVar(Channel = Channel.Reliable, ReadPermissions = ReadPermission.Observers, WritePermissions = WritePermission.ServerOnly)] [HideInInspector] public float health;

    [SyncVar(Channel = Channel.Reliable, ReadPermissions = ReadPermission.Observers, WritePermissions = WritePermission.ServerOnly)] public float maxHealth;
    
    [ServerRpc]
    public void TakeDamage(float damage)
    {
        health -= damage;
        
        if (health <= 0)
        {
            Die();
        }
    }
    
    public void Die()
    {
        health = maxHealth;
    }
}
