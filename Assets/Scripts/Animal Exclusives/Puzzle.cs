using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle : MonoBehaviour
{
    SpriteRenderer sr;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        if (transform.parent == null)
        {
            sr.color = new Color(105f, 105f, 105f, 0.35f);
            sr.sortingOrder = -1;
        }
        else
        {
            sr.color = new Color(1f, 1f, 1f, 1f);
            sr.sortingOrder = 0;
        }
    }
}
