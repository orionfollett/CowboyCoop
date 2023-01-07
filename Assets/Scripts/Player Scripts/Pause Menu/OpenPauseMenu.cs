using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class OpenPauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    private StarterAssetsInputs _input;
    public GameObject gunCamera;

    // Start is called before the first frame update
    void Start()
    {
        _input = GetComponent<StarterAssetsInputs>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_input.pause)
        {
            Open();
        }
        else {
            Close();
        }
    }

    private void Close() {
        pauseMenu.SetActive(false);
        gunCamera.SetActive(true);
    }

    private void Open() {
        pauseMenu.SetActive(true);
        gunCamera.SetActive(false);
    }

    public void CloseMenu() {
        Open();
        _input.pause = false;
    }
}
