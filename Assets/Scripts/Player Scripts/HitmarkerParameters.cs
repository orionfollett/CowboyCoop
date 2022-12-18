using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HitmarkerParameters : MonoBehaviour
{
    public float timeToFadeIn = 1.0f;
    public float timeToFadeOut = 1.0f;
    public float timeShowing = 1.0f;
    public Color killColor = Color.red;
    public Color normalColor = Color.white;
    private Image[] _hitmarkers;
    private void Start()
    {
        _hitmarkers = GetComponentsInChildren<Image>();   
    }

    public void Kill()
    {
        //if (playerName == _playerName) {
            foreach (var i in _hitmarkers)
            {
                i.color = killColor;
            }
        //}

        StartCoroutine(Wait());
    }

    IEnumerator Wait() {
        yield return new WaitForSeconds(1.0f);

        foreach (var i in _hitmarkers)
        {
            i.color = normalColor;
        }
    }
}
