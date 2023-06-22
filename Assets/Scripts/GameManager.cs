using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static Vector3 CheckPointLocation;
    public GameObject player;
    public GameObject deathScreen;
    public CinemachineVirtualCamera vCam;
    public Transform cameraPoint;

    // Start is called before the first frame update
    void Start()
    {
        CheckPointLocation = new Vector3(0, 0, 0);
    }

    public static void LoadGame()
    {
        SceneManager.LoadScene("MainPart");
    }
    
    public static void LoadBoss()
    {
        SceneManager.LoadScene("BossFight");
    }

    public static void LoadMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    // Update is called once per frame
    void Update()
    {
        if ((SceneManager.GetActiveScene() == SceneManager.GetSceneByName("MainPart") || SceneManager.GetActiveScene() == SceneManager.GetSceneByName("BossFight")) && !Player.IsAlive)
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
            deathScreen.SetActive(true);
        }
    }

    public void ResetFromCheckPoint()
    {
        Player.IsAlive = true;
        Player.Health = 100;
        player.transform.GetChild(0).GetComponent<Animator>().SetTrigger("restart");
        player.transform.position = CheckPointLocation;
        deathScreen.SetActive(false);
        vCam.Follow = cameraPoint;
        vCam.LookAt = cameraPoint;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
