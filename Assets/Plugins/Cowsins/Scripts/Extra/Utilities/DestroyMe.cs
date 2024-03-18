/// <summary>
/// This script belongs to cowsins� as a part of the cowsins� FPS Engine. All rights reserved. 
/// </summary>

using FishNet;
using FishNet.Object;
using UnityEngine;

namespace cowsins
{
    public class DestroyMe : NetworkBehaviour
    {
        public float timeToDestroy;

        void Start()
        {
            Invoke("DestroyMeObj", timeToDestroy);
        }

        // Update is called once per frame
        void DestroyMeObj()
        {
            if(IsSpawned) InstanceFinder.ServerManager.Despawn(gameObject);
            Destroy(this.gameObject);
        }
    }
}