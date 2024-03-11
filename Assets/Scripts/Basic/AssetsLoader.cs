using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetsLoader : MonoBehaviour
{
    // Folder path where sprite assets are located
    private static string spriteFolderPath = "Sprites/Text Sprites";

    private void Start()
    {
        LoadSprites();
    }

    private void LoadSprites()
    {
        // Load all sprite assets from the specified folder
        Sprite[] sprites = Resources.LoadAll<Sprite>(spriteFolderPath);

        // Check if any sprites were loaded
        if (sprites.Length == 0)
        {
            Debug.LogWarning("No sprites found in folder: " + spriteFolderPath);
            return;
        }

        // Process loaded sprites
        foreach (Sprite sprite in sprites)
        {
            // Do something with the loaded sprite (e.g., add to a list, display in UI, etc.)
            Debug.Log("Loaded sprite: " + sprite.name);
        }
    }
}
