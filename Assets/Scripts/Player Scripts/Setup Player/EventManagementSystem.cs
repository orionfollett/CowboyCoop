using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManagementSystem : MonoBehaviour
{

    private PlayerManager _playerManager; //playerID is an index into the players list in playermanager
    private List<PlayerMessageRouter> _playerRouters = new List<PlayerMessageRouter>();
   
    // Start is called before the first frame update
    void Start()
    {
        _playerManager = GetComponent<PlayerManager>();
        foreach (var p in _playerManager.players) {
            _playerRouters.Add(p.GetComponent<PlayerMessageRouter>());
        }
    }

    public void SendDamage(int damage, int sender, int receiver) {
        _playerRouters[receiver].ReceiveDamage(damage, sender, receiver);        
    }

    public void SendKillshotNotification(int sender, int receiver) {
        _playerRouters[receiver].ReceiveKillshotNotification(sender, receiver);
    }
}
