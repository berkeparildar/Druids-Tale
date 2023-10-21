using UnityEngine;

public class CheckPointStone : MonoBehaviour
{
    private GameObject _player;
    public GameObject interactScreen;
    public GameObject particle;

    private Vector3 _position;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(_player.transform.position, transform.position) < 10)
        {
            if (Vector3.Distance(_player.transform.position, transform.position) < 3)
            {
                interactScreen.SetActive(true);
                if (Input.GetKeyDown(KeyCode.F))
                {
                    _position = transform.position;
                    _position.y += 1;
                    var p = Instantiate(particle, _position, Quaternion.identity);
                    GameManager.CheckPointLocation
                        = new Vector3(_position.x, _position.y + 1, _position.z + 1);
                    Destroy(p, 3);
                }
            }
            else
            {
                interactScreen.SetActive(false);
            }
        }
    }
}