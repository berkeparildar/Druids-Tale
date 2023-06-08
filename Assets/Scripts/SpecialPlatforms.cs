using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SpecialPlatforms : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private bool _firstTouch = true;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();

        switch (gameObject.tag)
        {
            case "MovingLeft":
                Sequence leftMovement = DOTween.Sequence();
                leftMovement.Append(transform.DOMoveX(6, 3).SetRelative()
                    .SetEase(Ease.InOutSine));
                leftMovement.Append(transform.DOMoveX(-6, 3).SetRelative()
                    .SetEase(Ease.InOutSine));
                leftMovement.SetLoops(-1, LoopType.Restart);
                break;
            case "MovingRight":
                Sequence rightMovement = DOTween.Sequence();
                rightMovement.Append(transform.DOMoveX(-6, 3).SetRelative()
                    .SetEase(Ease.InOutSine));
                rightMovement.Append(transform.DOMoveX(6, 3).SetRelative()
                    .SetEase(Ease.InOutSine));
                rightMovement.SetLoops(-1, LoopType.Restart);
                break;
            case "Falling":
                _rigidbody = GetComponent<Rigidbody>();
                break;
        }
    }

    public IEnumerator Fall()
    {
        if (_firstTouch)
        {
            _firstTouch = false;
            transform.DOShakePosition(3, 0.1F, 2, 5f, false, true);
            yield return new WaitForSeconds(3);
            _rigidbody.isKinematic = false;
            _rigidbody.useGravity = true;
            yield return new WaitForSeconds(2);
            Destroy(gameObject);
        }
    }
    
    //TODO: - add invisible platform
}