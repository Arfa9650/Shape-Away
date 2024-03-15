using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCharacter : MonoBehaviour
{
    [SerializeField]
    GameObject boy;

    [SerializeField]
    GameObject girl;

    string character;

    private void Start()
    {
        character = PlayerPrefs.GetString("Character", "Boy");

        switch (character)
        {
            case "Boy":
                boy.SetActive(true);
                break;

            case "Girl":
                girl.SetActive(true);
                break;
        }
    }
}
