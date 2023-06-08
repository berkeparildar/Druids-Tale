using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    protected Animator Animator;
    public LayerMask groundLayer;
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
    private GameInterface _gameInterface;
    protected float JumpForce;
    private bool _isOnGround = true;
    protected float Speed;
    private bool _jump;
    private bool _abilityOneReady;
    private bool _abilityTwoReady;

    protected Color ResourceColor;
    
    protected static Image ResourceImage;
    public static float Health; // maybe it does not make sense to have different health for each form.. idk
    private float _maxHealth;
    private static readonly int Jumping = Animator.StringToHash("isJumping");
    private static readonly int VerticalMovement = Animator.StringToHash("verticalMovement");
    private static readonly int Die = Animator.StringToHash("die");
    private static readonly int HorizontalMovement = Animator.StringToHash("horizontalMovement");
    protected static readonly int CastSpellOne = Animator.StringToHash("castSpellOne");
    protected static readonly int CastSpellTwo = Animator.StringToHash("castSpellTwo");
    public static bool IsAlive;

    // Start is called before the first frame update
    private void Awake()
    {
        _maxHealth = 100;
        Health = _maxHealth;
        _isOnGround = true;
        IsCasting = false;
        _abilityTwoReady = true;
        _abilityOneReady = true;
        HumanScript = transform.GetComponent<Human>();
        CatScript = transform.GetComponent<Cat>();
        HumanModel = transform.GetChild(0).gameObject;
        CatModel = transform.GetChild(1).gameObject;
        Rigidbody = GetComponent<Rigidbody>();
        ResourceImage = GameObject.Find("Resource").GetComponent<Image>();
        IsAlive = true;
        _gameInterface = GetComponent<GameInterface>();
    }

    protected virtual void Movement()
    {
        GroundCheck();
        var humanTransform = transform;
        var movement
            = (CurrentForm == "cat"
                  ? (_horizontalInput * Speed / 5)
                  : (_horizontalInput * Speed)) * humanTransform.right +
              humanTransform.forward *
              (((CurrentForm == "cat"
                  ? (_verticalInput < 0 ? _verticalInput / 3 : _verticalInput)
                  : _verticalInput)) * Speed);
        movement.y = Rigidbody.velocity.y;
        if (_jump)
        {
            if (_isOnGround)
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
        Animator.SetFloat(HorizontalMovement, _horizontalInput);
        Animator.SetBool(Jumping, IsJumping);
        _verticalInput = Input.GetAxis("Vertical");
        _horizontalInput = Input.GetAxis("Horizontal");
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

    public void TakeDamage(int damage)
    {
        if (Health >= 0)
        {
            Health -= damage;
        }
        else if (IsAlive)
        {
            //TODO: need death logic
            Animator.SetTrigger(Die);
            IsAlive = false;
        }
    }

    protected void AbilityCooldown(int abilityNo, float cooldown)
    {
        switch (abilityNo)
        {
            case 1:
                _gameInterface.ShowCooldownInActionBar(1, cooldown, CurrentForm);
                StartCoroutine(AbilityCooldownCoroutine(abilityNo, cooldown));
                break;
            case 2:
                _gameInterface.ShowCooldownInActionBar(2, cooldown, CurrentForm);
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
}