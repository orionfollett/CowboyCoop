using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

public class PauseMenuManager : MonoBehaviour
{
    public OpenPauseMenu pauseMenu;
    public SetupPlayer setupPlayer;
    public MultiplayerEventSystem eventSystem;
    public AudioMixer mainAudioMixer;

    [Header("Panels")]
    public GameObject mainPanel;
    public GameObject optionsPanel;

    [Header("Input fields")]
    public Slider sensitivitySlider;
    public Slider aimSensitivitySlider;
    public Slider soundSlider;

    [Header("Dynamic Text Fields")]
    public TMP_Text sensitivityText;
    public TMP_Text aimSensitivityText;
    public TMP_Text soundText;

    [Header("First Button Selections")]
    public GameObject pauseFirstButton;
    public GameObject optionsFirstButton;

    // Start is called before the first frame update
    void OnEnable()
    {
        mainPanel.SetActive(true);
        optionsPanel.SetActive(false);
        eventSystem.SetSelectedGameObject(pauseFirstButton);
    }

    public void OnOptionsButtonPressed() {
        mainPanel.SetActive(false);
        optionsPanel.SetActive(true);

        //load in the playerpref values 
        //real sensitivity -> .01 - 1 is slower than average, 1 to 3 is faster
        //
        //slider sens -> 1 is 50 10 is 3
        //.1 to 5

        float sens = PlayerPrefs.GetInt(setupPlayer.playerName + "_sensitivity", 50);
        int numSens = (int)sens;
        sensitivityText.SetText(numSens.ToString());
        sensitivitySlider.value = numSens;

        float adsSens = PlayerPrefs.GetInt(setupPlayer.playerName + "_aim_sensitivity", 50);
        int numAdsSens = (int)adsSens;
        aimSensitivityText.SetText(numAdsSens.ToString());
        aimSensitivitySlider.value = numAdsSens;

        float volumeValue = PlayerPrefs.GetInt("master_volume", 50);
        int volumeInt = (int)volumeValue;
        soundText.SetText(volumeInt.ToString());
        soundSlider.value = volumeInt;

        eventSystem.SetSelectedGameObject(optionsFirstButton);
    }

    public void OnCancelButtonPressed()
    {
        mainPanel.SetActive(true);
        optionsPanel.SetActive(false);
        eventSystem.SetSelectedGameObject(pauseFirstButton);
    }

    public void OnApplyButtonPressed()
    {
        //submit the new value
        PlayerPrefs.SetInt(setupPlayer.playerName + "_sensitivity", int.Parse(sensitivityText.text));
        PlayerPrefs.SetInt(setupPlayer.playerName + "_aim_sensitivity", int.Parse(aimSensitivityText.text));
        PlayerPrefs.SetInt("master_volume", int.Parse(soundText.text));

        setupPlayer.LoadPreferences();

        mainPanel.SetActive(true);
        optionsPanel.SetActive(false);
        eventSystem.SetSelectedGameObject(pauseFirstButton);
    }

    public void OnContinueButtonPressed() {
        pauseMenu.CloseMenu();
    }
    public void OnQuitMainMenuButtonPressed() { }
    public void OnQuitDesktopButtonPressed() { }

    public void UpdateSliderText() {
        sensitivityText.SetText(sensitivitySlider.value.ToString());
        aimSensitivityText.SetText(aimSensitivitySlider.value.ToString());
        soundText.SetText(soundSlider.value.ToString());
    }
}
