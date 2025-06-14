# Unity Third-Person Rhythm Game 🎵⚔️

A Unity-based third-person rhythm game featuring a stick figure character with sword combat mechanics synchronized to music beats.

## 🎮 Features

### Player Character
- **Stick Figure Warrior**: Placeholder character model wielding a sword
- **Movement Controls**: 
  - WASD for character movement
  - Mouse for camera control (third-person follow with rotation)
- **Combat System**: Three unique sword attacks mapped to Q, E, R keys
  - **Q**: Water Surface Slash (vertical attack)
  - **E**: Water Wheel (circular spin attack) 
  - **R**: Flowing Dance (fluid side-to-side attack)

### Rhythm Mechanics
- **Beatmap System**: Loads `beatmap.json` with timed beat events
- **Beat Detection**: ±0.2s window for perfect timing
- **Enhanced vs Fallback Attacks**:
  - **On-Beat**: Enhanced animations + camera shake + particle effects + sword trails
  - **Off-Beat**: Slower animations or stumble effects
- **Audio Sync**: Plays "Gurenge" track in sync with beatmap

### Visual Effects
- **Sword Trails**: TrailRenderer effects on successful hits
- **Particle Systems**: Spark bursts and attack-specific effects
- **Camera Shake**: Cinemachine-powered screen shake on enhanced hits
- **Dynamic Animations**: Procedural sword movement for each attack type

### UI & Debug Features
- **Debug Panel**: Real-time display of:
  - Current song time
  - Beat window timing (±0.2s)
  - Combo streak counter
  - Score multiplier (x1, x2, x3, x4)
  - On-beat indicator with color coding
- **Combo System**: Tracks consecutive on-beat hits with multipliers

## 📁 Project Structure

```
Assets/
├── Scripts/
│   ├── PlayerController.cs          # Main character controller
│   ├── InputHandler.cs              # Unity Input System handler
│   ├── BeatmapReader.cs             # Beatmap loading and beat detection
│   ├── UIDebugPanel.cs              # Debug UI management
│   ├── GameManager.cs               # Game state and flow control
│   └── SwordAnimationController.cs  # Sword animations and effects
├── Animations/
│   └── [Placeholder sword animations]
├── Resources/
│   ├── beatmap.json                 # Beat timing data
│   └── Gurenge.mp3                  # Background music
└── Prefabs/
    └── StickFigureCharacter.prefab  # Player character prefab
```

## 🔧 Setup Instructions

### Prerequisites
- Unity 2022.3 LTS or newer
- Unity Input System package
- Cinemachine package
- TextMeshPro package

### Installation
1. Clone this repository
2. Open the project in Unity
3. Install required packages via Package Manager:
   - Input System
   - Cinemachine
   - TextMeshPro
4. Place your "Gurenge.mp3" file in `Assets/Resources/`
5. Create placeholder animations for the sword attacks
6. Set up the scene with the player character and Cinemachine camera

### Scene Setup
1. **Player Character**:
   - Add CharacterController component
   - Attach PlayerController and InputHandler scripts
   - Set up Animator with attack triggers
   - Add TrailRenderer to sword object

2. **Camera System**:
   - Create Cinemachine Virtual Camera
   - Set to Follow and Look At the player
   - Add noise component for camera shake

3. **Audio**:
   - Add AudioSource to scene
   - Assign "Gurenge.mp3" to the AudioSource
   - Link to BeatmapReader script

4. **UI**:
   - Create Canvas with debug panel elements
   - Link UI elements to UIDebugPanel script

## 🎵 Beatmap Format

The `beatmap.json` file uses this structure:

```json
{
  "overview": ["intro", "verse_1", "chorus_1", ...],
  "intro": [
    { "start_time": 0.5, "end_time": 0.6 },
    { "start_time": 1.0, "end_time": 1.1 }
  ],
  "verse_1": [
    { "start_time": 3.0, "end_time": 3.1 },
    ...
  ]
}
```

## 🎮 Controls

| Input | Action |
|-------|--------|
| W/A/S/D | Character movement |
| Mouse | Camera rotation |
| Q | Water Surface Slash |
| E | Water Wheel |
| R | Flowing Dance |
| F1 | Toggle debug panel |
| P | Pause/Resume |
| R | Restart game |
| Space | Debug beat info (console) |

## 🏗️ Architecture

### Modular Design
- **PlayerController**: Handles movement, input processing, and attack coordination
- **InputHandler**: Manages Unity Input System events and input mapping
- **BeatmapReader**: Loads beat data, tracks timing, and manages combo system
- **SwordAnimationController**: Manages sword animations and visual effects
- **UIDebugPanel**: Provides real-time feedback and debugging information
- **GameManager**: Oversees game state and system coordination

### Extensibility
The system is designed for easy expansion:
- Add new attack types by extending the switch statements
- Create new beatmap sections by adding to the JSON structure
- Implement additional visual effects through the existing particle systems
- Expand combo system with new multiplier tiers

## 🚀 Future Enhancements

- **Additional Moves**: "Splashing Water Flow", "Constant Flux"
- **Advanced Combo System**: Chain attacks, combo multipliers
- **Multiple Characters**: Different fighting styles and movesets
- **Level Progression**: Unlock new attacks and abilities
- **Online Leaderboards**: Compare scores with other players
- **Custom Beatmaps**: User-generated content support

## 🐛 Debug Features

- **F1**: Toggle debug panel visibility
- **Console Logging**: Beat detection, combo changes, and system events
- **Visual Indicators**: Color-coded UI elements for timing feedback
- **Real-time Metrics**: Song time, beat windows, and performance stats

## 📝 Notes

- Uses Unity's new Input System for better input handling
- Cinemachine provides smooth camera movement and effects
- Modular script architecture allows for easy feature additions
- Sample beatmap included for immediate testing
- All animations are placeholder and can be replaced with custom assets

---

*Ready to slice to the beat! 🎵⚔️*

