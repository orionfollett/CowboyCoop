using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDamageable : MonoBehaviour
{
    public SetupPlayer setupPlayer;
    public int playerId;
    [Tooltip("Could be used to apply different damage to different body parts")]
    public int damageMultiplier = 1;
    // Start is called before the first frame update
    void Start()
    {
        playerId = setupPlayer.playerId;
    } 
}
