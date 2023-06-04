using System.Collections;
using DG.Tweening;
using UnityEngine;

public class Human : Player
{
    private AnimatorStateInfo _stateInfo;
    public GameObject healingEffect;
    private float _mana;
    private float _maxMana;
    public  Sprite abilityOneIcon;
    public Sprite abilityTwoIcon;
    public Sprite nextForm;
    public Sprite previousForm;


    // Start is called before the first frame update
    private void Start()
    {
        IsJumping = false;
        CurrentForm = "human";
        MaxHealth = 100;
        Health = MaxHealth;
        _maxMana = 100;
        _mana = _maxMana;
        JumpForce = 5.0f;
        Speed = 5.0f;
        Animator = transform.GetChild(0).GetComponent<Animator>();
        ResourceColor = new Color(0, 0, 1, 0.59f);
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
        ResourceImage.fillAmount = _mana / _maxMana;
        AbilityOneSlot.sprite = abilityOneIcon;
        AbilityTwoSlot.sprite = abilityTwoIcon;
        PreviousFormSlot.sprite = previousForm;
        NextFormSlot.sprite = nextForm;
    }

    // Update is called once per frame
    void Update()
    {
        AbilityCheck();
        Morph();
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

    protected override void SpecialAbilityOne()
    {
        if (AbilityOneReady)
        {
            IsCasting = true;
            Animator.SetTrigger(CastSpellOne);
            AbilityCooldown(1, 7);
        }
    }

    protected override void SpecialAbilityTwo()
    {
        if (AbilityTwoReady)
        {
            IsCasting = true;
            StartCoroutine(Regeneration());
            Animator.SetTrigger(CastSpellTwo);
            AbilityCooldown(2, 10);
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
}