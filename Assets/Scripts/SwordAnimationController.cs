using UnityEngine;

public class SwordAnimationController : MonoBehaviour
{
    [Header("Animation Settings")]
    public Animator swordAnimator;
    public Transform swordTransform;
    
    [Header("Attack Animations")]
    public AnimationClip waterSurfaceSlash;
    public AnimationClip waterWheel;
    public AnimationClip flowingDance;
    public AnimationClip stumbleAnimation;
    
    [Header("Enhanced Effects")]
    public ParticleSystem slashParticles;
    public ParticleSystem spinParticles;
    public ParticleSystem danceParticles;
    public TrailRenderer swordTrail;
    
    [Header("Audio")]
    public AudioSource swordAudioSource;
    public AudioClip[] swordSounds;
    public AudioClip missSound;
    
    private PlayerController playerController;
    private BeatmapReader beatmapReader;
    
    // Animation state tracking
    private bool isAnimating = false;
    private string currentAnimation = "";
    
    void Start()
    {
        playerController = GetComponentInParent<PlayerController>();
        beatmapReader = FindObjectOfType<BeatmapReader>();
        
        // Initialize sword trail
        if (swordTrail != null)
        {
            swordTrail.enabled = false;
        }
        
        // Initialize particle systems
        InitializeParticleSystems();
    }
    
    void InitializeParticleSystems()
    {
        if (slashParticles != null) slashParticles.Stop();
        if (spinParticles != null) spinParticles.Stop();
        if (danceParticles != null) danceParticles.Stop();
    }
    
    public void PlayAttackAnimation(string attackType, bool isEnhanced)
    {
        if (isAnimating) return;
        
        isAnimating = true;
        currentAnimation = attackType;
        
        // Play appropriate animation and effects
        switch (attackType)
        {
            case "Q":
                PlayWaterSurfaceSlash(isEnhanced);
                break;
            case "E":
                PlayWaterWheel(isEnhanced);
                break;
            case "R":
                PlayFlowingDance(isEnhanced);
                break;
        }
        
        // Play audio
        PlaySwordAudio(isEnhanced);
        
        // Reset animation state after duration
        float animationDuration = GetAnimationDuration(attackType);
        Invoke(nameof(ResetAnimationState), animationDuration);
    }
    
    void PlayWaterSurfaceSlash(bool isEnhanced)
    {
        if (swordAnimator != null)
        {
            swordAnimator.SetTrigger("WaterSurfaceSlash");
        }
        
        if (isEnhanced)
        {
            // Enhanced effects
            EnableSwordTrail(0.5f);
            if (slashParticles != null)
            {
                slashParticles.Play();
            }
            
            // Add sword movement animation
            StartCoroutine(AnimateSwordSlash());
        }
        else
        {
            // Fallback animation - slower or stumble
            if (Random.value > 0.5f && swordAnimator != null)
            {
                swordAnimator.SetTrigger("Stumble");
            }
        }
    }
    
    void PlayWaterWheel(bool isEnhanced)
    {
        if (swordAnimator != null)
        {
            swordAnimator.SetTrigger("WaterWheel");
        }
        
        if (isEnhanced)
        {
            // Enhanced effects
            EnableSwordTrail(0.8f);
            if (spinParticles != null)
            {
                spinParticles.Play();
            }
            
            // Add spinning animation
            StartCoroutine(AnimateSwordSpin());
        }
        else
        {
            // Fallback animation
            if (Random.value > 0.5f && swordAnimator != null)
            {
                swordAnimator.SetTrigger("Stumble");
            }
        }
    }
    
