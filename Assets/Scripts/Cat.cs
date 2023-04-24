using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class Cat : Player
{
    private BoxCollider _boxCollider;
    // Start is called before the first frame update
    void Start()
    {
        Speed = 6.0f;
        CurrentForm = "cat";
        JumpForce = new Vector3(0, 5, 0);
        Health = 75;
        CatScript = GetComponent<Cat>();
        PlayerScript = GetComponent<Player>();
        CharacterController = GetComponent<CharacterController>();
        _boxCollider = GetComponent<BoxCollider>();
        _boxCollider.enabled = true;
    }

    protected override void Movement()
    {
        var verticalInput = Input.GetAxis("Vertical");
        var movementVector = verticalInput * Speed * transform.forward;
        transform.Translate(movementVector * Time.deltaTime);
    }

    protected override void Morph()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            PlayerScript.enabled = true;
            _boxCollider.enabled = false;
            CatScript.enabled = false;
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            
            // bear script
        }
    }

    // Update is called once per frame
    void Update()
    {
        Morph();
        Movement();
    }
}
