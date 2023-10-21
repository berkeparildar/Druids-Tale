using System.Collections;
using UnityEngine;
using DG.Tweening;

public class SpecialPlatforms : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private bool _firstTouch = true;
    private BoxCollider _boxCollider;
    private MeshRenderer _meshRenderer;
    private Sequence leftMovement;
    private Sequence rightMovement;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();

        switch (gameObject.tag)
        {
            case "MovingLeft":
                leftMovement = DOTween.Sequence();
                leftMovement.Append(transform.DOMoveX(10, 4).SetRelative()
                    .SetEase(Ease.InOutSine));
                leftMovement.Append(transform.DOMoveX(-10, 4).SetRelative()
                    .SetEase(Ease.InOutSine));
                leftMovement.SetLoops(-1, LoopType.Restart);
                break;
            case "MovingRight":
                rightMovement = DOTween.Sequence();
                rightMovement.Append(transform.DOMoveX(-10, 4).SetRelative()
                        .SetEase(Ease.InOutSine));
                    rightMovement.Append(transform.DOMoveX(10, 4).SetRelative()
                        .SetEase(Ease.InOutSine));
                    rightMovement.SetLoops(-1, LoopType.Restart);
                break;
            case "Falling":
                _rigidbody = GetComponent<Rigidbody>();
                break;
            case "Invisible":
                _boxCollider = GetComponent<BoxCollider>();
                _meshRenderer = GetComponent<MeshRenderer>();
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

    public IEnumerator TurnVisible()
    {
        _boxCollider.enabled = true;
        var currentColor = _meshRenderer.material.color;
        var newColor = new Color(currentColor.r, currentColor.g, currentColor.b, 0.18f);
        _meshRenderer.material.DOColor(newColor, 0.5f);
        yield return new WaitForSeconds(10);
        _meshRenderer.material.DOColor(currentColor, 0.5f);
    }
}