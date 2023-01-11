using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using TMPro;
using UnityEngine.SceneManagement;

public class JoinPlayers : MonoBehaviour
{
    public UnityEngine.UI.Button startButton;
    public TMP_Text numberOfPlayers;
    public int _numPlayers = 0;
    public GameObject playerConfig;

    void Start() {
        numberOfPlayers.SetText("Number Of Players: " + _numPlayers.ToString());
        playerConfig = GameObject.FindGameObjectWithTag("PlayerConfigs");
    }

    void OnPlayerJoined() {
        startButton.gameObject.SetActive(true);
        _numPlayers++;
        numberOfPlayers.SetText("Number Of Players: " + _numPlayers.ToString());
    }

    void OnPlayerLeft() { 
        _numPlayers--;
        numberOfPlayers.SetText("Number Of Players: " + _numPlayers.ToString());
    }
    void OnStart() {
        playerConfig.GetComponent<ConfigurePlayerConfig>().playerCount = _numPlayers;
        SceneManager.LoadScene(1);
    }
}
