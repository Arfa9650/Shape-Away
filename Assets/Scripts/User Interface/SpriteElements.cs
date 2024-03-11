using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteElements : MonoBehaviour
{
    bool firstTime = true;
    float elapsedTime = 0;
    Image native;

    private void Start()
    {
        native = GetComponent<Image>();
        native.SetNativeSize();
    }

    private void Update()
    {

        if (firstTime)
        {
            elapsedTime += Time.deltaTime;
            native.SetNativeSize();
            //Debug.Log("running");
            if (elapsedTime >= 1f && firstTime)
            {
                firstTime = false;
            }
        }
        

        
    }
}
