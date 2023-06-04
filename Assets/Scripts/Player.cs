using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    protected Animator Animator;
    private LayerMask _groundLayer;
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
    protected float Health;
    protected float MaxHealth;
    protected float JumpForce;
    private bool _isOnGround = true;
    protected float Speed;
    private bool _jump;
    protected bool AbilityOneReady;
    protected bool AbilityTwoReady;
    private Image _healthImage;
    protected Image ResourceImage;
    protected Color ResourceColor;
    protected Image NextFormSlot;
    protected Image PreviousFormSlot;
    protected Image AbilityOneSlot;
    protected Image AbilityTwoSlot;
    private static readonly int Jumping = Animator.StringToHash("isJumping");
    private static readonly int VerticalMovement = Animator.StringToHash("verticalMovement");
    private static readonly int HorizontalMovement = Animator.StringToHash("horizontalMovement");
    protected static readonly int CastSpellOne = Animator.StringToHash("castSpellOne");
    protected static readonly int CastSpellTwo = Animator.StringToHash("castSpellTwo");

    // Start is called before the first frame update
    private void Awake()
    {
        _groundLayer = LayerMask.NameToLayer("Ground");
        _isOnGround = true;
        IsCasting = false;
        AbilityTwoReady = true;
        AbilityOneReady = true;
        HumanScript = transform.GetComponent<Human>();
        CatScript = transform.GetComponent<Cat>();
        HumanModel = transform.GetChild(0).gameObject;
        CatModel = transform.GetChild(1).gameObject;
        Rigidbody = GetComponent<Rigidbody>();
        _healthImage = GameObject.Find("Health").GetComponent<Image>();
        ResourceImage = GameObject.Find("Resource").GetComponent<Image>();
        NextFormSlot = GameObject.Find("NextForm").GetComponent<Image>();
        PreviousFormSlot = GameObject.Find("PreviousForm").GetComponent<Image>();
        AbilityOneSlot = GameObject.Find("AbilityOne").GetComponent<Image>();
        AbilityTwoSlot = GameObject.Find("AbilityTwo").GetComponent<Image>();
    }

    protected virtual void Movement()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 0.2f, LayerMask.NameToLayer("Ground")))
        {
            _isOnGround = true;
            IsJumping = false;
            Animator.SetBool(Jumping, IsJumping);
            if (hit.transform.gameObject.CompareTag("Platform"))
            {
                transform.SetParent(hit.transform);
            }
        }
        else
        {
            _isOnGround = false;
            transform.SetParent(null);
        }

        var humanTransform = transform;
        var movement = (CurrentForm == "cat" ? (_horizontalInput * Speed / 5) : (_horizontalInput * Speed)) * humanTransform.right +
                       humanTransform.forward * (((CurrentForm == "cat" ? (_verticalInput < 0 ? _verticalInput / 3 : _verticalInput) : _verticalInput)) * Speed);
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
    public void TakeDamage(int damage)
    {
        Health -= damage;
        _healthImage.DOFillAmount((Health / MaxHealth), 0.5f);
        // if (Health <= 0)
        //die
        // Destroy(this);
    }

    protected void AbilityCooldown(int abilityNo, float cooldown)
    {
        switch (abilityNo)
        {
            case 1:
                var cooldownOneImage = AbilityOneSlot.transform.GetChild(0).GetComponent<Image>();
                cooldownOneImage.fillAmount = 1;
                cooldownOneImage.DOFillAmount((0), cooldown);
                StartCoroutine(AbilityCooldownCoroutine(abilityNo, cooldown));
                break;
            case 2:
                var cooldownTwoImage = AbilityTwoSlot.transform.GetChild(0).GetComponent<Image>();
                cooldownTwoImage.fillAmount = 1;
                cooldownTwoImage.DOFillAmount((0), cooldown);
                StartCoroutine(AbilityCooldownCoroutine(abilityNo, cooldown));
                break;
        }
    }

    IEnumerator AbilityCooldownCoroutine(int abilityNo, float cooldown)
    {
        switch (abilityNo)
        {
            case 1:
                AbilityOneReady = false;
                yield return new WaitForSeconds(cooldown);
                AbilityOneReady = true;
                break;
            case 2:
                AbilityTwoReady = false;
                yield return new WaitForSeconds(cooldown);
                AbilityTwoReady = true;
                break;
        }
    }
}