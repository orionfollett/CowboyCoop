using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    public int health = 3;
    public void Damage(int damage) {
        health -= damage;
        if (health <= 0) {
            SendMessage("IAmDead");
        }
    }


}
