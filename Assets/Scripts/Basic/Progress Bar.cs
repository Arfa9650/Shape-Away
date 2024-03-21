using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : IntEventInvoker
{
    [SerializeField]
    Image mask;

    int maximum;
    int current;

    public bool animOnStart = false;


    private void Start()
    {
        current = PlayerPrefs.GetInt("Difficulty", 1);
        maximum = PlayerPrefs.GetInt("Maximum", 10);
        
        if (EventManager.initialized)
        {
            EventManager.AddListener(EventNames.GameOver, GetCurrentFill);
        }

        if(animOnStart)
        {
            GetCurrentFill(current);
        }

        else if(!animOnStart && AudioManager.FirstTime)
        {
            AudioManager.FirstTime = false;
            GetCurrentFill(current);
        }

        else
        {
            GetCurrentFillWithoutAnimation(current);
        }
    }

    void GetCurrentFill(int num)
    {
        StartCoroutine(AnimateFill(num));
    }

    void GetCurrentFillWithoutAnimation(int num)
    {
        float fillAmount = (float)num / (float)maximum;
        mask.fillAmount = fillAmount;
    }

    IEnumerator AnimateFill(int targetValue)
    {
        float currentValue = mask.fillAmount;
        float targetFillAmount = (float)targetValue / (float)maximum;
        float duration = 1f; // Adjust as needed

        float timer = 0f;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            mask.fillAmount = Mathf.Lerp(currentValue, targetFillAmount, timer / duration);
            yield return null;
        }

        mask.fillAmount = targetFillAmount; // Ensure it reaches the exact target value
    }
}
