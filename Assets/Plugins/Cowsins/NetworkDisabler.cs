using UnityEngine;
using FishNet.Object;
using cowsins;

namespace cowsins
{
    public class NetworkDisabler : NetworkBehaviour
    {
        public static NetworkDisabler Instance;
        
        public UIController uiController;
        public InteractManager interactManager;
        public GameObject playerGraphics;
        public GameObject player;
        public GameObject cameraGO;
        public Camera weaponCamera;

        [HideInInspector] public bool isOwner;

        private void Start()
        {
            SetInstance();
            
            Invoke(nameof(Disable), 0.5f);

            InvokeRepeating(nameof(Rename), 0, 1);
        }

        public void SetInstance()
        {
#if !UNITY_SERVER
            if (Instance == null && IsOwner) 
            { 
                Instance = this; 
            }
# else
            if (Instance == null)
            {
                Instance = this;
            }
#endif
        }

        private void Rename()
        {
            player.name = IsOwner ? "LocalPlayer" : "RemotePlayer";
            player.tag = IsOwner ? "LocalPlayer" : "RemotePlayer";
        }

        private void Disable()
        {
            if (IsOwner) isOwner = true;
            else isOwner = false;
            
            interactManager.enabled = IsOwner;
            weaponCamera.enabled = IsOwner;
            
            player.SetActive(IsOwner);
            uiController.gameObject.SetActive(IsOwner);
            cameraGO.gameObject.SetActive(IsOwner);
            playerGraphics.SetActive(!IsOwner);
        }
    }
}