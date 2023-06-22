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
        LockCursor();
    }

    // Update is called once per frame
    void Update()
    {
        float mouseY = Input.GetAxis("Mouse X");
        _rotY += mouseY;
        transform.rotation = Quaternion.Euler(0, _rotY, 0);
    }
    
    private void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
