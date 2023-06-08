using System.Collections;
using DG.Tweening;
using UnityEngine;

public class Human : Player
{
    private AnimatorStateInfo _stateInfo;
    public GameObject wrath;
    public GameObject healingEffect;

    public float Mana { get; set; }

    private float _maxMana;
    
    // Start is called before the first frame update
    private void Start()
    {
        IsJumping = false;
        CurrentForm = "human";
        _maxMana = 360;
        Mana = _maxMana;
        JumpForce = 5.0f;
        Speed = 5.0f;
        Animator = transform.GetChild(0).GetComponent<Animator>();
        ResourceImage.color = ResourceColor;
        ResourceImage.fillAmount = 1;
        StartCoroutine(RegenerateMana());
    }

    private void OnEnable()
    {
        ResourceImage.color = ResourceColor;
        ResourceImage.fillAmount = Mana / _maxMana;
    }

    // Update is called once per frame
    void Update()
    {
        AbilityCheck();
        if (!IsCasting)
        {
            Morph();
        }
        GetInput();
        _stateInfo = Animator.GetCurrentAnimatorStateInfo(0);
        if (_stateInfo.IsName("Base Layer.Spell One") ||_stateInfo.IsName("Base Layer.Spell Two"))
        {
            if (_stateInfo.normalizedTime >= 0.9f)
            {
                IsCasting = false;
            }
        }
    }
    
    private void FixedUpdate()
    {
        Movement();
    }
    protected override void Morph()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            CatScript.enabled = true;
            HumanModel.SetActive(false);
            CatModel.SetActive(true);
            HumanScript.enabled = false;
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            //bear
        }
    }

    protected override void CastSpecialAbilityOne()
    {
        if (Mana > 60 && !IsCasting)
        {
            Animator.SetTrigger(CastSpellOne);
            IsCasting = true;
            Mana -= 60;
            AbilityCooldown(1, 7);
        }
    }

    protected override void CastSpecialAbilityTwo()
    {
        if (Mana > 40 && !IsCasting) 
        {
            Animator.SetTrigger(CastSpellTwo);
            IsCasting = true;
            Mana -= 40;
            AbilityCooldown(2, 10);
            StartCoroutine(Regeneration());
        }
    }

    private IEnumerator Regeneration()
    {
        var healEffect = Instantiate(healingEffect, transform.position, Quaternion.identity);
        healEffect.transform.SetParent(transform);
        Health += 20;
        yield return new WaitForSeconds(2); // Test later
        Health += 20;
        yield return new WaitForSeconds(2);
        Health += 20;
        Destroy(healEffect);
    }

    private IEnumerator RegenerateMana()
    {
        while (true)
        {
            if (Mana <= _maxMana)
            {
                Mana += 1;
            }

            yield return new WaitForSeconds(0.5f);
        }
    }

    public void WrathAbility()
    {
        var pos = GameObject.Find("WrathInitPos").transform.position;
        var initializedWrath = Instantiate(this.wrath, pos
            , Quaternion.identity);
        initializedWrath.GetComponent<Wrath>().SetDirection(transform.forward);
    }
}