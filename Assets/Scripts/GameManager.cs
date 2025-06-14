using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Game Settings")]
    public bool debugMode = true;
    public GameObject debugPanel;
    
    [Header("References")]
    public PlayerController playerController;
    public BeatmapReader beatmapReader;
    public UIDebugPanel uiDebugPanel;
    
    [Header("Game State")]
    public bool gameStarted = false;
    public float gameTime = 0f;
    
    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameManager>();
            }
            return instance;
        }
    }
    
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        InitializeGame();
    }
    
    void Start()
    {
        StartGame();
    }
    
    void Update()
    {
        if (gameStarted)
        {
            gameTime += Time.deltaTime;
        }
        
        HandleDebugInput();
    }
    
    void InitializeGame()
    {
        // Set up game settings
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 1;
        
        // Enable/disable debug panel
        if (debugPanel != null)
        {
            debugPanel.SetActive(debugMode);
        }
        
        Debug.Log("Game Manager initialized");
    }
    
    void StartGame()
    {
        gameStarted = true;
        gameTime = 0f;
        
        // Initialize all systems
        if (beatmapReader != null)
        {
            beatmapReader.OnBeatHit += OnBeatHit;
            beatmapReader.OnComboChanged += OnComboChanged;
        }
        
        Debug.Log("Game started!");
    }
    
    void HandleDebugInput()
    {
        if (!debugMode) return;
        
        // Toggle debug panel with F1
        if (Input.GetKeyDown(KeyCode.F1))
        {
            if (debugPanel != null)
            {
                debugPanel.SetActive(!debugPanel.activeSelf);
            }
        }
        
        // Restart game with R
        if (Input.GetKeyDown(KeyCode.R))
        {
            RestartGame();
        }
        
        // Pause/unpause with P
        if (Input.GetKeyDown(KeyCode.P))
        {
            TogglePause();
        }
    }
    
    void OnBeatHit(bool wasOnBeat)
    {
        // Handle global beat hit events
        if (wasOnBeat)
        {
            Debug.Log("Perfect hit!");
        }
        else
        {
            Debug.Log("Missed beat!");
        }
    }
    
    void OnComboChanged(int newCombo)
    {
        // Handle combo changes
        if (newCombo > 0 && newCombo % 10 == 0)
        {
            Debug.Log($"Combo milestone reached: {newCombo}!");
        }
    }
    
    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
    public void TogglePause()
    {
        if (Time.timeScale == 0f)
        {
            Time.timeScale = 1f;
            Debug.Log("Game resumed");
        }
        else
        {
            Time.timeScale = 0f;
            Debug.Log("Game paused");
        }
    }
    
    public void QuitGame()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
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

