using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseMovement : MonoBehaviour
{
    private float _rotX;

    private float _rotY;
    private Animator _animator;
    
    // Start is called before the first frame update
    void Start()
    {
        _animator = transform.GetChild(0).GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse Y");
        float mouseY = Input.GetAxis("Mouse X");

        _rotX += mouseX;
        _rotY += mouseY;
        _animator.SetFloat(Animator.StringToHash("rotation"), mouseY);
        Debug.Log(mouseY);
        transform.rotation = Quaternion.Euler(0, _rotY, 0);
    }
}
