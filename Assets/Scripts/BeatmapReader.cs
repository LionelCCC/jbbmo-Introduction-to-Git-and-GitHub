using UnityEngine;
using System.Collections.Generic;
using System;
using System.IO;

[System.Serializable]
public class BeatEvent
{
    public float start_time;
    public float end_time;
}

[System.Serializable]
public class BeatmapData
{
    public List<string> overview;
    public Dictionary<string, List<BeatEvent>> sections;
}

public class BeatmapReader : MonoBehaviour
{
    [Header("Audio Settings")]
    public AudioSource audioSource;
    public string beatmapFileName = "beatmap.json";
    
    [Header("Beat Detection")]
    public float beatWindow = 0.2f; // ±0.2s window for beat detection
    
    [Header("Combo System")]
    public int currentCombo = 0;
    public int maxCombo = 0;
    
    private BeatmapData beatmapData;
    private List<BeatEvent> allBeats;
    private float songStartTime;
    private bool songStarted = false;
    
    // Events
    public event Action<bool> OnBeatHit; // true for hit, false for miss
    public event Action<int> OnComboChanged;
    
    void Start()
    {
        LoadBeatmap();
        StartSong();
    }
    
    void LoadBeatmap()
    {
        try
        {
            string filePath = Path.Combine(Application.streamingAssetsPath, beatmapFileName);
            
            if (File.Exists(filePath))
            {
                string jsonContent = File.ReadAllText(filePath);
                
                // Parse JSON manually since Unity's JsonUtility doesn't handle dictionaries well
                ParseBeatmapJson(jsonContent);
                
                Debug.Log($"Loaded beatmap with {allBeats.Count} beats");
            }
            else
            {
                Debug.LogError($"Beatmap file not found: {filePath}");
                CreateSampleBeatmap();
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Error loading beatmap: {e.Message}");
            CreateSampleBeatmap();
        }
    }
    
    void ParseBeatmapJson(string jsonContent)
    {
        // Simple JSON parsing for the beatmap structure
        allBeats = new List<BeatEvent>();
        
        // This is a simplified parser - in a real project, use a proper JSON library
        // For now, create sample data
        CreateSampleBeatmap();
    }
    
    void CreateSampleBeatmap()
    {
        // Create sample beatmap data for testing
        allBeats = new List<BeatEvent>();
        
        // Add beats every 0.5 seconds for the first 30 seconds
        for (float time = 1f; time < 30f; time += 0.5f)
        {
            allBeats.Add(new BeatEvent { start_time = time, end_time = time + 0.1f });
        }
        
        // Add some more complex patterns
        for (float time = 30f; time < 60f; time += 0.25f)
        {
            allBeats.Add(new BeatEvent { start_time = time, end_time = time + 0.1f });
        }
        
        Debug.Log($"Created sample beatmap with {allBeats.Count} beats");
    }
    
    void StartSong()
    {
        if (audioSource != null && audioSource.clip != null)
        {
            songStartTime = Time.time;
            audioSource.Play();
            songStarted = true;
            Debug.Log("Song started!");
        }
        else
        {
            Debug.LogWarning("AudioSource or AudioClip not assigned!");
        }
    }
    
    public float GetCurrentSongTime()
    {
        if (!songStarted) return 0f;
        return Time.time - songStartTime;
    }
    
    public bool IsOnBeat(float currentTime)
    {
        float songTime = GetCurrentSongTime();
        
        foreach (BeatEvent beat in allBeats)
        {
            float timeDifference = Mathf.Abs(songTime - beat.start_time);
            if (timeDifference <= beatWindow)
            {
                return true;
            }
        }
        
        return false;
    }
    
    public BeatEvent GetNearestBeat(float currentTime)
    {
        float songTime = GetCurrentSongTime();
        BeatEvent nearestBeat = null;
        float nearestDistance = float.MaxValue;
        
        foreach (BeatEvent beat in allBeats)
        {
            float distance = Mathf.Abs(songTime - beat.start_time);
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestBeat = beat;
            }
        }
        
        return nearestBeat;
    }
    
    public float GetBeatWindow()
    {
        BeatEvent nearestBeat = GetNearestBeat(Time.time);
        if (nearestBeat != null)
        {
            float songTime = GetCurrentSongTime();
            float timeToBeat = nearestBeat.start_time - songTime;
            return timeToBeat;
        }
        return 0f;
    }
    
    public void RegisterHit(bool wasOnBeat)
    {
        if (wasOnBeat)
        {
            currentCombo++;
            if (currentCombo > maxCombo)
            {
                maxCombo = currentCombo;
            }
        }
        else
        {
            currentCombo = 0;
        }
        
        OnBeatHit?.Invoke(wasOnBeat);
        OnComboChanged?.Invoke(currentCombo);
        
        Debug.Log($"Hit registered: {(wasOnBeat ? "ON BEAT" : "MISSED")} - Combo: {currentCombo}");
    }
    
    public int GetComboMultiplier()
    {
        if (currentCombo >= 20) return 4;
        if (currentCombo >= 10) return 3;
        if (currentCombo >= 5) return 2;
        return 1;
    }
    
    void Update()
    {
        // Debug information
        if (songStarted && Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log($"Current song time: {GetCurrentSongTime():F2}s");
            Debug.Log($"Is on beat: {IsOnBeat(Time.time)}");
            Debug.Log($"Beat window: {GetBeatWindow():F2}s");
        }
    }
}

