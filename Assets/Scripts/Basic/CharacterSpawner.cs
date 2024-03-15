using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSpawner : MonoBehaviour
{
    string defaultCharacter = "Boy";

    string characterToSpawn;

    [SerializeField]
    string path = @"Prefabs\Characters\";

    [SerializeField]
    Vector2 position = new Vector2(-1.28f, 3.16f);

    private void Start()
    {
        if(PlayerPrefs.HasKey("Character"))
        {
            characterToSpawn = PlayerPrefs.GetString("Character");
        }
        else
        {
            characterToSpawn = defaultCharacter;
        }

        Instantiate(Resources.Load(path + characterToSpawn) as GameObject, position, Quaternion.identity, transform);
    }
}
