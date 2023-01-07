using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{

    private NavMeshAgent _agent;
    private GameObject [] _playerList;
    private float counter = 0.0f;

    public Vector3 destination;

    // Start is called before the first frame update
    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _playerList = GameObject.FindGameObjectsWithTag("Player");
    }



    // Update is called once per frame
    void Update()
    {
        counter += Time.deltaTime;
        if (counter > 0.1f) {
            counter = 0.0f;


            if (_playerList.Length == 0)
            {
                _playerList = GameObject.FindGameObjectsWithTag("Player");
            }

            SetEnemyDestination();
        }
        //InvokeRepeating is a huge drag
        //InvokeRepeating("SetEnemyDestination", 0, .5f);
    }

    void SetEnemyDestination()
    {
        float minDistance = float.MaxValue;
        Vector3 dest = transform.position;
        foreach (var p in _playerList) {
            float dist = Vector3.Distance(p.transform.position, transform.position);
            if (dist < minDistance) {
                minDistance = dist;
                dest = p.transform.position;
            }
        }
        _agent.SetDestination(dest);
        destination = dest;
    }

}
