using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

public class PlayerManager : MonoBehaviour
{
    public GameObject playerPrefab;
    public int playerCount = 0;

    readonly public List<GameObject> players = new List<GameObject>();
   
    void Awake()
    {
        GameObject playerConfigs = GameObject.FindGameObjectWithTag("PlayerConfigs");
        playerCount = playerConfigs.GetComponent<ConfigurePlayerConfig>().playerCount;
        
        for (int i = 0; i < playerCount; i++) {
            GameObject player = Instantiate(playerPrefab, new Vector3(i, 0, 0), Quaternion.identity);
            player.GetComponent<SetupPlayer>().playerId = i;
            player.GetComponent<SetupPlayer>().playerCount = playerCount;
            player.GetComponent<PlayerMessageRouter>().eventSystem = GetComponent<EventManagementSystem>();
            player.GetComponent<PlayerMessageRouter>().soundManager = GetComponent<SoundManager>();
            players.Add(player);
        }

        SetupViewPorts();
    }


    void SetPlayerCameraViewPort(GameObject player, Rect viewport) {
        Camera[] cameras = player.GetComponentsInChildren<Camera>();
        for (int i = 0; i < cameras.Length; i++) {
            cameras[i].rect = viewport;
        }
    }
    void SetupViewPorts()
    {
        if (playerCount == 1)
        {
            SetPlayerCameraViewPort(players[0], new Rect(0,0,1,1));

            players[0].GetComponent<SetupPlayer>().audioDirection = "m";
        }
        else if (playerCount == 2)
        {
            SetPlayerCameraViewPort(players[0], new Rect(0, 0.5f, 1, 0.5f)); //top / audio: left
            SetPlayerCameraViewPort(players[1], new Rect(0, 0, 1, 0.5f)); //bottom / audio: right

            players[0].GetComponent<SetupPlayer>().audioDirection = "m";
            players[1].GetComponent<SetupPlayer>().audioDirection = "m";
        }
        else if (playerCount == 3)
        {
            SetPlayerCameraViewPort(players[0], new Rect(0, 0.5f, 0.5f, 0.5f)); //top left / audio: left
            SetPlayerCameraViewPort(players[1], new Rect(0.5f, 0.5f, 0.5f, 0.5f)); //top right / audio: right
            SetPlayerCameraViewPort(players[2], new Rect(0, 0, 1, 0.5f)); //bottom / audio: (middle)

            players[0].GetComponent<SetupPlayer>().audioDirection = "l";
            players[1].GetComponent<SetupPlayer>().audioDirection = "r";
            players[2].GetComponent<SetupPlayer>().audioDirection = "m";
        }
        else if (playerCount == 4)
        {
            SetPlayerCameraViewPort(players[0], new Rect(0, 0.5f, 0.5f, 0.5f)); //top left / audio: left
            SetPlayerCameraViewPort(players[1], new Rect(0.5f, 0.5f, 0.5f, 0.5f)); //top right / audio: right
            SetPlayerCameraViewPort(players[2], new Rect(0, 0, 0.5f, 0.5f)); // bottom left / audio: left
            SetPlayerCameraViewPort(players[3], new Rect(0.5f, 0, 0.5f, 0.5f));// bottom right / audio: right

            players[0].GetComponent<SetupPlayer>().audioDirection = "l";
            players[1].GetComponent<SetupPlayer>().audioDirection = "r";
            players[2].GetComponent<SetupPlayer>().audioDirection = "l";
            players[3].GetComponent<SetupPlayer>().audioDirection = "r";
        }
        else
        {
            Debug.LogError("Only supports 1-4 players!");
        }
    }
}
