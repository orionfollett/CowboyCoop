using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicCrosshair : MonoBehaviour
{

    public int restingSize;
    public int maxSize;
    public float speed;
    private float currSize;
    

    [Tooltip("Give the object that has the player input manager")]
    public GameObject player;

    private StarterAssetsInputs _input;
    private RectTransform _rectTransform;

    // Start is called before the first frame update
    void Start()
    {
        _input = player.GetComponent<StarterAssetsInputs>();
        _rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_input.move.sqrMagnitude > 0)
        {
            currSize = Mathf.Lerp(currSize, maxSize, speed * Time.deltaTime);
        }
        else {
            currSize = Mathf.Lerp(currSize, restingSize, speed * Time.deltaTime);
        }
        _rectTransform.sizeDelta = new Vector2(currSize, currSize);
        
    }
}