    void PlayFlowingDance(bool isEnhanced)
    {
        if (swordAnimator != null)
        {
            swordAnimator.SetTrigger("FlowingDance");
        }
        
        if (isEnhanced)
        {
            // Enhanced effects
            EnableSwordTrail(1f);
            if (danceParticles != null)
            {
                danceParticles.Play();
            }
            
            // Add flowing dance animation
            StartCoroutine(AnimateSwordDance());
        }
        else
        {
            // Fallback animation
            if (Random.value > 0.5f && swordAnimator != null)
            {
                swordAnimator.SetTrigger("Stumble");
            }
        }
    }
    
    void EnableSwordTrail(float duration)
    {
        if (swordTrail != null)
        {
            swordTrail.enabled = true;
            swordTrail.Clear();
            Invoke(nameof(DisableSwordTrail), duration);
        }
    }
    
    void DisableSwordTrail()
    {
        if (swordTrail != null)
        {
            swordTrail.enabled = false;
        }
    }
    
    void PlaySwordAudio(bool isEnhanced)
    {
        if (swordAudioSource == null) return;
        
        if (isEnhanced && swordSounds.Length > 0)
        {
            AudioClip clipToPlay = swordSounds[Random.Range(0, swordSounds.Length)];
            swordAudioSource.PlayOneShot(clipToPlay);
        }
        else if (!isEnhanced && missSound != null)
        {
            swordAudioSource.PlayOneShot(missSound);
        }
    }
    
    float GetAnimationDuration(string attackType)
    {
        return attackType switch
        {
            "Q" => 0.5f, // Water Surface Slash
            "E" => 0.8f, // Water Wheel
            "R" => 1f,   // Flowing Dance
            _ => 0.5f
        };
    }
    
    void ResetAnimationState()
    {
        isAnimating = false;
        currentAnimation = "";
    }
    
    // Coroutines for sword movement animations
    System.Collections.IEnumerator AnimateSwordSlash()
    {
        if (swordTransform == null) yield break;
        
        Vector3 startRotation = swordTransform.localEulerAngles;
        Vector3 slashRotation = startRotation + new Vector3(-90f, 0f, 0f);
        
        float duration = 0.3f;
        float elapsed = 0f;
        
        // Slash down
        while (elapsed < duration)
        {
            float t = elapsed / duration;
            swordTransform.localEulerAngles = Vector3.Lerp(startRotation, slashRotation, t);
            elapsed += Time.deltaTime;
            yield return null;
        }
        
        // Return to start
        elapsed = 0f;
        while (elapsed < duration)
        {
            float t = elapsed / duration;
            swordTransform.localEulerAngles = Vector3.Lerp(slashRotation, startRotation, t);
            elapsed += Time.deltaTime;
            yield return null;
        }
        
        swordTransform.localEulerAngles = startRotation;
    }
    
    System.Collections.IEnumerator AnimateSwordSpin()
    {
        if (swordTransform == null) yield break;
        
        Vector3 startRotation = swordTransform.localEulerAngles;
        float duration = 0.8f;
        float elapsed = 0f;
        
        while (elapsed < duration)
        {
            float t = elapsed / duration;
            float spinAmount = t * 720f; // Two full rotations
            swordTransform.localEulerAngles = startRotation + new Vector3(0f, 0f, spinAmount);
            elapsed += Time.deltaTime;
            yield return null;
        }
        
        swordTransform.localEulerAngles = startRotation;
    }
    
    System.Collections.IEnumerator AnimateSwordDance()
    {
        if (swordTransform == null) yield break;
        
        Vector3 startRotation = swordTransform.localEulerAngles;
        float duration = 1f;
        float elapsed = 0f;
        
        while (elapsed < duration)
        {
            float t = elapsed / duration;
            float waveX = Mathf.Sin(t * Mathf.PI * 4) * 30f; // Side to side
            float waveY = Mathf.Sin(t * Mathf.PI * 2) * 15f; // Up and down
            swordTransform.localEulerAngles = startRotation + new Vector3(waveY, waveX, 0f);
            elapsed += Time.deltaTime;
            yield return null;
        }
        
        swordTransform.localEulerAngles = startRotation;
    }
}

