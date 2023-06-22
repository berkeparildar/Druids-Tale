using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationHit : MonoBehaviour
{
    private EnemySword _sword;
    private Human _playerHuman;
    private Cat _playerCat;
    private Bear _playerBear;
    private Enemy _enemy;
    public GameObject fireParticle;
    private Vector3 _hitPosition;
    private int _damage;

    void Start()
    {
        _damage = 5;
        _sword = transform.GetComponentInChildren<EnemySword>();
        _playerHuman = GameObject.Find("Player").GetComponent<Human>();
        _playerCat = GameObject.Find("Player").GetComponent<Cat>();
        _playerBear = GameObject.Find("Player").GetComponent<Bear>();
        _enemy = transform.GetComponentInParent<Enemy>();
        if (gameObject.name.Equals("Maw"))
        {
            _damage = 20;
        }
    }
    
    public void HitPlayer()
    {
        if (_sword.inContact)
        {
            switch (Player.CurrentForm)
            {
                case Form.Human:
                    _playerHuman.TakeDamage(_damage);
                    _hitPosition = _playerHuman.transform.position;
                    break;
                case Form.Cat:
                    _playerCat.TakeDamage(_damage);
                    _hitPosition = _playerHuman.transform.position;
                    break;
                case Form.Bear:
                    _playerBear.TakeDamage(_damage);
                    _hitPosition = _playerHuman.transform.position;
                    break;
            }
        }

        _hitPosition.y += 1;
        var particle = Instantiate(fireParticle, _hitPosition, Quaternion.identity);
        particle.GetComponent<ParticleSystem>().Play();
        Destroy(particle, 0.5f);
    }

    public void EndAttack(){
        _enemy.attackEnded = true;
    }
}
