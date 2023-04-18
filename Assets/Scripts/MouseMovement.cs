using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseMovement : MonoBehaviour
{
    private float _rotX;

    private float _rotY;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse Y");
        float mouseY = Input.GetAxis("Mouse X");

        _rotX += mouseX;
        _rotY += mouseY;
        transform.rotation = Quaternion.Euler(_rotX, _rotY, 0);
    }
}
