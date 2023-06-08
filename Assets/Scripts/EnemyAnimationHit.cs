using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationHit : MonoBehaviour
{
    private EnemySword _sword;
    private Human _playerHuman;
    private Cat _playerCat;
    private Enemy _enemy;
    // Start is called before the first frame update
    void Start()
    {
        _sword = transform.GetComponentInChildren<EnemySword>();
        _playerHuman = GameObject.Find("Player").GetComponent<Human>();
        _playerCat = GameObject.Find("Player").GetComponent<Cat>();
        _enemy = transform.GetComponentInParent<Enemy>();
    }
    

    // Update is called once per frame
    void Update()
    {
        
    }
    

    public void HitPlayer(){
        if (_playerHuman.enabled)
        {
            _playerHuman.TakeDamage(5);
        }
        else if (_playerCat.enabled)
        {
            _playerCat.TakeDamage(5);
        }
    }

    public void EndAttack(){
        _enemy.attackEnded = true;
    }
}
