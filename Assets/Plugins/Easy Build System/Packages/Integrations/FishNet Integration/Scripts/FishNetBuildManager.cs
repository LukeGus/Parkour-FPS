#if EBS_FISHNET

using UnityEngine;

using EasyBuildSystem.Features.Runtime.Buildings.Part;
using EasyBuildSystem.Features.Runtime.Buildings.Manager;
using EasyBuildSystem.Features.Runtime.Buildings.Manager.Saver;
using FishNet;
using FishNet.Object;

namespace EasyBuildSystem.Packages.Integrations.FishNet
{
    [DefaultExecutionOrder(999)]
    public class FishNetBuildManager : MonoBehaviour
    {
        void Awake()
        {
            foreach (BuildingPart buildingPart in BuildingManager.Instance.BuildingPartReferences)
            {
                if (buildingPart.GetComponent<NetworkObject>() != null)
                {
                    InstanceFinder.NetworkManager.SpawnablePrefabs.AddObject(buildingPart.GetComponent<NetworkObject>());
                    //NetworkClient.RegisterPrefab(buildingPart.gameObject);
                }
            }

            if (BuildingSaver.Instance != null)
            {
                BuildingSaver.Instance.LoadBuildingAtStart = false;
                BuildingSaver.Instance.SaveBuildingAtExit = false;

                BuildingSaver.Instance.OnEndingLoadingEvent.AddListener((BuildingPart[] buildingParts, long time) =>
                {
                    if (buildingParts == null)
                    {
                        return;
                    }

                    if (InstanceFinder.IsServer)
                    {
                        foreach (BuildingPart buildingPart in buildingParts)
                        {
                            InstanceFinder.ServerManager.Spawn(buildingPart.gameObject);
                        }
                    }
                });
            }
        }

        void OnApplicationQuit()
        {
            if (BuildingSaver.Instance != null)
            {
                BuildingSaver.Instance.ForceSave();
            }
        }

        void Start()
        {
            foreach (BuildingPart buildingPart in BuildingManager.Instance.BuildingPartReferences)
            {
                if (buildingPart.GetComponent<NetworkObject>() != null)
                {
                    InstanceFinder.NetworkManager.SpawnablePrefabs.AddObject(buildingPart.GetComponent<NetworkObject>());
                    //NetworkClient.RegisterPrefab(buildingPart.gameObject);
                }
            }
        }

        bool m_Loaded;

        void FixedUpdate()
        {
            if (InstanceFinder.IsServer)
            {
                if (!m_Loaded)
                {
                    if (BuildingSaver.Instance != null)
                    {
                        BuildingSaver.Instance.ForceLoad();
                    }

                    m_Loaded = true;
                }
            }
        }
    }
}
#endif