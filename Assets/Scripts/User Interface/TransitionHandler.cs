using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TransitionHandler : MonoBehaviour
{
    [SerializeField]
    GameObject hand;

    [SerializeField]
    GameObject transition;

    [SerializeField]
    TextMeshProUGUI levelText;

    [SerializeField]
    TextMeshProUGUI lableText;

    private void Start()
    {
        int difficulty = PlayerPrefs.GetInt("Difficulty", 1);
        levelText.text = "Level " + difficulty.ToString();
        lableText.text = difficulty < 10 ? "Training" : "Do it Yourself";
    }

    public void OpenGame()
    {
        int difficulty = PlayerPrefs.GetInt("Difficulty", 0);
        if(difficulty < 10)
        {
            hand.SetActive(true);
        }

        transition.SetActive(false);
    }
}
