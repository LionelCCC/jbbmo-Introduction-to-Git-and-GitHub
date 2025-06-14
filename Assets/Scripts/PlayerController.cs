using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float rotationSpeed = 10f;
    
    [Header("References")]
    public Transform cameraTarget;
    public Animator animator;
    public TrailRenderer swordTrail;
    public ParticleSystem hitParticles;
    public CinemachineVirtualCamera virtualCamera;
    
    [Header("Animation Hashes")]
    private int waterSurfaceSlashHash;
    private int waterWheelHash;
    private int flowingDanceHash;
    private int stumbleHash;
    private int isMovingHash;
    
    private Vector2 moveInput;
    private Vector2 lookInput;
    private CharacterController characterController;
    private InputHandler inputHandler;
    private BeatmapReader beatmapReader;
    private CinemachineBasicMultiChannelPerlin noise;
    
    private bool isPerformingAttack = false;
    private float attackCooldown = 0.5f;
    private float lastAttackTime;
    
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        inputHandler = GetComponent<InputHandler>();
        beatmapReader = FindObjectOfType<BeatmapReader>();
        
        // Get Cinemachine noise component for camera shake
        if (virtualCamera != null)
        {
            noise = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        }
        
        // Cache animation hashes for performance
        waterSurfaceSlashHash = Animator.StringToHash("WaterSurfaceSlash");
        waterWheelHash = Animator.StringToHash("WaterWheel");
        flowingDanceHash = Animator.StringToHash("FlowingDance");
        stumbleHash = Animator.StringToHash("Stumble");
        isMovingHash = Animator.StringToHash("IsMoving");
        
        // Subscribe to input events
        inputHandler.OnMoveInput += HandleMoveInput;
        inputHandler.OnLookInput += HandleLookInput;
        inputHandler.OnAttackInput += HandleAttackInput;
        
        // Disable sword trail initially
        if (swordTrail != null)
            swordTrail.enabled = false;
    }
    
    void Update()
    {
        HandleMovement();
        HandleRotation();
        UpdateAnimations();
    }
    
    void HandleMoveInput(Vector2 input)
    {
        moveInput = input;
    }
    
    void HandleLookInput(Vector2 input)
    {
        lookInput = input;
    }
    
    void HandleAttackInput(string attackType)
    {
        if (isPerformingAttack || Time.time - lastAttackTime < attackCooldown)
            return;
            
        PerformAttack(attackType);
    }
    
    void HandleMovement()
    {
        Vector3 movement = new Vector3(moveInput.x, 0, moveInput.y);
        movement = transform.TransformDirection(movement);
        movement *= moveSpeed * Time.deltaTime;
        
        // Apply gravity
        movement.y = -9.81f * Time.deltaTime;
        
        characterController.Move(movement);
    }
    
    void HandleRotation()
    {
        if (lookInput != Vector2.zero)
        {
            float mouseX = lookInput.x * rotationSpeed * Time.deltaTime;
            transform.Rotate(Vector3.up * mouseX);
        }
    }
    
    void UpdateAnimations()
    {
        bool isMoving = moveInput.magnitude > 0.1f;
        animator.SetBool(isMovingHash, isMoving);
    }
    
    void PerformAttack(string attackType)
    {
        lastAttackTime = Time.time;
        isPerformingAttack = true;
        
        // Check if attack is on beat
        bool isOnBeat = beatmapReader != null && beatmapReader.IsOnBeat(Time.time);
        
        int animationHash = GetAnimationHash(attackType);
        
        if (isOnBeat)
        {
            // Enhanced attack
            PlayEnhancedAttack(animationHash);
        }
        else
        {
            // Fallback attack
            PlayFallbackAttack(animationHash);
        }
        
        // Reset attack state after animation
        Invoke(nameof(ResetAttackState), 1f);
    }
    
    int GetAnimationHash(string attackType)
    {
        return attackType switch
        {
            "Q" => waterSurfaceSlashHash,
            "E" => waterWheelHash,
            "R" => flowingDanceHash,
            _ => waterSurfaceSlashHash
        };
    }
    
    void PlayEnhancedAttack(int animationHash)
    {
        animator.SetTrigger(animationHash);
        
        // Enable sword trail
        if (swordTrail != null)
        {
            swordTrail.enabled = true;
            swordTrail.Clear();
            Invoke(nameof(DisableSwordTrail), 0.5f);
        }
        
        // Camera shake
        StartCoroutine(CameraShake(0.3f, 0.5f));
        
        // Particle effect
        if (hitParticles != null)
        {
            hitParticles.Play();
        }
        
        // Notify beatmap reader of successful hit
        if (beatmapReader != null)
        {
            beatmapReader.RegisterHit(true);
        }
    }
    
    void PlayFallbackAttack(int animationHash)
    {
        // Play stumble animation instead or slower version
        if (Random.value > 0.5f)
        {
            animator.SetTrigger(stumbleHash);
        }
        else
        {
            animator.SetTrigger(animationHash);
            animator.speed = 0.5f; // Slow down animation
            Invoke(nameof(ResetAnimationSpeed), 1f);
        }
        
        // Notify beatmap reader of missed hit
        if (beatmapReader != null)
        {
            beatmapReader.RegisterHit(false);
        }
    }
    
    void DisableSwordTrail()
    {
        if (swordTrail != null)
            swordTrail.enabled = false;
    }
    
    void ResetAnimationSpeed()
    {
        animator.speed = 1f;
    }
    
    void ResetAttackState()
    {
        isPerformingAttack = false;
    }
    
    System.Collections.IEnumerator CameraShake(float duration, float magnitude)
    {
        if (noise == null) yield break;
        
        float elapsed = 0f;
        
        while (elapsed < duration)
        {
            float strength = Mathf.Lerp(magnitude, 0f, elapsed / duration);
            noise.m_AmplitudeGain = strength;
            noise.m_FrequencyGain = strength;
            
            elapsed += Time.deltaTime;
            yield return null;
        }
        
        noise.m_AmplitudeGain = 0f;
        noise.m_FrequencyGain = 0f;
    }
    
    void OnDestroy()
    {
        if (inputHandler != null)
        {
            inputHandler.OnMoveInput -= HandleMoveInput;
            inputHandler.OnLookInput -= HandleLookInput;
            inputHandler.OnAttackInput -= HandleAttackInput;
        }
    }
}

