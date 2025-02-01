using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentLevelInfo : MonoBehaviour
{
    public int levelIndex;
    private void Awake()
    {
        var objs = FindObjectsOfType<CurrentLevelInfo>();

        if (objs.Length > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
