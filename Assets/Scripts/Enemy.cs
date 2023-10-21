using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour, IEnemy
{
    private static readonly int HasTarget = Animator.StringToHash("hasTarget");
    private static readonly int Attacking = Animator.StringToHash("attacking");
    public GameObject healthBarPrefab;
    private GameObject _healthBar;
    private HealthBar _healthBarComponent;
    protected float Health;
    protected float MaxHealth;
    public int Damage;
    public bool attackEnded;
    protected float Speed;
    protected Rigidbody Rigidbody;
    private bool _isChasing;
    private bool _attacking;
    protected int AgroRange;
    protected float AttackRange;
    protected Animator Animator;
    protected CharacterController CharacterController;
    private static GameObject _player;
    private static readonly int Die = Animator.StringToHash("die");
    protected bool IsAlive;
    private bool _isFeared;

    // Start is called before the first frame update
    void Start()
    {
        Damage = 10;
        MaxHealth = 70;
        Health = MaxHealth;
        attackEnded = true;
        Animator = transform.GetChild(0).GetComponent<Animator>();
        Speed = 4.0f;
        AgroRange = 15;
        AttackRange = 1.5f;
        CharacterController = GetComponent<CharacterController>();
        _player = GameObject.Find("Player"); //TODO: this is causing problems
        IsAlive = true;
        // I am guessing where I set the position does not matter since it is set in the health-bar's start function
        _healthBar = Instantiate(healthBarPrefab, transform.position, Quaternion.identity);
        _healthBarComponent = _healthBar.GetComponent<HealthBar>();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsAlive && Player.IsAlive)
        {
            _healthBarComponent.SetPositionAndHealth(transform.position, (Health / 70));
            if (!_isFeared)
            {
                CheckPlayerAlive();
                AnimationUpdate();
                DetectPlayer();
                if (_isChasing)
                {
                    ChasePlayer();
                }
            }
        }
    }

    private void DetectPlayer()
    {
        var playerDistance =
            Vector3.Distance(transform.position, _player.transform.position);
        if (playerDistance < AgroRange)
        {
            _isChasing = true;
            _healthBarComponent.isInRange = true;
        }
        else
        {
            _isChasing = false;
            _healthBarComponent.isInRange = false;
        }
    }

    private void ChasePlayer()
    {
        var targetDirection = _player.transform.position - transform.position;
        var targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);
        ;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 160 * 
            Time.deltaTime);
        var playerDistance = Vector3.Distance(_player.transform.position, transform.position);
        if (playerDistance < AttackRange)
        {
            _attacking = true;
            attackEnded = false;
        }
        else
        {
            _attacking = false;
            // transform.Translate(Vector3.forward * (Time.deltaTime * Speed));
        }

        if (!_attacking && attackEnded)
        {
            CharacterController.SimpleMove(transform.forward * Speed);
        }
    }

    public void TakeDamage(int damage)
    {
        if (Health > damage)
        {
            Health -= damage;
        }
        else
        {
            if (IsAlive)
            {
                Animator.SetTrigger(Die);
                IsAlive = false;
                Destroy(_healthBar);
                GameManager.DeadEnemyCount++;
                Destroy(this.gameObject, 3);
            }
        }
    }

    private void AnimationUpdate()
    {
        Animator.SetBool(HasTarget, _isChasing);
        Animator.SetBool(Attacking, _attacking);
    }

    protected void CheckPlayerAlive()
    {
        if (!Player.IsAlive)
        {
            _attacking = false;
            _isChasing = false;
        }
    }

    public IEnumerator FearCoroutine()
    {
        _isFeared = true;
        Debug.Log("isfeared");
        Animator.SetBool("fear", _isFeared);
        yield return new WaitForSeconds(3);
        _isFeared = false;
        Debug.Log("is not feared");
        Animator.SetBool("fear", _isFeared);
    }
    
}