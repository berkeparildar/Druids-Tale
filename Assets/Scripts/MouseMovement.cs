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
        // if (_rotY > 359)
        // {
            // _rotY = 360 - _rotY;
        // }
        // else if (_rotY < -359)
        // {
            // _rotY = 360 + _rotY;
        // }

        // if (Mathf.Abs(_rotY) < 180 && Mathf.Abs(transform.rotation.eulerAngles.x) > 1)
        // {
        //     _rotX += mouseY / 180;
        // }
        // else if (Mathf.Abs(_rotY) > 180 && Mathf.Abs(transform.rotation.eulerAngles.x) > 1)
        // {
        //     _rotX -= mouseY / 180; 
        // }
        _rotY += mouseY;
        // _animator.SetFloat(Animator.StringToHash("rotation"), mouseY);
        transform.rotation = Quaternion.Euler(0, _rotY, 0);
        // RaycastHit hit;
        // if (Physics.Raycast(origin: transform.position, direction: -Vector3.up, maxDistance: 1.0f, hitInfo: out hit))
        // {
        //     Debug.Log(hit.collider.name + "hit normal" + hit.normal);
        //     var rot = Quaternion.FromToRotation(Vector3.up, hit.normal);
        //     transform.GetChild(0).localRotation = Quaternion.Euler(rot.x, _rotY, 0);
        //     Debug.Log(rot.eulerAngles);
        // }
        
    }
    private void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
