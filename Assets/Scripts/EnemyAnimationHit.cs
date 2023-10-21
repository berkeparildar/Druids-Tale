using UnityEngine;

public class EnemyAnimationHit : MonoBehaviour
{
    [SerializeField] private EnemySword sword;
    [SerializeField] private Human playerHuman;
    [SerializeField] private Cat playerCat;
    [SerializeField] private Bear playerBear;
    [SerializeField] private Enemy enemy;
    [SerializeField] private GameObject fireParticle;
    [SerializeField] private Vector3 hitPosition;
    [SerializeField] private int damage;

    private void Start()
    {
        var player = GameObject.Find("Player");
        playerHuman = player.GetComponent<Human>();
        playerCat = player.GetComponent<Cat>();
        playerBear = player.GetComponent<Bear>();
     }
    
    public void HitPlayer()
    {
        if (!sword.inContact) return;
        switch (Player.CurrentForm)
        {
            case Form.Human:
                playerHuman.TakeDamage(damage);
                hitPosition = playerHuman.transform.position;
                break;
            case Form.Cat:
                playerCat.TakeDamage(damage);
                hitPosition = playerHuman.transform.position;
                break;
            case Form.Bear:
                playerBear.TakeDamage(damage);
                hitPosition = playerHuman.transform.position;
                break;
        }
        hitPosition.y += 1;
        var particle = Instantiate(fireParticle, hitPosition, Quaternion.identity);
        particle.GetComponent<ParticleSystem>().Play();
        Destroy(particle, 0.5f);
    }

    public void EndAttack(){
        enemy.attackEnded = true;
    }
}
