using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class TweenActions : MonoBehaviour
{
    // This class is for basic tween actions that does not change thorought the game
    private void Start()
    {
        switch (gameObject.tag) {
            case "UI":
                if (GetComponent<Image>() != null)
                {
                    GetComponent<Image>().DOColor(new Color(1, 1, 1, 0.7f), 3).SetLoops(-1, LoopType.Yoyo);
                }
                break;
        }
    }
}
