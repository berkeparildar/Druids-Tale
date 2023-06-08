using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private GameObject _canvas;
    private Vector3 _position;
    private Camera _camera;
    private Image _barImage;

    public void SetPositionAndHealth(Vector3 enemyPos, float fillAmount)
    {
        _position = new Vector3(enemyPos.x, enemyPos.y + 2, enemyPos.z);
        _barImage.fillAmount = fillAmount;
    }
    
    void Start()
    {
        _camera = GameObject.Find("Main Camera").GetComponent<Camera>();
        _barImage = transform.GetChild(1).GetComponent<Image>();
        _canvas = GameObject.Find("Canvas");
        transform.SetParent(_canvas.transform);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = _camera.WorldToScreenPoint(_position);
    }
}
