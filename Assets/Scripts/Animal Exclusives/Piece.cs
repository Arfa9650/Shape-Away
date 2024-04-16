using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    SpriteRenderer sr;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(sr != null)
            sr.color = new Color(0.52f, 0.52f, 0.52f, 1f);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (sr != null)
            sr.color = new Color(0.3f, 0.3f, 0.3f, 1f);
    }
}
