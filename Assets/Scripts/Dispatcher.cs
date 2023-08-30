using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Dispatcher : MonoBehaviour
{
    public static Queue<Action> ExecuteOnMainThread = new Queue<Action>();

    public void Update()
    {
        while (ExecuteOnMainThread.Count > 0)
        {
            ExecuteOnMainThread.Dequeue().Invoke();
        }
    }

}