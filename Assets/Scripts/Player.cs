using System;
using System.Collections;
using Cinemachine;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.Serialization;
using UnityEngine.UI;

public enum Form
{
    Human,
    Cat,
    Bear
}

public class Player : MonoBehaviour
{
    protected Animator Animator;
    public LayerMask groundLayer;
    private Rigidbody _rigidbody;
    
    protected GameObject CatModel;
    protected Cat CatScript;
    protected GameObject HumanModel;
    protected Player HumanScript;
    protected GameObject BearModel;
    protected Bear BearScript;
    
    private float _verticalInput;
    private float _horizontalInput;
    protected bool IsJumping;
    protected bool IsCasting;
    protected float JumpForce;
    private bool _isOnGround = true;
    private float _maxHealth;
    protected float Speed;
    private bool _jump;
    
    protected GameInterface GameInterface;
    private bool _abilityOneReady;
    private bool _abilityTwoReady;
    protected Color ResourceColor;
    protected static Image ResourceImage;
    
    public static float Health; // maybe it does not make sense to have different health for each form.. idk
    private static readonly int Jumping = Animator.StringToHash("isJumping");
    private static readonly int VerticalMovement = Animator.StringToHash("verticalMovement");
    private static readonly int Die = Animator.StringToHash("die");
    private static readonly int HorizontalMovement = Animator.StringToHash("horizontalMovement");
    protected static readonly int CastSpellOne = Animator.StringToHash("castSpellOne");
    protected static readonly int CastSpellTwo = Animator.StringToHash("castSpellTwo");
    public static bool IsAlive;
    public static Form CurrentForm;
    public static bool bossLevel;

    // Start is called before the first frame update
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        HumanScript = transform.GetComponent<Human>();
        CatScript = transform.GetComponent<Cat>();
        BearScript = transform.GetComponent<Bear>();
        HumanModel = transform.GetChild(0).gameObject;
        CatModel = transform.GetChild(1).gameObject;
        BearModel = transform.GetChild(2).gameObject;
        GameInterface = GetComponent<GameInterface>();
        ResourceImage = GameObject.Find("Resource").GetComponent<Image>();
        _maxHealth = 100;
        Health = _maxHealth;
        _isOnGround = true;
        IsCasting = false;
        _abilityTwoReady = true;
        _abilityOneReady = true;
        IsAlive = true;
    }

    protected void Movement()
    {
        GroundCheck();
        var humanTransform = transform;
        var movement = humanTransform.forward * (_verticalInput * Speed);
        movement.y = _rigidbody.velocity.y;
        if (_jump)
        {
            if (_isOnGround)
            {
                _rigidbody.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);
                IsJumping = true;
            }
        }
        else
        {
            if (!IsCasting)
            {
                _rigidbody.velocity = movement;
            }
        }
    }

    private void GroundCheck()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out var hit,
                0.2f, groundLayer))
        {
            _isOnGround = true;
            IsJumping = false;
            Animator.SetBool(Jumping, IsJumping);
            if (hit.transform.gameObject.CompareTag("MovingLeft") || hit
                    .transform.gameObject.CompareTag("MovingRight"))
            {
                transform.SetParent(hit.transform);
            }
            else if (hit.transform.gameObject.CompareTag("Falling"))
            {
                var enumerator = hit.transform.GetComponent<SpecialPlatforms>()
                    .Fall();
                StartCoroutine(enumerator);
            }
        }
        else
        {
            _isOnGround = false;
            transform.SetParent(null);
        }
    }

    protected void GetInput()
    {
        Animator.SetFloat(VerticalMovement, _verticalInput);
        Animator.SetBool(Jumping, IsJumping);
        _verticalInput = Input.GetAxis("Vertical");
        if (Input.GetKey(KeyCode.Space))
        {
            _jump = true;
        }
        else
        {
            _jump = false;
        }

        if (Input.GetKeyDown(KeyCode.Alpha1) && _abilityOneReady)
        {
            CastSpecialAbilityOne();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) && _abilityTwoReady)
        {
            CastSpecialAbilityTwo();
        }
    }

    protected virtual void Morph()
    {
    }

    protected virtual void CastSpecialAbilityOne()
    {
        //Special Attack;
    }

    protected virtual void CastSpecialAbilityTwo()
    {
        //
    }

    protected void AbilityCheck()
    {
    }

    public virtual void TakeDamage(int damage)
    {
        if (Health > 0)
        {
            Health -= damage;
        }

        if (Health <= 0)
        {
            if (IsAlive)
            {
                //TODO: need death logic
                Animator.SetTrigger(Die);
                IsAlive = false;
            }
        }
    }

    protected void AbilityCooldown(int abilityNo, float cooldown)
    {
        switch (abilityNo)
        {
            case 1:
                GameInterface.ShowCooldownInActionBar(1, cooldown);
                StartCoroutine(AbilityCooldownCoroutine(abilityNo, cooldown));
                break;
            case 2:
                GameInterface.ShowCooldownInActionBar(2, cooldown);
                StartCoroutine(AbilityCooldownCoroutine(abilityNo, cooldown));
                break;
        }
    }

    IEnumerator AbilityCooldownCoroutine(int abilityNo, float cooldown)
    {
        switch (abilityNo)
        {
            case 1:
                _abilityOneReady = false;
                yield return new WaitForSeconds(cooldown);
                _abilityOneReady = true;
                break;
            case 2:
                _abilityTwoReady = false;
                yield return new WaitForSeconds(cooldown);
                _abilityTwoReady = true;
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Equals("Fall Limit"))
        {
            GameObject.Find("Virtual Follow Cam").GetComponent<CinemachineVirtualCamera>().Follow = null;
            IsAlive = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name.Equals("Cube"))
        {
            other.GetComponent<BoxCollider>().isTrigger = false;
            other.GetComponent<MeshRenderer>().enabled = true;
        }
    }
}