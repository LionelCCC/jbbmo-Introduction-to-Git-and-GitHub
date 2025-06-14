using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    [Header("Camera Settings")]
    public Transform target;
    public float followDistance = 5f;
    public float followHeight = 2f;
    public float rotationSpeed = 2f;
    public float verticalAngleLimit = 80f;
    
    [Header("Cinemachine")]
    public CinemachineVirtualCamera virtualCamera;
    public CinemachineTransposer transposer;
    public CinemachineComposer composer;
    
    private float currentRotationX = 0f;
    private float currentRotationY = 0f;
    private InputHandler inputHandler;
    
    void Start()
    {
        inputHandler = FindObjectOfType<InputHandler>();
        
        if (inputHandler != null)
        {
            inputHandler.OnLookInput += HandleLookInput;
        }
        
        // Get Cinemachine components
        if (virtualCamera != null)
        {
            transposer = virtualCamera.GetCinemachineComponent<CinemachineTransposer>();
            composer = virtualCamera.GetCinemachineComponent<CinemachineComposer>();
        }
        
        // Set initial camera position
        if (target != null)
        {
            SetupCameraPosition();
        }
        
        // Lock cursor to center of screen
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    
    void HandleLookInput(Vector2 lookInput)
    {
        // Update rotation based on mouse input
        currentRotationX -= lookInput.y * rotationSpeed;
        currentRotationY += lookInput.x * rotationSpeed;
        
        // Clamp vertical rotation
        currentRotationX = Mathf.Clamp(currentRotationX, -verticalAngleLimit, verticalAngleLimit);
        
        // Apply rotation to camera
        UpdateCameraRotation();
    }
    
    void SetupCameraPosition()
    {
        if (virtualCamera == null || target == null) return;
        
        // Set follow and look at target
        virtualCamera.Follow = target;
        virtualCamera.LookAt = target;
        
        // Configure transposer for third-person view
        if (transposer != null)
        {
            transposer.m_FollowOffset = new Vector3(0f, followHeight, -followDistance);
            transposer.m_BindingMode = CinemachineTransposer.BindingMode.WorldSpace;
        }
        
        // Configure composer for smooth tracking
        if (composer != null)
        {
            composer.m_TrackedObjectOffset = new Vector3(0f, 1.5f, 0f);
            composer.m_DeadZoneWidth = 0.1f;
            composer.m_DeadZoneHeight = 0.1f;
            composer.m_SoftZoneWidth = 0.8f;
            composer.m_SoftZoneHeight = 0.8f;
        }
    }
    
    void UpdateCameraRotation()
    {
        if (target == null) return;
        
        // Create rotation quaternion
        Quaternion rotation = Quaternion.Euler(currentRotationX, currentRotationY, 0f);
        
        // Calculate camera position based on rotation
        Vector3 offset = rotation * new Vector3(0f, followHeight, -followDistance);
        Vector3 desiredPosition = target.position + offset;
        
        // Update camera transform
        transform.position = desiredPosition;
        transform.LookAt(target.position + Vector3.up * 1.5f);
        
        // If using Cinemachine, update the virtual camera's rotation
        if (virtualCamera != null)
        {
            // Create a temporary target for the camera to orbit around
            Vector3 orbitCenter = target.position + Vector3.up * followHeight;
            Vector3 orbitPosition = orbitCenter + rotation * Vector3.back * followDistance;
            
            // Update virtual camera position
            virtualCamera.transform.position = orbitPosition;
            virtualCamera.transform.LookAt(orbitCenter);
        }
    }
    
    void Update()
    {
        // Handle cursor lock toggle
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleCursorLock();
        }
        
        // Manual camera update if not using Cinemachine
        if (virtualCamera == null && target != null)
        {
            UpdateCameraRotation();
        }
    }
    
    void ToggleCursorLock()
    {
        if (Cursor.lockState == CursorLockMode.Locked)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
    
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
        SetupCameraPosition();
    }
    
    public void ShakeCamera(float intensity, float duration)
    {
        if (virtualCamera != null)
        {
            var noise = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            if (noise != null)
            {
                StartCoroutine(CameraShakeCoroutine(noise, intensity, duration));
            }
        }
    }
    
    System.Collections.IEnumerator CameraShakeCoroutine(CinemachineBasicMultiChannelPerlin noise, float intensity, float duration)
    {
        noise.m_AmplitudeGain = intensity;
        noise.m_FrequencyGain = intensity;
        
        yield return new WaitForSeconds(duration);
        
        noise.m_AmplitudeGain = 0f;
        noise.m_FrequencyGain = 0f;
    }
    
    void OnDestroy()
    {
        if (inputHandler != null)
        {
            inputHandler.OnLookInput -= HandleLookInput;
        }
    }
}

