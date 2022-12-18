using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;
using StarterAssets;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
using UnityEngine.Audio;

public class SetupPlayer : MonoBehaviour
{
    public string playerName;
    public int playerId = 1;
    public int playerCount = 2;
    public string audioDirection = "m"; // can be l, r or m for left, middle, or right

    [Header("Cameras")]
    public Camera mainCamera;
    public Camera gunCamera;
    public CinemachineVirtualCamera playerFollowCamera;

    [Header("Player Objects")]
    public GameObject gunAndArms;
    public GameObject characterModel;
    public GameObject pauseMenu;
    public GameObject playerCapsule;
    public GameObject hud;

    [Header("Settings")]
    public int fov = 60;
    public AudioMixer audioMixer;

    private int playerLayer;
    private int notLayer;
    private int gunLayer;
    private int uiLayer;

    void Start()
    {   
        playerName = "Player" + playerId.ToString();

        playerLayer = LayerMask.NameToLayer($"onlyP{playerId}");
        notLayer = LayerMask.NameToLayer($"notP{playerId}");
        gunLayer = LayerMask.NameToLayer($"gunP{playerId}");
        uiLayer = LayerMask.NameToLayer($"UI");
        SetupLayers();
        SetupCameraLayerMasks();
        LoadPreferences();

        GetComponent<RespawnAndKillPlayer>().Spawn();
        mainCamera.fieldOfView = fov;
        gunCamera.fieldOfView = fov;
        playerFollowCamera.m_Lens.FieldOfView = fov;
    }

    public void LoadPreferences() {
        FirstPersonController settings = playerCapsule.GetComponent<FirstPersonController>();

        //Load player prefs
        settings.RestingRotationSpeed = (float)PlayerPrefs.GetInt(playerName + "_sensitivity", 50) * 15 / 100;
        settings.AimingRotationSpeed = (float)PlayerPrefs.GetInt(playerName + "_aim_sensitivity", 50) / 100.0f * settings.RestingRotationSpeed;

        //audio ranges from -80 to 0, slider ranges from 1-100
        float volume = .8f*(PlayerPrefs.GetInt("master_volume", 50) - 80.0f);
        audioMixer.SetFloat("MasterVolume", volume);
        
    }

    void SetupLayers() {
        SetLayerAndAllChildrenLayer(gameObject.transform, playerLayer);//must come first
        SetLayerAndAllChildrenLayer(characterModel.transform, notLayer);
        SetLayerAndAllChildrenLayer(gunAndArms.transform, gunLayer);
        SetLayerAndAllChildrenLayer(hud.transform, gunLayer);
        SetLayerAndAllChildrenLayer(pauseMenu.transform, uiLayer);
    }

    void SetupCameraLayerMasks() {
        //main camera
        LayerMask mainMask = GetEverythingLayer();
        List<int> onlyLayersBesidesYours = GetOnlyLayersBesidesYours();
        List<int> gunLayers = GetGunLayers();
        for (int i = 0; i < onlyLayersBesidesYours.Count; i++)
        {
            mainMask = RemoveLayerToLayerMask(mainMask, onlyLayersBesidesYours[i]);
            mainMask = RemoveLayerToLayerMask(mainMask, gunLayers[i]);
        }
        mainMask = RemoveLayerToLayerMask(mainMask, notLayer);
        mainCamera.cullingMask = mainMask;

        //gun camera
        LayerMask gunMask = GetNothingLayer();
        gunMask = AddLayerToLayerMask(gunMask, gunLayer);
        gunCamera.cullingMask = gunMask;
    }

    void SetLayerAndAllChildrenLayer(Transform root, LayerMask layer) {
        var children = root.GetComponentsInChildren<Transform>(includeInactive: true);
        foreach (var child in children)
        {
            child.gameObject.layer = layer;
        }
    }

    List<int> GetOnlyLayersBesidesYours() {
        List<int> layers = new List<int>(4);

        for (int i = 0; i < 4; i++) {
            if (i != playerId) {
                layers.Add(LayerMask.NameToLayer($"onlyP{i}"));
            }
        }

        return layers;
    }

    List<int> GetGunLayers()
    {
        List<int> layers = new List<int>(4);

        for (int i = 0; i < 4; i++)
        {
            layers.Add(LayerMask.NameToLayer($"gunP{i}"));
        }

        return layers;
    }

    LayerMask AddLayerToLayerMask(LayerMask originalLayerMask, int layerToAdd) {
        originalLayerMask |= (1 << layerToAdd);
        return originalLayerMask;
    }

    LayerMask RemoveLayerToLayerMask(LayerMask originalLayerMask, int layerToRemove)
    {
        originalLayerMask &= ~(1 << layerToRemove);
        return originalLayerMask;
    }

    LayerMask GetEverythingLayer() {
        return ~0;
    }

    LayerMask GetNothingLayer() {
        return 0;
    }
}
