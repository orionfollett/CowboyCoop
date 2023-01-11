using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnAndKillPlayer : MonoBehaviour
{
    public GameObject playerCapsule;
    public CanvasGroup deathCanvas;
    
    private PlayerMessageRouter _playerRouter;
    private List<Transform> _respawnPoints;
    private Transform respawnOrigin;
    private void Start()
    {
        _playerRouter = GetComponent<PlayerMessageRouter>();
        deathCanvas.alpha = 0.0f;
        respawnOrigin = GameObject.FindGameObjectWithTag("Respawn").transform;
        _respawnPoints = new List<Transform>(GameObject.FindGameObjectWithTag("Respawn").GetComponentsInChildren<Transform>());
    }

    public void Spawn() {
        //playerCapsule.SetActive(false);
        respawnOrigin = GameObject.FindGameObjectWithTag("Respawn").transform;
        _respawnPoints = new List<Transform>(GameObject.FindGameObjectWithTag("Respawn").GetComponentsInChildren<Transform>());
        playerCapsule.transform.position = GetRandomSpawnPoint() + new Vector3(GetComponent<SetupPlayer>().playerId, 0, 0);
        playerCapsule.SetActive(true);
    }

    public void Respawn(int respawnTime)
    {
        playerCapsule.transform.position = GetRandomSpawnPoint();
        playerCapsule.SetActive(false);
        StartCoroutine(ShowDeathCanvas(respawnTime * .2f, respawnTime * .7f, respawnTime * .1f));
        StartCoroutine(DelayRespawn(respawnTime));
    }

    public Vector3 GetRandomSpawnPoint() 
    {
        int randomIndex = Random.Range(0, _respawnPoints.Count);
        return _respawnPoints[randomIndex].transform.position;
    }

    IEnumerator DelayRespawn(int respawnTime)
    {
        yield return new WaitForSeconds(respawnTime);
        playerCapsule.SetActive(true);
    }

    IEnumerator ShowDeathCanvas(float _timeToFadeIn, float _timeShowing, float _timeToFadeOut)
    {
        for (float alpha = 0f; alpha <= 1.0f; alpha += 0.1f)
        {
            deathCanvas.alpha = alpha;
            yield return new WaitForSeconds(_timeToFadeIn / 10.0f);
        }

        yield return new WaitForSeconds(_timeShowing);

        for (float alpha = 1f; alpha >= 0f; alpha -= 0.1f)
        {
            deathCanvas.alpha = alpha;
            yield return new WaitForSeconds(_timeToFadeOut / 10.0f);
        }

        deathCanvas.alpha = 0f;
    }
}
