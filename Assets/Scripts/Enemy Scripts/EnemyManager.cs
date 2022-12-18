using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public int maxEnemies = 5;
    public float spawnDelay = 3.0f;
    public GameObject enemyPrefab;

    private int enemyId = 0;
    private Dictionary<int, GameObject> enemies;
    private GameObject[] _spawnPoints;
    private GameObject[]  _playerList;
    private int spawnIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        _spawnPoints = GameObject.FindGameObjectsWithTag("EnemySpawn");
        _playerList = GameObject.FindGameObjectsWithTag("Player");
        enemies = new Dictionary<int, GameObject>();
        InvokeRepeating("SpawnEnemy", 1, spawnDelay);
    }

    void SpawnEnemy() {
        if (enemies.Count < maxEnemies)
        {
            Vector3 spawnPosition = _spawnPoints[spawnIndex].transform.position;
            spawnIndex++;
            GameObject enemy = Instantiate(enemyPrefab, spawnPosition, new Quaternion(0, 0, 0, 0));
            enemy.GetComponent<EnemyDeath>().manager = this;
            enemy.GetComponent<EnemyDeath>().id = enemyId;
            enemy.GetComponent<EnemyMovement>().playerList = new List<GameObject>(_playerList);
            enemies.Add(enemyId, enemy);
            enemyId++;

            if (spawnIndex >= _spawnPoints.Length) {
                spawnIndex = 0;
            }
        }
    }

    public void DeleteDeadEnemy(int id) {
        enemies.Remove(id);
    }
}
