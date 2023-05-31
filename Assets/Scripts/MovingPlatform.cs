using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MovingPlatform : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private bool _firstTouch = true;
    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        Sequence s = DOTween.Sequence();
        s.Append(transform.DOMoveX(6, 3).SetRelative().SetEase(Ease.InOutSine));
        s.Append(transform.DOMoveX(-6, 3).SetRelative().SetEase(Ease.InOutSine));
        s.SetLoops(-1, LoopType.Restart);
    }
}
