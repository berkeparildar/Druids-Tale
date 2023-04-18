using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private Animator _animator;
    private float _jumpForce = 4.0f;
    private float _speed = 3.0f;
    public bool _isOnGround = true;
    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = transform.GetComponent<Rigidbody>();
        _animator = transform.GetChild(0).GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    private void Movement()
    {
        var verticalInput = Input.GetAxis("Vertical");
        _animator.SetFloat(Animator.StringToHash("movement"), verticalInput);
        _rigidbody.velocity = new Vector3(0, _rigidbody.velocity.y, verticalInput * _speed);

        if (Input.GetKeyDown(KeyCode.Space) && _isOnGround)
        {
            _rigidbody.velocity = new Vector3(0, _jumpForce, _rigidbody.velocity.z);
            _animator.SetTrigger(Animator.StringToHash("jump"));
            //_isOnGround = false;
        }
    }
    
}
