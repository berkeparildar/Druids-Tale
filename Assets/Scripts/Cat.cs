using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cat : Player
{
    private AnimatorStateInfo _stateInfo;
    private float _maxEnergy;
    private float _energy;
    public  Sprite abilityOneIcon;
    public Sprite abilityTwoIcon;
    public Sprite nextForm;
    public Sprite previousForm;
    private void Start()
    {
        Speed = 9.0f;
        IsJumping = false;
        CurrentForm = "cat";
        JumpForce = 2;
        Health = 75;
        _maxEnergy = 100;
        _energy = _maxEnergy;
        Animator = transform.GetChild(1).GetComponent<Animator>();
        ResourceColor = new Color(1, 1, 0, 0.45f);
        ResourceImage.color = ResourceColor;
        ResourceImage.fillAmount = 1;   
        AbilityOneSlot.sprite = abilityOneIcon;
        AbilityTwoSlot.sprite = abilityTwoIcon;
        PreviousFormSlot.sprite = previousForm;
        NextFormSlot.sprite = nextForm;
    }

    private void OnEnable()
    {
        ResourceImage.color = ResourceColor;
        ResourceImage.fillAmount = _energy / _maxEnergy;
        AbilityOneSlot.sprite = abilityOneIcon;
        AbilityTwoSlot.sprite = abilityTwoIcon;
        PreviousFormSlot.sprite = previousForm;
        NextFormSlot.sprite = nextForm;
    }

    private void FixedUpdate()
    {
        Movement();
    }
    
    private void Update()
    {
        AbilityCheck();
        Morph();
        GetInput();
        _stateInfo = Animator.GetCurrentAnimatorStateInfo(0);
        if (_stateInfo.IsName("Base Layer.Spell One"))
        {
            Debug.Log("in attack anim");
            if (_stateInfo.normalizedTime >= 0.9f)
            {
                IsCasting = false;
            }
        }
    }

    protected override void Morph()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            HumanScript.enabled = true;
            CatModel.SetActive(false);
            HumanModel.SetActive(true);
            CatScript.enabled = false;
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            // bear script
        }
    }

    protected override void SpecialAbilityOne()
    {
        Animator.SetTrigger(CastSpellOne);
        IsCasting = true;
    }
}