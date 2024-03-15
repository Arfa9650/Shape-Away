using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    Animator anim;

    private bool gameOver = false;

    private void Start()
    {
        anim = GetComponent<Animator>();
        EventManager.AddListener(EventNames.FitShape, Success);
        EventManager.AddListener(EventNames.FailToFit, Failed);
        EventManager.AddListener(EventNames.GameOver, DontTransition);
    }

    public void DontTransition(int zero)
    {
        gameOver = true;
        anim.SetTrigger("Happy");
    }

    public void Success(int num)
    {
        int rand = Random.Range(0, 2);
        //anim.SetTrigger((rand == 0) ? "Success" : "Happy");
        anim.SetTrigger("Success");
    }

    public void Failed(int num)
    {
        int rand = Random.Range(0, 2);
        //anim.SetTrigger((rand == 0) ? "Fail" : "Angry");
        anim.SetTrigger("Fail");
    }

    public void ResetToDefaultState()
    {
        if (!gameOver)
        {
            anim.ResetTrigger("Fail");
            anim.ResetTrigger("Success");
        }
    }
}
