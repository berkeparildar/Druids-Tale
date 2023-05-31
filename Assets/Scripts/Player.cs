using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    protected Animator Animator;
    public LayerMask _groundLayer;
    protected GameObject CatModel;
    protected Cat CatScript;
    protected GameObject HumanModel;
    protected Player HumanScript;
    protected Rigidbody Rigidbody;
    private float _verticalInput;
    private float _horizontalInput;
    protected bool IsJumping;
    protected bool IsCasting;
    protected string CurrentForm;
    protected int Health;
    protected float JumpForce;
    protected bool IsOnGround;
    protected float Speed;
    protected bool Jump;
    private static readonly int Jumping = Animator.StringToHash("isJumping");
    private static readonly int VerticalMovement = Animator.StringToHash("verticalMovement");
    private static readonly int HorizontalMovement = Animator.StringToHash("horizontalMovement");
    protected static readonly int CastSpellOne = Animator.StringToHash("castSpellOne");
    
    // Start is called before the first frame update
    private void Awake()
    {
        // _groundLayer = LayerMask.NameToLayer("Ground");
        IsOnGround = true;
        IsCasting = false;
        HumanScript = transform.GetComponent<Human>();
        CatScript = transform.GetComponent<Cat>();
        HumanModel = transform.GetChild(0).gameObject;
        CatModel = transform.GetChild(1).gameObject;
        Rigidbody = GetComponent<Rigidbody>();
        Debug.Log("Hello from the player script!");
    }

    protected virtual void Movement()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 0.2f, _groundLayer))
        {
            IsOnGround = true;
            IsJumping = false;
            Animator.SetBool(Jumping, IsJumping);
            if (hit.transform.gameObject.CompareTag("Platform"))
            {
                transform.SetParent(hit.transform);
            }
        }
        else
        {
            IsOnGround = false;
            transform.SetParent(null);
        }

        var humanTransform = transform;
        var movement = (CurrentForm == "cat" ? (_horizontalInput * Speed / 5) : (_horizontalInput * Speed)) * humanTransform.right +
                       humanTransform.forward * (((CurrentForm == "cat" ? (_verticalInput < 0 ? _verticalInput / 3 : _verticalInput ) : _verticalInput)) * Speed);
        movement.y = Rigidbody.velocity.y;
        if (Jump)
        {
            if (IsOnGround)
            {
                Rigidbody.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);
                IsJumping = true;
            }
        }
        else
        {
            if (!IsCasting)
            {
                Rigidbody.velocity = movement;
            }
        }
    }
    
    protected void GetInput()
    {
        Animator.SetFloat(VerticalMovement, _verticalInput);
        Animator.SetFloat(HorizontalMovement, _horizontalInput);
        Animator.SetBool(Jumping, IsJumping);
        _verticalInput = Input.GetAxis("Vertical");
        _horizontalInput = Input.GetAxis("Horizontal");
        if (Input.GetKey(KeyCode.Space))
        {
            Jump = true;
        }
        else
        {
            Jump = false;
        }
    }

    protected virtual void Morph()
    {
        
    }

    protected virtual void SpecialAbilityOne()
    {
        //Special Attack;
    }

    protected virtual void SpecialAbilityTwo()
    {
        //
    }

    protected void AbilityCheck()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SpecialAbilityOne();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SpecialAbilityTwo();
        }
    }
    public virtual void TakeDamage(int damage)
    {
        Health -= damage;
        // if (Health <= 0)
            //die
            // Destroy(this);
    }
}
