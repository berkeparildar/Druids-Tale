using System.Collections;
using UnityEngine;

public class Human : Player
{
    private AnimatorStateInfo _stateInfo;

    public GameObject healingEffect;
    // Start is called before the first frame update
    private void Start()
    {
        IsJumping = false;
        IsOnGround = true;
        CurrentForm = "human";
        Health = 100;
        JumpForce = 5.0f;
        Speed = 5.0f;
        Animator = transform.GetChild(0).GetComponent<Animator>();
        _stateInfo = Animator.GetCurrentAnimatorStateInfo(0);
    }

    // Update is called once per frame
    void Update()
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
        StartCoroutine(Regeneration());
    }

    protected override void SpecialAbilityTwo()
    {
        IsCasting = true;
        Animator.SetTrigger(CastSpellOne);
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