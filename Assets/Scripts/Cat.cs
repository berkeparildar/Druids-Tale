using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cat : Player
{
    private AnimatorStateInfo _stateInfo;
    private void Start()
    {
        Speed = 9.0f;
        IsJumping = false;
        IsOnGround = true;
        CurrentForm = "cat";
        JumpForce = 2;
        Health = 75;
        Animator = transform.GetChild(1).GetComponent<Animator>();
        
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