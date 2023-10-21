using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour, IEnemy
{
    [SerializeField] private float agroRange;
    [SerializeField] private Image healthBar;
    [SerializeField] private float health;
    [SerializeField] private float maxHealth;
    [SerializeField] private float speed;
    [SerializeField] private bool isChasing;
    [SerializeField] private bool attacking;
    [SerializeField] private float attackRange;
    [SerializeField] private Animator animator;
    [SerializeField] private CharacterController characterController;
    [SerializeField] private GameObject player;
    [SerializeField] private bool isAlive;
    [SerializeField] private bool isFeared;
    [SerializeField] private GameInterface gameInterface;
    public GameObject circle;
    private static readonly int Stop = Animator.StringToHash("stop");
    private static readonly int CastSpellOne = Animator.StringToHash("castSpellOne");
    private static readonly int HasTarget = Animator.StringToHash("chasing");
    private static readonly int Attacking = Animator.StringToHash("attacking");
    private static readonly int Die = Animator.StringToHash("die");
    private AnimatorStateInfo _stateInfo;
    
    private void Start()
    {
        health = maxHealth;
        player = GameObject.Find("Player");
        gameInterface = player.GetComponent<GameInterface>();
        StartCoroutine(CastCoroutine());
    }

    private void Update()
    {
        if (isAlive && Player.IsAlive)
        {
            UpdateQuest();
            if (!isFeared)
            {
                CheckPlayerAlive();
                DetectPlayer();
                if (isChasing)
                {
                    ChasePlayer();
                }
            }
        }
        AnimationUpdate();
        StopIfAttacking();
        ResetAfterPlayerDeath();
    }

    private void UpdateQuest()
    {
        gameInterface.SetQuestFillAmount(health / maxHealth);
    }

    private void ResetAfterPlayerDeath()
    {
        if (!Player.IsAlive)
        {
            animator.SetTrigger(Stop);
            isChasing = false;
            health = maxHealth;
        }
    }

    private void DetectPlayer()
    {
        var playerDistance =
            Vector3.Distance(transform.position, player.transform.position);
        if (playerDistance < agroRange)
        {
            isChasing = true;
        }
    }

    private void StopIfAttacking()
    {
        _stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (_stateInfo.IsName("Base Layer.Spell One") || _stateInfo.IsName("Base Layer.Attack"))
        {
            if (_stateInfo.normalizedTime <= 0.9f)
            {
                speed = 0;
            }
        }
        else
        {
            speed = 4;
        }
    }

    private void ChasePlayer()
    {
        var targetDirection = player.transform.position - transform.position;
        var targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 160 *
            Time.deltaTime);
        var playerDistance = Vector3.Distance(player.transform.position, transform.position);
        if (playerDistance < attackRange)
        {
            attacking = true;
        }
        else
        {
            attacking = false;
            characterController.SimpleMove(transform.forward * speed);
        }
    }

    public void TakeDamage(int damage)
    {
        if (health > 0)
        {
            health -= damage;
        }
        else
        {
            if (isAlive)
            {
                animator.SetTrigger(Die);
                isAlive = false;
                gameInterface.QuestFinished();
            }
        }
    }

    private void AnimationUpdate()
    {
        animator.SetBool(HasTarget, isChasing);
        animator.SetBool(Attacking, attacking);
    }

    private void CheckPlayerAlive()
    {
        if (!Player.IsAlive)
        {
            attacking = false;
            isChasing = false;
        }
    }

    public IEnumerator FearCoroutine()
    {
        isFeared = true;
        Debug.Log("isfeared");
        animator.SetBool("fear", isFeared);
        yield return new WaitForSeconds(3);
        isFeared = false;
        Debug.Log("is not feared");
        animator.SetBool("fear", isFeared);
    }

    private IEnumerator CastCoroutine()
    {
        while (true)
        {
            if (isChasing && isAlive)
            {
                animator.SetTrigger(CastSpellOne);
                yield return new WaitForSeconds(2.5f);
                var c = Instantiate(circle, player.transform.position, Quaternion.identity);
                switch (Player.CurrentForm)
                {
                    case Form.Cat:
                        player.GetComponent<Cat>().TakeDamage(6);
                        yield return new WaitForSeconds(1);
                        player.GetComponent<Cat>().TakeDamage(7);
                        yield return new WaitForSeconds(1);
                        player.GetComponent<Cat>().TakeDamage(8);
                        break;
                    case Form.Human:
                        player.GetComponent<Human>().TakeDamage(6);
                        yield return new WaitForSeconds(1);
                        player.GetComponent<Human>().TakeDamage(7);
                        yield return new WaitForSeconds(1);
                        player.GetComponent<Human>().TakeDamage(8);
                        break;
                    case Form.Bear:
                        player.GetComponent<Bear>().TakeDamage(6);
                        yield return new WaitForSeconds(1);
                        player.GetComponent<Bear>().TakeDamage(7);
                        yield return new WaitForSeconds(1);
                        player.GetComponent<Bear>().TakeDamage(8);
                        break;
                }

                Destroy(c, 3);
            }
            yield return new WaitForSeconds(10);
        }
    }
}