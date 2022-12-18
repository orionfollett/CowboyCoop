using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{

    private Animator _animator;
    private StarterAssetsInputs _input;

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        _input = GetComponentInParent<StarterAssetsInputs>();
    }

    // Update is called once per frame
    void Update()
    {
        _animator.SetFloat("Speed", _input.move.SqrMagnitude());
    }
}
