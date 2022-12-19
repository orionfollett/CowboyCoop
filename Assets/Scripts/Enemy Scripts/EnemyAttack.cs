using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public int damage = 1;
    
    private Damageable _enemyHealth;
    private GameObject _managers;

    private void Start()
    {
        _enemyHealth = GetComponent<Damageable>();
        _managers = GameObject.FindGameObjectWithTag("GameController");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.SendMessage("ReceiveZombieDamage", damage);
        }

        _managers.SendMessage("PlaySoundMessage", "enemyAttack");

        //destroy this enemy properly
        _enemyHealth.health = 0;
        _enemyHealth.Damage(1);
    }
}
