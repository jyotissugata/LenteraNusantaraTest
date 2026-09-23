# Lentera Nusantara - Game Programmer Test Case

This repository contains the completed technical test case for the Game Programmer position at Lentera Nusantara. The project demonstrates a scalable interaction system bridging 3D exploration and 2D UI puzzles, built with a clean and decoupled architecture.

## 🎮 Links
- **Video Demo & Playable Build:** [Google Drive Link](https://drive.google.com/drive/folders/1rxmObB05KSR0Q-vMWnwd0uaoYTAywr3L?usp=sharing)

## ✨ Core Features
All required features have been successfully implemented:
- **3D Exploration:** Camera-relative character movement (Walk and Sprint) with smooth animations.
- **Object Interaction:** Proximity-based interaction detection with visual feedback (VFX spark).
- **2D Puzzle Integration:** Interacting with specific 3D objects smoothly transitions the game into a 2D UI puzzle mode.
- **Input Management:** Character movement and 3D interactions are completely frozen while a puzzle is active using the New Input System's Action Maps.
- **Puzzle 1 (Memory Match):** A fully functional card-matching game featuring shuffling, pair validation, and DOTween flip animations.
- **Puzzle 2 (Numpad Passcode):** A digit-input puzzle with validation, error shake feedback, and dynamic hint generation.
- **State Restoration:** Canceling or completing a puzzle seamlessly returns the player to the 3D exploration state.

## 🏗️ Architecture & Code Structure (Poin Plus)
To ensure the codebase is modular, scalable, and easy to maintain, the following architectural patterns were used:
- **Model-View-Controller (MVC):** Puzzles (Memory Match and Numpad) strictly separate logic (Model), visuals (View), and input handling (Controller).
- **ScriptableObject-Based Event Channels:** Used for decoupled communication between systems (e.g., triggering puzzles, completing puzzles, HUD updates) without tight Singleton coupling.
- **Hierarchical State Machine:** 
  - **Game State:** Manages high-level modes (`ExplorationState` vs `PuzzleState`) and handles Input Action Map switching.
  - **Character State:** A highly modular, ScriptableObject-driven state machine (`LoopStateSO`, `ActionStateSO`) handling player locomotion and animation transitions seamlessly.

## 🕹️ Controls
| Action | Key / Input |
| :--- | :--- |
| **Move** | `W`, `A`, `S`, `D` or `Arrow Keys` |
| **Sprint** | Hold `Left Shift` |
| **Interact** | `E` (when near an interactable object) |
| **Cancel/Exit Puzzle** | `Esc` |
| **Puzzle Input** | `Mouse Left Click` |

## 📦 Assets & Plugins Used
- **3D Environment & Props:** POLYGON Starter Pack by Synty Studios.
- **Characters:** Synty Modular Characters.
- **Visual Effects:** Simple FX - Cartoon Particles.
- **Animation / UI Tweening:** [DOTween (HOTween v2)](http://dotween.demigiant.com/) for smooth UI fades, camera transitions, and object success animations.
- **Input:** Unity New Input System.

---
*Developed by Jyotis Sugata.*
