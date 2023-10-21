using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Portal : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject _player;
    public GameObject interactScreen;
// Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(_player.transform.position, transform.position) < 10 && GameManager.DeadEnemyCount == 25)
        {
                interactScreen.SetActive(true);
                if (Input.GetKeyDown(KeyCode.F))
                {
                    DOTween.KillAll();
                    GameManager.LoadBoss();
                }
            else
            {
                interactScreen.SetActive(false);
            }
        }
    }
}
