using UnityEngine;
using cowsins;
using FishNet.Object;

public class NetworkDisabler : NetworkBehaviour
{
    public static NetworkDisabler Instance;

    public PlayerMovement playerMovement;
    public PlayerStates playerStates;
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
#if !DEDICATED_SERVER
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

        weaponCamera.enabled = IsOwner;
        playerMovement.enabled = IsOwner;
        playerStates.enabled = IsOwner;

        cameraGO.gameObject.SetActive(IsOwner);
        playerGraphics.SetActive(!IsOwner);
    }
}