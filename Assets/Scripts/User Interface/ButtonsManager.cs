using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonsManager : MonoBehaviour
{
    private void Start()
    {
        Application.targetFrameRate = 61;
    }

    public void Play()
    {
        SceneManager.LoadScene(1);
    }

    public void Restart()
    {
        if (PlayerPrefs.HasKey("Difficulty"))
        {
            int level = PlayerPrefs.GetInt("Difficulty");
            PlayerPrefs.SetInt("Difficulty", level + 1);
        }
        else
        {
            PlayerPrefs.SetInt("Difficulty", 1);
        }
        SceneManager.LoadScene(2);
    }

    public void ResetDifficulty()
    {
        PlayerPrefs.SetInt("Difficulty", 1);
        SceneManager.LoadScene(0);
    }
}
