using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using DG.Tweening;
using UnityEngine.InputSystem;

public class CinemachinePOVModule : CinemachineExtension
{
    [SerializeField] private CinemachineRecomposer cinemachineRecomposer;
    
    [SerializeField] private Camera mainCamera;
    
    public float defaultFOV;
    
    [SerializeField] private float minYRotation, maxYRotation;
    [SerializeField] private float minXRotation, maxXRotation;
    
    [SerializeField] private float horizontalSpeed;
    [SerializeField] private float verticalSpeed;
    
    private Vector3 startingRotation;
    private Vector2 deltaInput;

    protected override void Awake()
    {
        base.Awake();
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
    {
        if (vcam.Follow)
        {
            if (stage == CinemachineCore.Stage.Aim)
            {
                if (startingRotation == null) startingRotation = transform.localRotation.eulerAngles;
                
                //deltaInput = movementManager.lookInput;
                
                startingRotation.x += deltaInput.x * verticalSpeed * Time.deltaTime;
                startingRotation.y += deltaInput.y * horizontalSpeed *  Time.deltaTime;
                
                startingRotation.y = Mathf.Clamp(startingRotation.y, minYRotation, maxYRotation);
                
                if (minXRotation != 0f || maxXRotation != 0f)
                {
                    startingRotation.x = Mathf.Clamp(startingRotation.x, minXRotation, maxXRotation);
                }
                else
                {
                    startingRotation.x = Mathf.Clamp(startingRotation.x, Single.NegativeInfinity, Single.PositiveInfinity);
                }
                
                state.RawOrientation = Quaternion.Euler(-startingRotation.y, startingRotation.x, state.RawOrientation.eulerAngles.z);
                //movementManager.orientation.rotation = Quaternion.Euler(0, startingRotation.x, 0);
            }
        }
    }

    public void SetFOV(float endValue)
    {
        mainCamera.DOFieldOfView(endValue, 0.25f);
    }

    public void SetTilt(float zTilt)
    { 
        cinemachineRecomposer.m_Dutch = Mathf.Lerp(cinemachineRecomposer.m_Dutch, zTilt, 1f);
    }

    public void SetMinMaxXRotation(float min, float max)
    {
        minXRotation = min;
        maxXRotation = max;
    }

    public void SetMinMaxYRotation(float min, float max)
    {
        minYRotation = min;
        maxYRotation = max;
    }
}