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

    int shapesInScene;

    int events = 0;

    int previous = 0;

    const float waitForEvents = 10f;

    float elapsedTime = 0f;

    #endregion

    #region Methods

    private void Start()
    {
        shapesInScene = PlayerPrefs.GetInt("Difficulty", 1);
        if (shapesInScene > 9)
            shapesInScene = 9;
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
            if (PlayerPrefs.HasKey("Difficulty"))
            {
                int level = PlayerPrefs.GetInt("Difficulty");
                PlayerPrefs.SetInt("Difficulty", level + 1);
            }
            else
            {
                PlayerPrefs.SetInt("Difficulty", 2);
            }
            StartCoroutine(GameOver());
        }
    }

    IEnumerator GameOver()
    {
        yield return new WaitForSeconds(2.1f);
        mainCanvas.SetActive(false);
        AudioManager.Play(AudioClipNames.Applause);
        levelComplete.SetActive(true);
    }

    private IEnumerator LoadScene(int index)
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(index);
    }

    public void Restart()
    {
        StartCoroutine(LoadScene(2));
    }

    #endregion
}
