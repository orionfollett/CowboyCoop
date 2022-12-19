using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public BoxCollider attackCollider;
    //public Animator animator;

    public int damage = 1;
    public float attackWindow = 0.2f;
    public float attackRange = 2.0f;

    private bool isAttacking = false;

    private void Start()
    {
        attackCollider.center = new Vector3(0, 0, -0.25f + attackRange / 2);
        attackCollider.size = new Vector3(1, 1, attackRange);
        attackCollider.enabled = false;
    }

    //called by animation event
    public void Attack() {
        Debug.Log("ATTACK!");
        isAttacking = true;
        attackCollider.enabled = true;
        StartCoroutine(CancelAttack(attackWindow));
    }

    IEnumerator CancelAttack(float time) {
        yield return new WaitForSeconds(time);
        isAttacking = false;
        attackCollider.enabled = false;
    }

    private void OnTriggerStay(Collider other)
    {
        if (isAttacking) {
            isAttacking = false;
            other.SendMessage("ReceiveZombieDamage", damage);
            attackCollider.enabled = false;
        }   

    }
}
