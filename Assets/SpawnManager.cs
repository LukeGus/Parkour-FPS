using System.Collections;
using System.Collections.Generic;
using FishNet;
using UnityEngine;
using FishNet.Object;
using FishNet.Connection;
using FishNet.Managing;

public class SpawnManager : NetworkBehaviour
{
    public GameObject playerPrefab;
    public Transform spawnPoint;

    private NetworkManager _networkManager;

    public override void OnStartClient()
    {
        base.OnStartClient();
        
        SpawnPlayerServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    public void SpawnPlayerServerRpc(NetworkConnection conn = null)
    {
        GameObject player = Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
        InstanceFinder.ServerManager.Spawn(player, conn);
    }
}