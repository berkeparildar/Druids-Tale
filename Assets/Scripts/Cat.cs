using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Cat : Player
{
    private AnimatorStateInfo _stateInfo;
    private float _maxEnergy;
    public float Energy { get; private set; }
    public LayerMask enemyLayer; 
    public bool hitBite;
    public bool hitShred;

    private void Start()
    {
        Speed = 8.0f;
        IsJumping = false;
        CurrentForm = Form.Cat;
        JumpForce = 3;
        _maxEnergy = 100;
        Energy = _maxEnergy;
        Animator = transform.GetChild(1).GetComponent<Animator>();
        ResourceColor = new Color(1, 1, 0, 0.45f);
        ResourceImage.color = ResourceColor;
        ResourceImage.fillAmount = 1;
        StartCoroutine(RegenerateEnergy());
    }
    
    private void FixedUpdate()
    {
        var position = transform.position;
        var raycastPos = new Vector3(position.x, position.y + 1, position.z);
        if (Physics.Raycast(raycastPos, transform.forward, out var hit, 1.6f, enemyLayer))
        {
            if (hitBite)
            {
                hit.transform.GetComponent<IEnemy>().TakeDamage(30);
                hitBite = false;
            }
            else if (hitShred)
            {
                StartCoroutine(BleedCoroutine(hit.transform.GetComponent<IEnemy>()));
                hitShred = false;
            }
        }
        Movement();
        Debug.DrawRay(raycastPos, transform.forward * 1.6f, Color.red);
    }

    private void Update()
    {
        AbilityCheck();
        Morph();
        GetInput();
        _stateInfo = Animator.GetCurrentAnimatorStateInfo(0);
        if (_stateInfo.IsName("Base Layer.Spell One") || _stateInfo.IsName("Base Layer.Spell Two"))
        {
            if (_stateInfo.normalizedTime >= 0.9f)
            {
                IsCasting = false;
            }
        }
    }

    protected override void Morph()
    {
        if (!IsCasting)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                HumanScript.enabled = true;
                CatModel.SetActive(false);
                HumanModel.SetActive(true);
                CatScript.enabled = false;
                CurrentForm = Form.Human;
                GameInterface.UpdateUIAccordingToForm();
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                BearScript.enabled = true;
                CatModel.SetActive(false);
                BearModel.SetActive(true);
                CatScript.enabled = false;
                CurrentForm = Form.Bear;
                GameInterface.UpdateUIAccordingToForm();
            }
        }
    }

    protected override void CastSpecialAbilityOne()
    {
        if (!IsCasting && Energy >= 20)
        {
            Animator.SetTrigger(CastSpellOne);
            Energy -= 20;
            IsCasting = true;
            AbilityCooldown(1, 1);
        }
    }

    protected override void CastSpecialAbilityTwo()
    {
        if (!IsCasting && Energy >= 40)
        {
            Animator.SetTrigger(CastSpellTwo);
            Energy -= 40;
            IsCasting = true;
            AbilityCooldown(2, 3);
        }
    }

    private IEnumerator BleedCoroutine(IEnemy enemy)
    {
        enemy.TakeDamage(10);
        yield return new WaitForSeconds(1);
        enemy.TakeDamage(10);
        yield return new WaitForSeconds(1);
        enemy.TakeDamage(10);
    }

    private IEnumerator RegenerateEnergy()
    {
        while (true)
        {
            if (Energy <= _maxEnergy)
            {
                Energy += 10;
            }
            yield return new WaitForSeconds(1);
        }
    }
}