using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class RevivePlayer : MonoBehaviour
{

    public int reviveRange;
    public float reviveTimeLength;
    public Transform cameraTransform;
    public LayerMask hitableLayerMask;

    private StarterAssetsInputs _input;
    private float _reviveTimerCount;

    // Start is called before the first frame update
    void Start()
    {
        _input= GetComponent<StarterAssetsInputs>();
    }

    // Update is called once per frame
    void Update()
    {
        //if interact is being held down this will fire repeatedly, but will stop when released
        if (_input.interact)
        {
            AttemptRevive();
        }
    }

    public void AttemptRevive()
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hitInfo, reviveRange, hitableLayerMask))
        {
            GameObject _otherPlayer = hitInfo.transform.gameObject;
            if (_otherPlayer.tag == "Player" && _otherPlayer.GetComponent<BulletDamageable>().playerId != GetComponent<BulletDamageable>().playerId)
            {
                
                _reviveTimerCount += Time.deltaTime;
               
                if (_reviveTimerCount > reviveTimeLength)
                {
                    _reviveTimerCount = 0;
                    _otherPlayer.SendMessage("ReviveMe");
                }
            }
            else
            {
                _reviveTimerCount = 0;
            }
        }
        else
        {
            _reviveTimerCount = 0;
        }

    }
}
