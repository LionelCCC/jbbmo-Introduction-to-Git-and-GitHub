# Unity Third-Person Rhythm Game - Setup Instructions 🎮

## Quick Start Guide

### 1. Unity Project Setup
1. **Create New Unity Project**:
   - Unity 2022.3 LTS or newer
   - Use 3D template
   - Name: "ThirdPersonRhythmGame"

2. **Install Required Packages**:
   Open Package Manager (Window → Package Manager) and install:
   - **Input System** (com.unity.inputsystem)
   - **Cinemachine** (com.unity.cinemachine) 
   - **TextMeshPro** (com.unity.textmeshpro)

### 2. Import Scripts
Copy all scripts from the `Assets/Scripts/` folder into your Unity project's `Assets/Scripts/` folder.

### 3. Create Input Actions
1. Create folder: `Assets/InputActions/`
2. Copy `PlayerInputActions.inputactions` file
3. In Unity, select the file and click "Generate C# Class"

### 4. Scene Setup

#### A. Create Player Character
1. **Create Empty GameObject** → Name: "Player"
2. **Add Components**:
   - CharacterController
   - PlayerController script
   - InputHandler script
   - Assign the PlayerInputActions asset to InputHandler

3. **Create Stick Figure**:
   - Child object: "StickFigureBody" (Capsule primitive)
   - Child object: "Sword" (Cube primitive, scaled thin)
   - Add TrailRenderer to Sword object
   - Add ParticleSystem as child of Sword

4. **Setup Animator**:
   - Create Animator Controller: "PlayerAnimator"
   - Add trigger parameters: "WaterSurfaceSlash", "WaterWheel", "FlowingDance", "Stumble"
   - Add bool parameter: "IsMoving"

#### B. Setup Camera System
1. **Create Cinemachine Virtual Camera**:
   - GameObject → Cinemachine → Virtual Camera
   - Name: "PlayerFollowCamera"
   - Set Follow and LookAt to Player transform
   - Add CinemachineBasicMultiChannelPerlin to Noise section

2. **Camera Settings**:
   - Body: Transposer
   - Follow Offset: (0, 2, -5)
   - Aim: Composer
   - Tracked Object Offset: (0, 1.5, 0)

#### C. Audio Setup
1. **Create Empty GameObject** → Name: "AudioManager"
2. **Add AudioSource component**
3. **Place "Gurenge.mp3"** in `Assets/Resources/` folder
4. **Assign audio clip** to AudioSource
5. **Add BeatmapReader script** to AudioManager

#### D. UI Setup
1. **Create Canvas** (Screen Space - Overlay)
2. **Create Debug Panel**:
   ```
   Canvas
   └── DebugPanel (Panel)
       ├── SongTimeText (TextMeshPro)
       ├── BeatWindowText (TextMeshPro)  
       ├── ComboText (TextMeshPro)
       ├── MultiplierText (TextMeshPro)
       ├── OnBeatIndicator (TextMeshPro)
       └── BeatWindowSlider (Slider)
   ```
3. **Add UIDebugPanel script** to DebugPanel
4. **Link all UI elements** in the script inspector

#### E. Game Manager Setup
1. **Create Empty GameObject** → Name: "GameManager"
2. **Add GameManager script**
3. **Link references**:
   - PlayerController
   - BeatmapReader  
   - UIDebugPanel
   - DebugPanel GameObject

### 5. Configure Components

#### PlayerController Setup:
- **Move Speed**: 5
- **Rotation Speed**: 10
- **Camera Target**: Player transform
- **Animator**: Player's Animator component
- **Sword Trail**: Sword's TrailRenderer
- **Hit Particles**: Sword's ParticleSystem
- **Virtual Camera**: PlayerFollowCamera

#### BeatmapReader Setup:
- **Audio Source**: AudioManager's AudioSource
- **Beatmap File Name**: "beatmap.json"
- **Beat Window**: 0.2

#### InputHandler Setup:
- **Input Actions**: PlayerInputActions asset

### 6. Final Steps

1. **Create StreamingAssets folder**: `Assets/StreamingAssets/`
2. **Copy beatmap.json** to StreamingAssets folder
3. **Test the scene**:
   - Press Play
   - Use WASD to move
   - Mouse to look around
   - Q/E/R for attacks
   - F1 to toggle debug panel

### 7. Optional Enhancements

#### Visual Effects:
- Configure TrailRenderer with gradient colors
- Set up ParticleSystem with spark effects
- Add materials for sword and character

#### Audio:
- Add sword swing sound effects
- Configure audio mixer for better sound control

#### Animations:
- Create actual animation clips for attacks
- Set up blend trees for movement

## 🎮 Controls Reference

| Key | Action |
|-----|--------|
| W/A/S/D | Move character |
| Mouse | Camera rotation |
| Q | Water Surface Slash |
| E | Water Wheel |
| R | Flowing Dance |
| F1 | Toggle debug panel |
| P | Pause/Resume |
| Esc | Toggle cursor lock |

## 🐛 Troubleshooting

**Input not working?**
- Ensure Input System package is installed
- Check PlayerInputActions asset is assigned
- Verify "Generate C# Class" was clicked

**Camera not following?**
- Check Cinemachine package is installed
- Verify Follow and LookAt targets are set
- Ensure Virtual Camera has priority

**Audio not playing?**
- Verify Gurenge.mp3 is in Resources folder
- Check AudioSource has the clip assigned
- Ensure beatmap.json is in StreamingAssets folder

**Beat detection not working?**
- Check beatmap.json format is correct
- Verify BeatmapReader is finding the file
- Test with Space key for debug info

## 🚀 Ready to Play!

Once setup is complete, you should have a fully functional third-person rhythm game where timing your sword attacks to the beat creates enhanced visual effects and builds combos!

*Happy slicing! 🎵⚔️*

