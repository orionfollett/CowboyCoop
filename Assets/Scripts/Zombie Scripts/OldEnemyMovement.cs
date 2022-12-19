using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    public List<GameObject> playerList;

    private NavMeshAgent _agent;
    private Vector3 _enemyDestination;
    private bool _newDestFlag = false;
    private float _attackRange;

    public enum EnemyAnimationState { RUN = 0, ATTACK = 1 };
    private Animator _anim;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _anim = GetComponent<Animator>();
    }
    // Start is called before the first frame update
    void Start()
    {   
        InvokeRepeating("SetEnemyDestination", 1, .25f);
        _enemyDestination = new Vector3(0, 0, 0);
        _attackRange = GetComponent<EnemyAttack>().attackRange;
        _agent.stoppingDistance = _attackRange * .75f;
    }

    void SetEnemyDestination() {
        float minDistance = float.MaxValue;
        Vector3 target = new Vector3(0, 0, 0);
        foreach (var player in playerList) {
            //player.transform.position
            Vector3 pos = player.transform.position;
            float dist = Vector3.Distance(pos, transform.position);
            if (dist < minDistance) {
                minDistance = dist;
                target = pos;
            }
        }
        _enemyDestination = target;
        _newDestFlag = true;
    }

    void Update()
    {
        //transform.position = _agent.nextPosition;


        if (_newDestFlag && _agent.enabled) {
            _agent.SetDestination(_enemyDestination);
        }

        if (Vector3.Magnitude(_agent.nextPosition - _enemyDestination) < _attackRange)
        {
            //Debug.Log("ATTACK");
            _anim.SetInteger("State", (int)EnemyAnimationState.ATTACK);
            //need to point the enemy at player
        }
        else
        {
           // Debug.Log("RUN");
            _anim.SetInteger("State", (int)EnemyAnimationState.RUN);
            _anim.SetFloat("MovementSpeed", _agent.velocity.magnitude / _agent.speed);
        }
    }
}
