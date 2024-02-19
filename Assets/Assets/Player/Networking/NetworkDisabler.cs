using UnityEngine;
using FishNet.Object;
using cowsins;

public class NetworkDisabler : NetworkBehaviour
{
    public PlayerMovement playerMovement;
    public MoveCamera moveCamera;
    public UIController uiController;
    public InteractManager interactManager;
    public GameObject playerGraphics;
    public GameObject player;
    public Camera mainCamera;
    public Camera weaponCamera;

    private void Start()
    {
        Disable();
        
        InvokeRepeating(nameof(Rename), 0, 1);
    }
    
    private void Rename()
    {
        player.name = IsOwner ? "LocalPlayer" : "RemotePlayer";
        player.tag = IsOwner ? "LocalPlayer" : "RemotePlayer";
    }
    
    private void Disable()
    {
        SetComponentEnabled(playerMovement, IsOwner);
        SetComponentEnabled(moveCamera, IsOwner);
        SetComponentEnabled(uiController, IsOwner);
        SetComponentEnabled(interactManager, IsOwner);
        SetComponentEnabled(mainCamera, IsOwner);
        SetComponentEnabled(weaponCamera, IsOwner);
        
        SetGameObjectEnabled(playerGraphics, !IsOwner);
    }

    private void SetComponentEnabled(Component component, bool isEnabled)
    {
        if (component != null)
        {
            component.gameObject.SetActive(isEnabled);
        }
    }
    
    private void SetGameObjectEnabled(GameObject gameObject, bool isEnabled)
    {
        if (gameObject != null)
        {
            gameObject.SetActive(isEnabled);
        }
    }
}