using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
//using UnityEngine.Localization.Settings;

public class ChooseLanguage : MonoBehaviour
{
    [SerializeField] 
    GameObject mainCanvas;

    [SerializeField] 
    GameObject text1;

    [SerializeField] 
    GameObject text2;

    [SerializeField]
    Button[] buttonsToDisable;

    string language = "Nothing";
    
    float animTime = 0.5f;
    

    public void English()
    {
        language = "English";
        PlayerPrefs.SetString("Language", language);
        DisableOtherButtons();
        //Invoke("Disappear", animTime);
        StartCoroutine(SetLocale(1));
    }
    public void Urdu()
    {
        language = "Urdu";
        PlayerPrefs.SetString("Language", language);
        DisableOtherButtons();
        StartCoroutine(SetLocale(3));
    }
    public void Arabic()
    {
        language = "Arabic";
        PlayerPrefs.SetString("Language", language);
        DisableOtherButtons();
        StartCoroutine(SetLocale(0));
    }
    
    public void Hindi()
    {
        language = "Hindi";
        PlayerPrefs.SetString("Language", language);
        DisableOtherButtons();
        StartCoroutine(SetLocale(2));
    }

    void Disappear()
    {
        EnableButtons();
        //gameObject.SetActive(false);
        //mainCanvas.SetActive(true);
        text1.SetActive(true);
        text2.SetActive(true);
        SceneManager.LoadScene(0);
    }

    IEnumerator SetLocale(int localeId)
    {
        AudioManager.Play(AudioClipNames.Button);
        text1.SetActive(false);
        text2.SetActive(false);
        yield return new WaitForSeconds(animTime);
        //LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[localeId];
        //Invoke("Disappear", animTime);
        SceneManager.LoadScene(0);
    }

    private void DisableOtherButtons()
    {
        foreach (Button button in buttonsToDisable)
        {
            button.interactable = false; // Disable interactivity
        }
    }
    
    private void EnableButtons()
    {
        foreach (Button button in buttonsToDisable)
        {
            button.interactable = true; // Enable interactivity
        }
    }
}
