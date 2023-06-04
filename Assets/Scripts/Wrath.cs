using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wrath : MonoBehaviour
{
    private float _speed;
    public Vector3 moveDirection;
    public LayerMask enemy;
    private bool _contact;
    private MeshRenderer _meshRenderer;
    private Vector3 _startPosition;
    public GameObject explosion;

    // Start is called before the first frame update
    void Start()
    {
        _startPosition = transform.position;
        _speed = 10.0f;
    }

    private void Awake()
    {
        _meshRenderer = transform.GetChild(1).GetComponent<MeshRenderer>();
    }

    public void SetDirection(Vector3 dir)
    {
        moveDirection = dir;
    }
    
    void Update()
    {
        if (!_contact)
        {
            transform.Translate(moveDirection * (Time.deltaTime * _speed));
        }

        if (Vector3.Distance(_startPosition, transform.position) > 40)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy") && !_contact)
        {
            StartCoroutine(Impact());
            _contact = true;
        }
    }

    private IEnumerator Impact()
    {
        _meshRenderer.enabled = false;
        var tempExplosion = Instantiate(explosion, transform.position, Quaternion.identity);
        var colliders = Physics.OverlapSphere(transform.position, 4, enemy);
        for (int i = 0; i < colliders.Length; i++)
        {
            colliders[i].transform.GetComponent<Enemy>().TakeDamage(20);
        }
        yield return new WaitForSeconds(1);
        Destroy(tempExplosion);
        Destroy(gameObject);
    }
}
