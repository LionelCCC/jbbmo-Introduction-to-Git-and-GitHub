using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIDebugPanel : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI songTimeText;
    public TextMeshProUGUI beatWindowText;
    public TextMeshProUGUI comboText;
    public TextMeshProUGUI multiplierText;
    public TextMeshProUGUI onBeatIndicator;
    public Slider beatWindowSlider;
    public Image beatWindowFill;
    
    [Header("Colors")]
    public Color onBeatColor = Color.green;
    public Color offBeatColor = Color.red;
    public Color neutralColor = Color.white;
    
    private BeatmapReader beatmapReader;
    private PlayerController playerController;
    
    void Start()
    {
        beatmapReader = FindObjectOfType<BeatmapReader>();
        playerController = FindObjectOfType<PlayerController>();
        
        if (beatmapReader != null)
        {
            beatmapReader.OnBeatHit += OnBeatHit;
            beatmapReader.OnComboChanged += OnComboChanged;
        }
        
        // Initialize UI
        if (beatWindowSlider != null)
        {
            beatWindowSlider.minValue = -0.2f;
            beatWindowSlider.maxValue = 0.2f;
            beatWindowSlider.value = 0f;
        }
    }
    
    void Update()
    {
        UpdateDebugInfo();
    }
    
    void UpdateDebugInfo()
    {
        if (beatmapReader == null) return;
        
        // Update song time
        float songTime = beatmapReader.GetCurrentSongTime();
        if (songTimeText != null)
        {
            songTimeText.text = $"Song Time: {songTime:F2}s";
        }
        
        // Update beat window
        float beatWindow = beatmapReader.GetBeatWindow();
        if (beatWindowText != null)
        {
            beatWindowText.text = $"Beat Window: {beatWindow:F2}s";
        }
        
        // Update beat window slider
        if (beatWindowSlider != null)
        {
            beatWindowSlider.value = Mathf.Clamp(beatWindow, -0.2f, 0.2f);
            
            // Color the slider based on beat proximity
            if (beatWindowFill != null)
            {
                float normalizedDistance = Mathf.Abs(beatWindow) / 0.2f;
                if (normalizedDistance <= 1f)
                {
                    beatWindowFill.color = Color.Lerp(onBeatColor, neutralColor, normalizedDistance);
                }
                else
                {
                    beatWindowFill.color = offBeatColor;
                }
            }
        }
        
        // Update on-beat indicator
        bool isOnBeat = beatmapReader.IsOnBeat(Time.time);
        if (onBeatIndicator != null)
        {
            onBeatIndicator.text = isOnBeat ? "ON BEAT!" : "Off Beat";
            onBeatIndicator.color = isOnBeat ? onBeatColor : offBeatColor;
        }
        
        // Update combo and multiplier
        if (comboText != null)
        {
            comboText.text = $"Combo: {beatmapReader.currentCombo}";
        }
        
        if (multiplierText != null)
        {
            int multiplier = beatmapReader.GetComboMultiplier();
            multiplierText.text = $"Multiplier: x{multiplier}";
            
            // Color based on multiplier
            Color multiplierColor = multiplier switch
            {
                4 => Color.magenta,
                3 => Color.yellow,
                2 => Color.cyan,
                _ => Color.white
            };
            multiplierText.color = multiplierColor;
        }
    }
    
    void OnBeatHit(bool wasOnBeat)
    {
        // Visual feedback for hits/misses
        if (onBeatIndicator != null)
        {
            StartCoroutine(FlashIndicator(wasOnBeat));
        }
    }
    
    void OnComboChanged(int newCombo)
    {
        // Animate combo text when it changes
        if (comboText != null)
        {
            StartCoroutine(AnimateComboText());
        }
    }
    
    System.Collections.IEnumerator FlashIndicator(bool wasOnBeat)
    {
        if (onBeatIndicator == null) yield break;
        
        Color originalColor = onBeatIndicator.color;
        Color flashColor = wasOnBeat ? onBeatColor : offBeatColor;
        
        // Flash effect
        for (int i = 0; i < 3; i++)
        {
            onBeatIndicator.color = flashColor;
            yield return new WaitForSeconds(0.1f);
            onBeatIndicator.color = originalColor;
            yield return new WaitForSeconds(0.1f);
        }
    }
    
    System.Collections.IEnumerator AnimateComboText()
    {
        if (comboText == null) yield break;
        
        Vector3 originalScale = comboText.transform.localScale;
        Vector3 targetScale = originalScale * 1.2f;
        
        // Scale up
        float elapsed = 0f;
        float duration = 0.1f;
        
        while (elapsed < duration)
        {
            float t = elapsed / duration;
            comboText.transform.localScale = Vector3.Lerp(originalScale, targetScale, t);
            elapsed += Time.deltaTime;
            yield return null;
        }
        
        // Scale back down
        elapsed = 0f;
        while (elapsed < duration)
        {
            float t = elapsed / duration;
            comboText.transform.localScale = Vector3.Lerp(targetScale, originalScale, t);
            elapsed += Time.deltaTime;
            yield return null;
        }
        
        comboText.transform.localScale = originalScale;
    }
    
    void OnDestroy()
    {
        if (beatmapReader != null)
        {
            beatmapReader.OnBeatHit -= OnBeatHit;
            beatmapReader.OnComboChanged -= OnComboChanged;
        }
    }
}

