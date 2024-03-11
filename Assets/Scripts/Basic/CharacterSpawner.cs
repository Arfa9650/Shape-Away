using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSpawner : MonoBehaviour
{
    string defaultCharacter = "Boy";

    string characterToSpawn;

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

        Instantiate(Resources.Load(@"Prefabs\Characters\" + characterToSpawn) as GameObject, new Vector2(-1.28f, 3.16f), Quaternion.identity, transform);
    }
}
