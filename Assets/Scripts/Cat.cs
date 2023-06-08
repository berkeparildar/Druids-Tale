using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Cat : Player
{
    private AnimatorStateInfo _stateInfo;
    private float _maxEnergy;
    private float _energy;
    public float Energy
    {
        get => _energy;
        set => _energy = value;
    }

    private BoxCollider _boxCollider;
    public LayerMask enemyLayer; 
    public bool hitBite;
    public bool hitShred;

    private void Start()
    {
        Speed = 9.0f;
        IsJumping = false;
        CurrentForm = "cat";
        JumpForce = 2;
        _maxEnergy = 100;
        _energy = _maxEnergy;
        Animator = transform.GetChild(1).GetComponent<Animator>();
        ResourceColor = new Color(1, 1, 0, 0.45f);
        ResourceImage.color = ResourceColor;
        ResourceImage.fillAmount = 1;
        _boxCollider = GetComponentInChildren<BoxCollider>();
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
                hit.transform.GetComponent<Enemy>().TakeDamage(20);
                hitBite = false;
            }
            else if (hitShred)
            {
                StartCoroutine(BleedCoroutine(hit.transform.GetComponent<Enemy>()));
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
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                // bear script
            }
        }
    }

    protected override void CastSpecialAbilityOne()
    {
        if (!IsCasting)
        {
            Animator.SetTrigger(CastSpellOne);
            _energy -= 20;
            IsCasting = true;
            AbilityCooldown(1, 1);
        }
    }

    protected override void CastSpecialAbilityTwo()
    {
        if (!IsCasting)
        {
            Animator.SetTrigger(CastSpellTwo);
            _energy -= 40;
            IsCasting = true;
            AbilityCooldown(2, 3);
        }
    }

    private IEnumerator BleedCoroutine(Enemy enemy)
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
            if (_energy <= _maxEnergy)
            {
                _energy += 5;
            }
            yield return new WaitForSeconds(1);
        }
    }
}