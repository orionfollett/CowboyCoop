using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("Variables")]
    public int health = 100;
    public int downCounter = 0;
    public int maxDowns = 3;
    public int downTimeTillDeath = 10;
    public int respawnTime = 3;
    public float damageScreenSpeed = 10f;
    public bool isDown = false;

    [Header("Objects")]
    public CanvasGroup damageScreen;

    private PlayerMessageRouter _playerRouter;
    private FirstPersonController _controller;
    private int _startingHealth;
    private float _damageScreenStep;
    private float _targetAlpha = 0.0f;

    private IEnumerator _delayedDeathCoroutine;

    private void Start()
    {
        _startingHealth = health;
        _playerRouter = GetComponent<PlayerGun>().playerRoot.GetComponent<PlayerMessageRouter>();
        _damageScreenStep = 1.0f / (float)(_startingHealth);
        _controller = GetComponent<FirstPersonController>();
        _delayedDeathCoroutine = DelayedDeath(downTimeTillDeath);
    }

    private void Update()
    {
        _targetAlpha = (_startingHealth - health) * _damageScreenStep;
        damageScreen.alpha = Mathf.Lerp(damageScreen.alpha, _targetAlpha, Time.deltaTime * damageScreenSpeed);
    }

    //pick up a health potion
    public void HealthPickup(int healthRecieved)
    {
        health += healthRecieved;
        health = Mathf.Clamp(health, 0, _startingHealth);
    }

    //receive damage from another player
    public void ReceiveDamage(int damage, int enemyId) {
        health -= damage;
        if (health <= 0) {
            _playerRouter.SendKillshotNotification(enemyId);
            //_playerRouter.ReceiveRespawn(respawnTime);
            GoDown(downTimeTillDeath);

        }
    }

    //receive damage from an enemy
    public void ReceiveZombieDamage(int damage) {
        health -= damage;
        if (health <= 0 && !isDown)
        {
            //_playerRouter.ReceiveRespawn(respawnTime);
            GoDown(downTimeTillDeath);
        }
    }

    //"going down" means that the player cannot move, their screen turns black and white, and they cannot take any damage
    //normal functions are restored if a player revives them
    public void GoDown(int timeTillDeath) {
        downCounter += 1;
        if (downCounter > maxDowns)
        {
            Die();
        }
        else {
            isDown = true;
            _controller.isStuck = true;
            _delayedDeathCoroutine = DelayedDeath(downTimeTillDeath);
            StartCoroutine(_delayedDeathCoroutine); //delays the death to give time for a revive
        }
    }

    public void ReviveMe() {
        if (isDown) {
            Debug.Log("Revived!");
            isDown = false;
            _controller.isStuck = false;
            health = Mathf.Clamp(health + 1, 1, _startingHealth);
            StopCoroutine(_delayedDeathCoroutine);
        }
    }

    IEnumerator DelayedDeath(int delay)
    {
        yield return new WaitForSeconds(delay);
        Die();
    }

    public void Die() {
        //player die
        health = _startingHealth;
        _controller.isStuck = false;
        isDown = false;
        _playerRouter.ReceiveRespawn(respawnTime);
    }
}
