using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    #region Fields

    [SerializeField]
    GameObject mainCanvas;

    [SerializeField]
    GameObject pauseCanvas;

    [SerializeField]
    GameObject settingsCanvas;

    public AudioClip defaultClip;

    public Sprite mutedSprite;
    public Sprite unmutedSprite;
    public Image musicButtonImage;
    public Image hapticsButtonImage;

    public Button[] buttonsToDisable;

    float animTime = 0.4f;

    #endregion

    #region Methods

    private void Start()
    {
        if (AudioManager.audioSource.clip != null && AudioManager.audioSource.clip != defaultClip)
        {
            AudioManager.audioSource.clip = defaultClip;
            AudioManager.audioSource.Play();
        }

        UpdateMusicButtonSprite();
        UpdateHapticsButtonSprite();
    }

    public void Pause()
    {
        mainCanvas.SetActive(false);
        pauseCanvas.SetActive(true);
    }
    
    public void Resume()
    {
        DisableOtherButtons();
        StartCoroutine(LoadScene(pauseCanvas, mainCanvas, false));
        Time.timeScale = 1;
    }
    
    public void Settings()
    {
        DisableOtherButtons();
        StartCoroutine(LoadScene(pauseCanvas, settingsCanvas, false));
    }
    
    public void Back()
    {
        StartCoroutine(LoadScene(settingsCanvas, pauseCanvas, false));
    }

    public void Exit()
    {
        Debug.Log("Exiting");
        AudioManager.FirstTime = true;
        DisableOtherButtons();
        StartCoroutine(LoadScene(0));
    }

    public void Music()
    {
        AudioManager.Play(AudioClipNames.Button);
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
        AudioManager.Play(AudioClipNames.Button);
        if (AudioManager.Initialized)
        {
            if(AudioManager.Haptics)
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

    private IEnumerator LoadScene(int index)
    {
        AudioManager.Play(AudioClipNames.Button);
        //Time.timeScale = 1;
        yield return new WaitForSeconds(animTime);
        SceneManager.LoadScene(index);
    }

    private IEnumerator LoadScene(GameObject from, GameObject to, bool needPause)
    {
        AudioManager.Play(AudioClipNames.Button);
        yield return new WaitForSeconds(animTime);
        EnableButtons();
        from.SetActive(false);
        to.SetActive(true);
        if(needPause)
        {
            Time.timeScale = 0;
        }
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

    #endregion
}
