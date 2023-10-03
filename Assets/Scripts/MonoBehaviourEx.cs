using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoBehaviourEx<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Instance { get; private set; }
    protected bool hasDestroyed;
    protected virtual void Awake()
    {
        if (Instance != null)
        {
            DestroyImmediate(this.gameObject);
            hasDestroyed = true;
            return;
        }

        Instance = this as T;
    }
}
