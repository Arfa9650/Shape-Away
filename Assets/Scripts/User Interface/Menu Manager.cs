using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Localization.Settings;

public class MenuManager : MonoBehaviour
{
    #region Fields

    [SerializeField]
    GameObject mainCanvas;
    
    [SerializeField]
    GameObject settingsCanvas;

    [SerializeField]
    GameObject languageSelector;

    [SerializeField]
    GameObject ratings;

    [SerializeField]
    GameObject startingLanguageSelector;

    [SerializeField]
    Button[] buttonsToDisable;

    float animTime = 0.5f;

    string language = "Nothing";

    public AudioClip defaultClip;

    public Sprite mutedSprite;
    public Sprite unmutedSprite;
    public Image musicButtonImage;
    public Image hapticsButtonImage;

    #endregion

    #region Methods

    private void Awake()
    {
        Application.targetFrameRate = 61;

        if(!PlayerPrefs.HasKey("Language"))
        {
            startingLanguageSelector.SetActive(true);
            mainCanvas.SetActive(false);
        }
        else
        {
            language = PlayerPrefs.GetString("Language");
        }
    }

    private void Start()
    {
        

        switch (language)
        {
            case "English":
                StartCoroutine(SetLocale(1));
                break;
            case "Urdu":
                StartCoroutine(SetLocale(3));
                break;
            case "Arabic":
                StartCoroutine(SetLocale(0));
                break;
            case "Hindi":
                StartCoroutine(SetLocale(2));
                break;
            default:
                StartCoroutine(SetLocale(1));
                break;
        }

        UpdateMusicButtonSprite();
        UpdateHapticsButtonSprite();

        if (AudioManager.audioSource.clip != null && AudioManager.audioSource.clip != defaultClip)
        {
            AudioManager.audioSource.clip = defaultClip;
            AudioManager.audioSource.Play();
        }
    }

    IEnumerator SetLocale(int localeId)
    {
        yield return LocalizationSettings.InitializationOperation;
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[localeId];
    }

    public void PlayButton()
    {
        DisableOtherButtons();
        StartCoroutine(LoadScene(1));
    }

    public void Settings()
    {
        DisableOtherButtons();
        StartCoroutine(LoadScene(mainCanvas, settingsCanvas));
    }

    public void LanguageSelector()
    {
        DisableOtherButtons();
        StartCoroutine(LoadScene(settingsCanvas, languageSelector));
    }

    public void Ratings()
    {
        DisableOtherButtons();
        StartCoroutine(LoadScene(mainCanvas, ratings));
    }

    public void RateUs()
    {
        Application.OpenURL("https://play.google.com/store/apps/details?id=com.k2xtech.brainbudspuzzles");
    }

    public void Back()
    {
        DisableOtherButtons();
        StartCoroutine(LoadScene(languageSelector, settingsCanvas)); 
    }

    public void Backtomainmenu()
    {
        DisableOtherButtons();
        StartCoroutine(LoadScene(settingsCanvas, mainCanvas));
    }
    
    public void Backtomainmenutwo()
    {
        StartCoroutine(LoadScene(ratings, mainCanvas));
    }

    public void Shop()
    {
        DisableOtherButtons();
        StartCoroutine(LoadScene(3));
    }

    private IEnumerator LoadScene(int index)
    {
        AudioManager.Play(AudioClipNames.Button);
        yield return new WaitForSeconds(animTime);
        SceneManager.LoadScene(index);
    }
    
    private IEnumerator LoadScene(GameObject from, GameObject to)
    {
        AudioManager.Play(AudioClipNames.Button);
        yield return new WaitForSeconds(animTime);
        EnableButtons();
        from.SetActive(false);
        to.SetActive(true);
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

    public void Music()
    {
        if (AudioManager.Initialized)
        {
            if (AudioManager.audioSource.clip == null)
            {
                AudioManager.audioSource.clip = defaultClip;
                AudioManager.audioSource.Play();
            }
            else
            {
                AudioManager.audioSource.clip = null;
            }
        }
        UpdateMusicButtonSprite();
    }

    public void Haptics()
    {
        if (AudioManager.Initialized)
        {
            if (AudioManager.Haptics)
            {
                AudioManager.Haptics = false;
            }
            else
            {
                AudioManager.Haptics = true;
            }
        }
        UpdateHapticsButtonSprite();
    }

    private void UpdateMusicButtonSprite()
    {
        if (AudioManager.audioSource.clip == null)
        {
            musicButtonImage.sprite = mutedSprite;
        }
        else
        {
            musicButtonImage.sprite = unmutedSprite;
        }
    }

    private void UpdateHapticsButtonSprite()
    {
        if (AudioManager.Haptics)
        {
            hapticsButtonImage.sprite = unmutedSprite;
        }
        else
        {
            hapticsButtonImage.sprite = mutedSprite;
        }
    }

    #endregion
}
