# Brainbuds Puzzles

A 2D mobile puzzle game for young children, built in Unity. Players drag shapes into matching
slots and reassemble sliced animal pictures, with difficulty that ramps up automatically as
levels are cleared.

- **Engine:** Unity `2022.3.16f1`
- **Publisher:** K2X Technologies
- **Version:** `0.4.0`
- **Platform:** Android (`com.k2xtech.brainbudspuzzles`, min SDK 22) — iOS project settings exist but are not configured
- **Repo name:** `Shape-Away` (the product was later renamed to Brainbuds Puzzles)

## Getting started

1. Install Unity **2022.3.16f1** via Unity Hub (the exact version is pinned in
   [ProjectVersion.txt](ProjectSettings/ProjectVersion.txt)).
2. Add the Android Build Support module, including the SDK/NDK and OpenJDK.
3. Clone the repo and open the root folder as a Unity project. Package resolution and the
   Google Mobile Ads dependency resolver will run on first import.
4. Open [Assets/_Scenes/Menu.unity](Assets/_Scenes/Menu.unity) and press Play. Starting from
   `Menu` matters — the other scenes read `PlayerPrefs` state that the menu sets up.

To play a gameplay scene directly, open [Shapes Scene](Assets/_Scenes/Shapes%20Scene.unity) or
[Animals Scene](Assets/_Scenes/Animals%20Scene.unity); they will start at whatever difficulty is
currently stored in `PlayerPrefs`.

## Game modes

### Shapes

The core mode. A grid of shape-shaped "trigger" outlines is spawned, and shapes are handed to the
player one at a time from the bottom of the screen. Drag a shape onto its matching outline to fit
it; a mismatch plays a fail sound and vibrates the device. Clearing every slot completes the level.

Eleven shapes are supported ([ShapeName](Assets/Scripts/Enums/Shape%20Name.cs)): circle, square,
triangle, crystal, semicircle, hexagon, quadrant, diamond, crystal two, trapezoid, trapezoid two.

Difficulty is a single stored integer that increments on every win:

| Difficulty | Behaviour |
| --- | --- |
| 1–2 | Slots spawn unrotated; shape count equals the difficulty |
| 3 | Rotation tutorial — a demo canvas pauses the game until dismissed |
| 4+ | Slots spawn at random 90° rotations; shapes must be tapped to rotate before they fit |

Shape count is capped at 9 (a 3×3 grid). Rotation is only accepted when the shape is near its
resting position, and each shape type has its own allowed rotation set — see
[Movable Shape.cs](Assets/Scripts/Basic/Movable%20Shape.cs) and
[RandSpawner.cs](Assets/Scripts/Basic/RandSpawner.cs).

### Animals

A jigsaw mode. A source picture is loaded from `Resources/Sprites/Write Sprites/` and sliced at
runtime into an X×Y grid by [SpriteSlicer.cs](Assets/Scripts/Animal%20Exclusives/SpriteSlicer.cs).
The greyed-out full picture sits underneath as a guide, and pieces are dragged back onto it.
Grid dimensions grow alternately in X and Y with the `Animals` difficulty counter, capped at 4×6.

### Shared flow

Both modes track their own level counter and their own "milestone" ceiling (`Maximum` /
`AnimalMaximum`). Reaching the ceiling plays an applause celebration screen and doubles the
ceiling; otherwise the next level loads immediately. If the player stalls for 15 seconds, a
pointing-hand hint appears.

## Project layout

