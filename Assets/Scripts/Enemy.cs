using UnityEngine;
public class Enemy : MonoBehaviour {

    private static readonly int HasTarget = Animator.StringToHash("hasTarget");
    private static readonly int Attacking = Animator.StringToHash("attacking");
    protected int Health;
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
    
  // Start is called before the first frame update
    void Start()
    {
        Health = 70;
        attackEnded = true;
        Animator = transform.GetChild(0).GetComponent<Animator>();
        Speed = 4.0f;
        AgroRange = 15;
        AttackRange = 1.5f;
        CharacterController = GetComponent<CharacterController>();
        _playerTransform = GameObject.Find("Player").transform;
    }

  // Update is called once per frame
    void Update() {
        Debug.Log(Health);
        AnimationUpdate();
        DetectPlayer();
        if (_isChasing)
        {
            ChasePlayer();
        }
    }

    private void DetectPlayer() {
        var playerDistance =
        Vector3.Distance(transform.position, _playerTransform.position);
        if (playerDistance < AgroRange) {
            _isChasing = true;
        }  
        else {
            _isChasing = false;
        }
    }

    private void ChasePlayer() {
        var targetDirection = _playerTransform.position - transform.position;
        var targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 160 * Time.deltaTime);
        var playerDistance = Vector3.Distance(_playerTransform.position, transform.position);
        if (playerDistance < AttackRange)
        {
            _attacking = true;
            attackEnded = false;
        }
        else {   
            _attacking = false;
            // transform.Translate(Vector3.forward * (Time.deltaTime * Speed));
        }

        if (!_attacking && attackEnded)
        {
            CharacterController.SimpleMove(transform.forward * Speed);
        }
    }

    public void TakeDamage(int damage){
        Health -= damage;
        if (Health <= 0)
        {
            //die 
        }
    }

    private void AnimationUpdate(){
        Animator.SetBool(HasTarget, _isChasing);
        Animator.SetBool(Attacking, _attacking);
    }

    protected void Attack(){
        
    }
}
