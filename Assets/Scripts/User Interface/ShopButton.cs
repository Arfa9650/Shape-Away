using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopButton : MonoBehaviour
{
    [SerializeField]
    string objectName;

    [SerializeField]
    Sprite greenButton;

    string currentCharacter;

    private void Start()
    {
        currentCharacter = PlayerPrefs.GetString("Character", "Boy");

        if (currentCharacter == objectName)
        {
            GetComponent<Image>().sprite = greenButton;
        }
    }
}
