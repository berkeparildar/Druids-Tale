using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Boss : MonoBehaviour, IEnemy
{
    public GameObject bossUI;
    public GameObject barrier;
    private float _agroRange;
    private static readonly int HasTarget = Animator.StringToHash("chasing");
    private static readonly int Attacking = Animator.StringToHash("attacking");
    private static readonly int Die = Animator.StringToHash("die");
    public Image healthBar;
    private float _health;
    private float _maxHealth;
    private float _speed;
    private bool _isChasing;
    private bool _attacking;
    private float _attackRange;
    private Animator _animator;
    private CharacterController _character;
    private static GameObject _player;
    public bool isAlive;
    private bool _isFeared;
    public GameObject circle;
    private static readonly int Stop = Animator.StringToHash("stop");
    private static readonly int CastSpellOne = Animator.StringToHash("castSpellOne");
    private AnimatorStateInfo _stateInfo;
    

    // Start is called before the first frame update
    void Start()
    {
        _maxHealth = 500;
        _health = _maxHealth;
        _animator = transform.GetChild(0).GetComponent<Animator>();
        _speed = 4.0f;
        _agroRange = 30;
        _attackRange = 6;
        _character = GetComponent<CharacterController>();
       _player = GameObject.Find("Player");
        isAlive = true;
        StartCoroutine(CastCoroutine());
    }

    // Update is called once per frame
    void Update()
    {
        _stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
        if (_stateInfo.IsName("Base Layer.Spell One"))
        {
            if (_stateInfo.normalizedTime <= 0.9f)
            {
                _character.SimpleMove(Vector3.zero);
            }
        }
        if (isAlive && Player.IsAlive)
        {
            healthBar.fillAmount = (_health / _maxHealth);
            if (!_isFeared)
            {
                CheckPlayerAlive();
                DetectPlayer();
                if (_isChasing)
                {
                    ChasePlayer();
                }
            }
        }
        AnimationUpdate();

        if (!Player.IsAlive)
        {
            barrier.GetComponent<BoxCollider>().isTrigger = true;
            barrier.GetComponent<MeshRenderer>().enabled = false;
            bossUI.SetActive(false);
            _animator.SetTrigger(Stop);
            _isChasing = false;
            _health = _maxHealth;
        }
    }
    
    private void DetectPlayer()
    {
        var playerDistance =
            Vector3.Distance(transform.position, _player.transform.position);
        if (playerDistance < _agroRange)
        {
            _isChasing = true;
        }
    }
    
    private void ChasePlayer()
    {
        bossUI.SetActive(true);
        var targetDirection = _player.transform.position - transform.position;
        var targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);
        ;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 160 * 
            Time.deltaTime);
        var playerDistance = Vector3.Distance(_player.transform.position, transform.position);
        if (playerDistance < _attackRange)
        {
            _attacking = true;
        }
        else
        {
            _attacking = false;
        }
        if (!_attacking)
        {
            _character.SimpleMove(transform.forward * _speed);
        }
    }
    
    public void TakeDamage(int damage)
    {
        if (_health > 0)
        {
            _health -= damage;
        }
        else
        {
            if (isAlive)
            {
                _animator.SetTrigger(Die);
            }
            isAlive = false;
            Destroy(healthBar);
        }
    }
    
    private void AnimationUpdate()
    {
        _animator.SetBool(HasTarget, _isChasing);
        _animator.SetBool(Attacking, _attacking);
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
        _animator.SetBool("fear", _isFeared);
        yield return new WaitForSeconds(3);
        _isFeared = false;
        Debug.Log("is not feared");
        _animator.SetBool("fear", _isFeared);
    }

    private IEnumerator CastCoroutine()
    {
        while (true)
        {
            if (_isChasing && isAlive)
            {
                _animator.SetTrigger(CastSpellOne);
                yield return new WaitForSeconds(3);
                var c = Instantiate(circle, _player.transform.position, Quaternion.identity);
                switch (Player.CurrentForm)
                {
                    case Form.Cat:
                        _player.GetComponent<Cat>().TakeDamage(6);
                        yield return new WaitForSeconds(1);
                        _player.GetComponent<Cat>().TakeDamage(7);
                        yield return new WaitForSeconds(1);
                        _player.GetComponent<Cat>().TakeDamage(8);
                        break;
                    case Form.Human:
                        _player.GetComponent<Human>().TakeDamage(6);
                        yield return new WaitForSeconds(1);
                        _player.GetComponent<Human>().TakeDamage(7);
                        yield return new WaitForSeconds(1);
                        _player.GetComponent<Human>().TakeDamage(8);
                        break;
                    case Form.Bear:
                        _player.GetComponent<Bear>().TakeDamage(6);
                        yield return new WaitForSeconds(1);
                        _player.GetComponent<Bear>().TakeDamage(7);
                        yield return new WaitForSeconds(1);
                        _player.GetComponent<Bear>().TakeDamage(8);
                        break;
                }
                Destroy(c,3);
            }
            yield return new WaitForSeconds(10);
        }
    }
}
