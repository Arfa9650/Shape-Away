using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    [SerializeField]
    Sprite defaultButton;

    [SerializeField]
    Sprite greenButton;

    [SerializeField]
    Button maleButton;
    
    [SerializeField]
    Button femaleButton;

    public void SetMale()
    {
        PlayerPrefs.SetString("Character", "Boy");
        maleButton.GetComponent<Image>().sprite = greenButton;
        femaleButton.GetComponent<Image>().sprite = defaultButton;
    }
    
    public void SetFemale()
    {
        PlayerPrefs.SetString("Character", "Girl");
        femaleButton.GetComponent<Image>().sprite = greenButton;
        maleButton.GetComponent<Image>().sprite = defaultButton;
    }
}
