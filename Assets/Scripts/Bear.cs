using UnityEngine;

public class Bear : Player
{
    public bool hitSwipe;
    public float Rage { get; private set; }
    public LayerMask enemyLayer; 
    private AnimatorStateInfo _stateInfo;

    void Start()
    {
        CurrentForm = Form.Bear;
        Animator = transform.GetChild(2).GetComponent<Animator>();
        JumpForce = 2.0f;
        Speed = 4.0f;
        Rage = 0;
    }
    

    private void FixedUpdate()
    {
        var position = transform.position;
        var raycastPos = new Vector3(position.x, position.y + 1, position.z);
        if (Physics.Raycast(raycastPos, transform.forward, out var hit, 1.6f, enemyLayer))
        {
            if (hitSwipe)
            {
                hit.transform.GetComponent<IEnemy>().TakeDamage(10);
                Rage += 34;
                if (Rage > 100)
                {
                    Rage = 100;
                }
                hitSwipe = false;
            }
        }
        Movement();
        
    }

    private void Update()
    {
        AbilityCheck();
        if (!IsCasting)
        {
            Morph();
        }
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
                CatScript.enabled = true;
                BearModel.SetActive(false);
                CatModel.SetActive(true);
                CurrentForm = Form.Cat;
                GameInterface.UpdateUIAccordingToForm();
                BearScript.enabled = false;
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                HumanScript.enabled = true;
                BearModel.SetActive(false);
                HumanModel.SetActive(true);
                CurrentForm = Form.Human;
                GameInterface.UpdateUIAccordingToForm();
                BearScript.enabled = false;
            }
        }
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage / 2);
        Rage += (damage / 2);
    }

    protected override void CastSpecialAbilityOne()
    {
        if (!IsCasting)
        {
            Animator.SetTrigger(CastSpellOne);
            IsCasting = true;
            AbilityCooldown(1, 2);
        }
    }

    protected override void CastSpecialAbilityTwo()
    {
        if (!IsCasting && Rage >= 100)
        {
            Animator.SetTrigger(CastSpellTwo);
            Fear();
            Rage -= 100;
            IsCasting = true;
            AbilityCooldown(2, 0);
        }
    }
    
    private void Fear()
    {
        var colliders = Physics.OverlapSphere(transform.position, 4, enemyLayer);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject.name.Contains("Enemy"))
            {
                var fearCoroutine = colliders[i].transform.GetComponent<Enemy>().FearCoroutine();
                StartCoroutine(fearCoroutine);
            }
        }
    }
}