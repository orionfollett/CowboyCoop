using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeath : MonoBehaviour
{
    public EnemyManager manager;
    public int id;

    void IAmDead() {
        Destroy(gameObject);
        manager.DeleteDeadEnemy(id);
    }
}
