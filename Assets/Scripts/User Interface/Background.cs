using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : IntEventInvoker
{
    Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
        EventManager.AddListener(EventNames.FitShape, Success);
        EventManager.AddListener(EventNames.FailToFit, Failed);
    }

    public void Success(int num)
    {
        anim.SetTrigger("Success");
    }

    public void Failed(int num)
    {
        anim.SetTrigger("Fail");
    }

    public void ResetToDefaultState()
    {
        anim.ResetTrigger("Fail");
        anim.ResetTrigger("Success");
    }
}
