using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Events;

public class ToggleProxyEmitter : MonoBehaviour
{
    public UnityEvent Enable = new UnityEvent();
    public UnityEvent Disable = new UnityEvent();
    [SerializeField,ReadOnly]
    private bool active;

    public void Receive()
    {
        if (active)
        {
            Enable?.Invoke();
        }
        else
        {
            Disable?.Invoke();
        }

        active = !active;
    }

}
