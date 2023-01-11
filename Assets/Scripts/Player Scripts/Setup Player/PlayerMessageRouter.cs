using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMessageRouter : MonoBehaviour
{
    public SoundManager soundManager;
    public EventManagementSystem eventSystem;
    public GameObject playerCapsule;
    public HitmarkerParameters hitmarker;

    private PlayerStats _playerStats;
    private PlayerGun _playerGun;
    private int _playerId;
    private RespawnAndKillPlayer _respawnAndKillPlayer;
    private string _audioDirection;
    private void Start()
    {
        _respawnAndKillPlayer = GetComponent<RespawnAndKillPlayer>();
        _playerStats = playerCapsule.GetComponent<PlayerStats>();
        _playerGun = playerCapsule.GetComponent<PlayerGun>();
        _playerId = GetComponent<SetupPlayer>().playerId;
        _audioDirection = GetComponent<SetupPlayer>().audioDirection;
    }

    IEnumerator DelayPlaySound(string soundName, float timeInSeconds)
    {
        yield return new WaitForSeconds(timeInSeconds);
        soundManager.PlaySound(_audioDirection, soundName);
    }

    //Send and receive damage
    public void ReceiveDamage(int damage, int sender, int receiver) {
        _playerStats.ReceiveDamage(damage, sender);
    }

    public void SendDamage(int damage, int receiver) { 
        eventSystem.SendDamage(damage, _playerId, receiver);
    }

    //Send and receive killshot notification
    public void ReceiveKillshotNotification(int sender, int receiver)
    {
        StartCoroutine(DelayPlaySound("successfulKill", 0.25f));
        hitmarker.Kill();
    }

    public void SendKillshotNotification(int receiver)
    {
        eventSystem.SendKillshotNotification(_playerId, receiver);
    }


    //Receive new health
    public void ReceiveNewHealth(int health) {
        _playerStats.health = health;
    }

    public void ReceiveRespawn(int respawnTime) { 
        _respawnAndKillPlayer.Respawn(respawnTime);
    }
}
