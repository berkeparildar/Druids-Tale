using System.Collections.Generic;
using UnityEngine;

public class InvisiblePlatformStone : MonoBehaviour
{
    private GameObject _player;
    public GameObject interactScreen;
    private List<SpecialPlatforms> _invisiblePlatforms = new List<SpecialPlatforms>();

    private bool _once;
    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player");
        GameObject[] platforms = GameObject.FindGameObjectsWithTag("Invisible");
        foreach (var platform in platforms)
        {
            _invisiblePlatforms.Add(platform.GetComponent<SpecialPlatforms>());
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(_player.transform.position, transform.position) < 10)
        {
            if (Vector3.Distance(_player.transform.position, transform.position) < 5)
            {
                interactScreen.SetActive(true);
                if (Input.GetKeyDown(KeyCode.F))
                {
                    foreach (var platform in _invisiblePlatforms)
                    {
                        var coroutine = platform.TurnVisible();
                        StartCoroutine(coroutine);
                    }
                }
            }else
            {
                interactScreen.SetActive(false);
            }
        }
    }

   
}
