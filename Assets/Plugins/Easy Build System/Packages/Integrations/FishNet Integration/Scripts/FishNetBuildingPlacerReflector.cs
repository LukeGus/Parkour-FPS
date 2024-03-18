#if EBS_FISHNET

using UnityEngine;

using EasyBuildSystem.Features.Runtime.Buildings.Part;
using EasyBuildSystem.Features.Runtime.Buildings.Manager;
using FishNet.Object;
using FishNet;

namespace EasyBuildSystem.Packages.Integrations.FishNet
{
    [RequireComponent(typeof(FishNetBuildingPlacer))]
    public class FishNetBuildingPlacerReflector : NetworkBehaviour
    {
        [SerializeField] FishNetBuildingPlacer m_Placer;

        #region Methods

        void Update()
        {
            m_Placer.enabled = IsOwner;
        }

        [ServerRpc]
        public void CmdPlace(string partIdentifier, Vector3 position, Vector3 rotation, Vector3 scale)
        {
            BuildingPart buildingPart = BuildingManager.Instance.GetBuildingPartByIdentifier(partIdentifier);

            if (buildingPart != null)
            {
                BuildingPart instancedBuildingPart = BuildingManager.Instance.PlaceBuildingPart(buildingPart, position, rotation, scale, true);
                InstanceFinder.ServerManager.Spawn(instancedBuildingPart.gameObject);
            }
        }

        [ServerRpc]
        public void CmdDestroy(NetworkObject identity)
        {
            InstanceFinder.ServerManager.Despawn(identity.gameObject);
        }

        #endregion
    }
}
#endif