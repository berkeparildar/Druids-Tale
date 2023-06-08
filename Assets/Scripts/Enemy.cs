using DG.Tweening;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private static readonly int HasTarget = Animator.StringToHash("hasTarget");
    private static readonly int Attacking = Animator.StringToHash("attacking");
    protected float Health;
    protected float MaxHealth;
    public int Damage;
    public bool attackEnded;
    protected float Speed;
    protected Rigidbody Rigidbody;
    private bool _isChasing;
    private bool _attacking;
    private Transform _playerTransform;
    protected int AgroRange;
    protected float AttackRange;
    protected Animator Animator;
    protected CharacterController CharacterController;
    private GameObject _player;
    private static readonly int Die = Animator.StringToHash("die");
    protected bool IsAlive;

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
        _playerTransform = _player.transform;
        IsAlive = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsAlive)
        {
            CheckPlayerAlive();
            Debug.Log(Health);
            AnimationUpdate();
            DetectPlayer();
            if (_isChasing)
            {
                ChasePlayer();
            }
            FaceHealthBar();
        }
    }

    private void DetectPlayer()
    {
        var playerDistance =
            Vector3.Distance(transform.position, _playerTransform.position);
        if (playerDistance < AgroRange)
        {
            _isChasing = true;
        }
        else
        {
            _isChasing = false;
        }
    }

    //TODO: Enemy AI as dum as me, need fix
    private void ChasePlayer()
    {
        var targetDirection = _playerTransform.position - transform.position;
        var targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);
        ;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 160 * 
            Time.deltaTime);
        var playerDistance = Vector3.Distance(_playerTransform.position, transform.position);
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
        Health -= damage;
        if (Health <= 0)
        {
            Animator.SetTrigger(Die);
            Destroy(this.gameObject, 3);
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

    private void FaceHealthBar()
    {
        var healthBar = transform.GetChild(1);
        var targetDirection = _playerTransform.position - transform.position;
        var targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);
        ;
        healthBar.localRotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 160 * 
            Time.deltaTime);
    }
}