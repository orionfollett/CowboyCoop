using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("Variables")]
    public int health = 100;
    public int downs = 1;
    public int respawnTime = 3;
    public float damageScreenSpeed = 10f;

    [Header("Objects")]
    public CanvasGroup damageScreen;

    private PlayerMessageRouter _playerRouter;
    private int _startingHealth;
    private float _damageScreenStep;
    private float _targetAlpha = 0.0f;

    private void Start()
    {
        _startingHealth = health;
        _playerRouter = GetComponent<PlayerGun>().playerRoot.GetComponent<PlayerMessageRouter>();
        _damageScreenStep = 1.0f / (float)(_startingHealth);
    }

    private void Update()
    {
        _targetAlpha = (_startingHealth - health) * _damageScreenStep;
        damageScreen.alpha = Mathf.Lerp(damageScreen.alpha, _targetAlpha, Time.deltaTime * damageScreenSpeed);
    }

    public void ReceiveDamage(int damage, int enemyId) {
        health -= damage;
        if (health == 0) {
            _playerRouter.SendKillshotNotification(enemyId);
            health = _startingHealth;
            _playerRouter.ReceiveRespawn(respawnTime);
        }
    }

    public void ReceiveZombieDamage(int damage) {
        
        health -= damage;
        if (health == 0)
        {
            health = _startingHealth;
            _playerRouter.ReceiveRespawn(respawnTime);
        }
    }

    public void HealthPickup(int healthRecieved) {
        health += healthRecieved;
        health = Mathf.Clamp(health, 0, _startingHealth);
    }
}
