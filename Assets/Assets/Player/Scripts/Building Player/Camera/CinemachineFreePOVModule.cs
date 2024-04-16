using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using DG.Tweening;
using UnityEngine.InputSystem;

public class CinemachineFreePOVModule : CinemachineExtension
{
    [SerializeField] private CinemachineRecomposer cinemachineRecomposer;
    
    [SerializeField] private Camera mainCamera;
    
    [SerializeField] private GameObject orientation;

    [SerializeField] private GameObject cameraRoot;
    
    public float defaultFOV;
    
    [SerializeField] private float minYRotation, maxYRotation;
    [SerializeField] private float minXRotation, maxXRotation;
    
    [SerializeField] private float horizontalSpeed;
    [SerializeField] private float verticalSpeed;

    [SerializeField] public float slowSpeed;
    [SerializeField] public float normalSpeed;
    [SerializeField] public float sprintSpeed;
    
    private Vector3 startingRotation;
    private Vector2 deltaInput;
    private float currentSpeed;

    protected void Update()
    {
        deltaInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        
        Movement();
    }

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
                orientation.transform.rotation = Quaternion.Euler(-startingRotation.y, startingRotation.x, state.RawOrientation.eulerAngles.z);
            }
        }
    }

    public void Movement()
    {
        // Get input for movement
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Determine movement direction relative to camera
        Vector3 cameraForward = orientation.transform.forward;
        Vector3 cameraRight = orientation.transform.right;
        Vector3 moveDirection = (cameraForward * verticalInput + cameraRight * horizontalInput).normalized;

        // Determine speed based on input
        if (Input.GetKey(KeyCode.LeftShift))
        {
            currentSpeed = sprintSpeed;
        }
        else if (Input.GetKey(KeyCode.LeftAlt))
        {
            currentSpeed = slowSpeed;
        }
        else
        {
            currentSpeed = normalSpeed;
        }

        // Move the cameraRoot based on input and speed
        cameraRoot.transform.Translate(moveDirection * currentSpeed * Time.deltaTime);
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