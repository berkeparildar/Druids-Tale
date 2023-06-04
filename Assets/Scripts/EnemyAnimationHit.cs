using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationHit : MonoBehaviour
{
    private EnemySword _sword;
    private Player _player;
    private Enemy _enemy;
    // Start is called before the first frame update
    void Start()
    {
        _sword = transform.GetComponentInChildren<EnemySword>();
        _player = GameObject.Find("Player").GetComponent<Player>();
        _enemy = transform.GetComponentInParent<Enemy>();
    }
    

    // Update is called once per frame
    void Update()
    {
        
    }
    

    public void HitPlayer(){
        if (_sword.inContact)
        {
            _player.TakeDamage(_enemy.Damage);
            Debug.Log("Should damage for" + _enemy.Damage);
        }
    }

    public void EndAttack(){
        _enemy.attackEnded = true;
    }
}
