using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class IntEventInvoker : MonoBehaviour
{
    #region Fields
    
    protected Dictionary<EventNames, UnityEvent<int>> unityEvents = new Dictionary<EventNames, UnityEvent<int>>();

    //protected bool strawberryPerm = true;

    #endregion

    #region Methods

    public void AddListener(EventNames eventName, UnityAction<int> listener)
    {
        if(unityEvents.ContainsKey(eventName))
        {
            unityEvents[eventName].AddListener(listener);
        }
    }

    #endregion
}
