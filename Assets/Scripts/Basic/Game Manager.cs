using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : IntEventInvoker
{
    #region Fields

    [SerializeField]
    GameObject hand;

    [SerializeField]
    GameObject mainCanvas;

    [SerializeField]
    GameObject levelComplete;

    [SerializeField]
    AudioClip applause;

    [SerializeField]
    GameObject transition;

    [SerializeField]
    GameObject particles;

    int shapesInScene;

    int events = 0;

    int previous = 0;

    const float waitForEvents = 15f;

    float elapsedTime = 0f;

    int difficulty;

    #endregion

    #region Methods

    private void Start()
    {
        difficulty = PlayerPrefs.GetInt("Difficulty", 1);
        shapesInScene = difficulty > 9 ? 9 : difficulty;
        if (shapesInScene > 9)
            shapesInScene = 9;

        unityEvents.Add(EventNames.GameOver, new GameOver());
        EventManager.AddInvoker(EventNames.GameOver, this);

        EventManager.AddListener(EventNames.FitShape, AddEvent);
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;

        if(elapsedTime >= waitForEvents)
        {
            hand.SetActive(true);
            elapsedTime = 0;
        }
    }

    void AddEvent(int num)
    {
        events++;
        elapsedTime = 0;
        if(events >= shapesInScene)
        {
            AudioManager.Play(AudioClipNames.LevelComplete);
            //mainCanvas.SetActive(false);

            if (PlayerPrefs.HasKey("Difficulty"))
            {
                int level = PlayerPrefs.GetInt("Difficulty");
                PlayerPrefs.SetInt("Difficulty", level + 1);
            }
            else
            {
                PlayerPrefs.SetInt("Difficulty", 2);
            }

            unityEvents[EventNames.GameOver].Invoke(PlayerPrefs.GetInt("Difficulty"));
            StartCoroutine(GameOver());
        }
    }

    IEnumerator GameOver()
    {
        yield return new WaitForSeconds(2.1f);

        int max = PlayerPrefs.GetInt("Maximum", 10);

        if (PlayerPrefs.GetInt("Difficulty", 1) < max)
            SceneManager.LoadScene(2);
        else
        {
            PlayerPrefs.SetInt("Maximum", max * 2);
            AudioManager.audioSource.clip = applause;
            AudioManager.audioSource.loop = false;
            AudioManager.audioSource.Play();
            mainCanvas.SetActive(false);
            levelComplete.SetActive(true);
        }
    }

    private IEnumerator LoadScene(int index)
    {
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(index);
    }

    public void Restart()
    {
        AudioManager.Play(AudioClipNames.Button);
        //particles.SetActive(false);
        /*if (PlayerPrefs.HasKey("Difficulty"))
        {
            int level = PlayerPrefs.GetInt("Difficulty");
            PlayerPrefs.SetInt("Difficulty", level + 1);
        }
        else
        {
            PlayerPrefs.SetInt("Difficulty", 2);
        }*/
        AudioManager.FirstTime = true;
        StartCoroutine(LoadScene(2));
    }

    IEnumerator Transition(int level)
    {
        transition.SetActive(true);
        yield return new WaitForSeconds(1.1f);
        AudioManager.audioSource.loop = true;
        SceneManager.LoadScene(level);
    }

    #endregion
}
