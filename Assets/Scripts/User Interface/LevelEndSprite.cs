using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelEndSprite : MonoBehaviour
{
    [SerializeField]
    Sprite boy;

    [SerializeField]
    Sprite girl;

    string character;

    private void Start()
    {
        character = PlayerPrefs.GetString("Character", "Boy");

        switch(character)
        {
            case "Boy":
                GetComponent<Image>().sprite = boy;
                break;

            case "Girl":
                GetComponent<Image>().sprite = girl;
                break;
        }
        //GetComponent<Image>().SetNativeSize();
    }
}
