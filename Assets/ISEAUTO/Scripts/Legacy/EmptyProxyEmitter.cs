using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EmptyProxyEmitter : MonoBehaviour
{
    public UnityEvent Proxy = new UnityEvent();


    public void Receive()
    {
        Proxy?.Invoke();
    }
}
