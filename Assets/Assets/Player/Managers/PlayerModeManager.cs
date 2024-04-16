using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModeManager : MonoBehaviour
{
    public GameObject mainPlayer;
    public GameObject builderPlayer;
    
    private bool isMainPlayerActive = false;
    private bool isBuilderPlayerActive = false;
    
    private void Start()
    {
        SetMainPlayerActive();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (isMainPlayerActive)
            {
                SetBuilderPlayerActive();
            }
            else if (isBuilderPlayerActive)
            {
                SetMainPlayerActive();
            }
        }

        if (isMainPlayerActive)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else if (isBuilderPlayerActive)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
    
    public void SetMainPlayerActive()
    {
        mainPlayer.SetActive(true);
        builderPlayer.SetActive(false);
        isMainPlayerActive = true;
        isBuilderPlayerActive = false;
    }
    
    public void SetBuilderPlayerActive()
    {
        mainPlayer.SetActive(false);
        builderPlayer.SetActive(true);
        isMainPlayerActive = false;
        isBuilderPlayerActive = true;
    }
}