```
Assets/
  _Scenes/            Menu, Level Screen, Shapes Scene, Shop, Animals Scene, Testing
  Scripts/
    Basic/            Shapes-mode gameplay: GameManager, RandSpawner, MovableShape, Hand, Character
    Animal Exclusives/ Jigsaw mode: AnimalsManager, SpriteSlicer, Puzzle, Piece, PuzzleHand
    Events/           EventManager plus the FitShape / FailToFit / GameOver events
    User Interface/   Menus, level select, shop, transitions, localization
    Audio/            AudioManager and clip name enum
    Ads/              AdMob initializer, banner, interstitial
    Utilities/        ViewportHandler, TutorialController, CameraAnchor
    Enums/            ShapeName, ShapeColors
  Resources/          Prefabs (shapes, triggers, characters), sprites, audio, fonts, languages
  Plugins/            Android/iOS native libs for Google Mobile Ads
  Editor/             Mobile notifications editor support
ProjectSettings/      Unity project configuration
Packages/             UPM manifest
```

Note that scenes and script folders use spaces in their names, so paths need quoting on the
command line.

### Event system

Gameplay is decoupled through a small static hub in
[EventManager.cs](Assets/Scripts/Events/EventManager.cs). Components extend
[IntEventInvoker](Assets/Scripts/Events/IntEventInvoker.cs), register themselves as invokers, and
other components subscribe as listeners. Three events exist: `FitShape`, `FailToFit`, `GameOver`.
`EventManager.Initialize()` is called from `Awake` in each mode's spawner to clear stale
subscriptions on scene load.

### Saved state

All progress lives in `PlayerPrefs` — there is no save file or backend.

| Key | Meaning |
| --- | --- |
| `Difficulty` | Current Shapes-mode level (default 1) |
| `Maximum` | Shapes-mode celebration milestone, doubles each time it is hit (default 10) |
| `Animals` | Current Animals-mode level (default 1) |
| `AnimalMaximum` | Animals-mode celebration milestone (default 10) |
| `Character` | Selected avatar, `"Boy"` or `"Girl"` |
| `Language` | Selected language name |
| `RemoveAds` | Present when ads are disabled |

To reset progress during development, use `Edit → Clear All PlayerPrefs` or call
`PlayerPrefs.DeleteAll()`.

## Localization

Uses Unity Localization (`com.unity.localization`) with Addressables-backed string and asset
tables. Four locales ship: English (en-US), Arabic (ar), Hindi (hi-IN), and Urdu (ur-PK). On first
launch the menu shows a language picker; the choice is stored in `PlayerPrefs` and mapped to a
locale index in [Menu Manager.cs](Assets/Scripts/User%20Interface/Menu%20Manager.cs).

## Ads

Google Mobile Ads (AdMob) serves a bottom banner on every scene load plus interstitials, unless
the `RemoveAds` pref is set.

The editor and development builds always serve Google's test ad units, so running the game while
developing never generates live impressions. Release builds read the real units from
[AdUnits.cs](Assets/Scripts/Ads/AdUnits.cs). iOS is not a configured target, so its entries there
are still test units.

## Building for Android

The build scene list is defined in
[EditorBuildSettings.asset](ProjectSettings/EditorBuildSettings.asset). Enabled scenes, in build
index order:

| Index | Scene |
| --- | --- |
| 0 | Menu |
| 1 | Level Screen |
| 2 | Shapes Scene |
| 3 | Shop |
| 4 | Animals Scene |

Several scripts call `SceneManager.LoadScene` with these raw indices, so **reordering or removing
a scene will break navigation** unless the call sites are updated too.

`File → Build Settings → Android → Build`.

Release builds are signed with a keystore that is **not** in this repo — `*.keystore` and `*.jks`
are gitignored. Place your own keystore in the project root and point at it under
`Player Settings → Publishing Settings`, supplying the keystore and key passwords there. Without
one, Unity falls back to a debug signing key, which Play Store will not accept.

## Notes

- `Shapes_BurstDebugInformation_DoNotShip/` is Burst compiler output. As the name says, it is not
  meant to be shipped and can be deleted safely.
- The `Testing` scene is a scratch scene and is not part of the build.
- [ViewportHandler.cs](Assets/Scripts/Utilities/ViewportHandler.cs) is third-party (MIT,
  © 2014 Marcel Căşvan) and keeps the orthographic camera framing consistent across aspect ratios.
