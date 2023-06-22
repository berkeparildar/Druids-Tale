using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private GameObject _canvas;
    private GameObject _healthBarContainer;
    private Vector3 _position;
    private Camera _camera;
    private Image _barImage;
    public bool isInRange;

    public void SetPositionAndHealth(Vector3 enemyPos, float fillAmount)
    {
        _position = new Vector3(enemyPos.x, enemyPos.y + 2, enemyPos.z);
        _barImage.fillAmount = fillAmount;
    }
    
    void Start()
    {
        // I want bars to be visible only in range, I set that by putting healthbars in canvas
        // So if not in range then I put them in the container, if in range canvas.
        _healthBarContainer = GameObject.Find("Healthbar Container");
        _camera = GameObject.Find("Camera").GetComponent<Camera>();
        _barImage = transform.GetChild(1).GetComponent<Image>();
        _canvas = GameObject.Find("Canvas");
    }

    // Update is called once per frame
    void Update()
    {
        if (isInRange)
        {
            transform.SetParent(_canvas.transform);
        }
        else
        {
            transform.SetParent(_healthBarContainer.transform);
        }
        transform.position = _camera.WorldToScreenPoint(_position);
    }
}
