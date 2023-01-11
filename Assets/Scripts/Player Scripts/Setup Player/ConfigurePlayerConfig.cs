using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ConfigurePlayerConfig : MonoBehaviour
{
    public int playerCount = 0;
    private void Awake()
    {
        DontDestroyOnLoad(this);
    }
}
