using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMovement : MonoBehaviour
{
    public float moveDistance = 3f;
    public float moveDuration = 1.5f;
    void Start()
    {
        StartMovement();
    }

    void StartMovement()
    {
        transform.DOMoveY(transform.position.y + moveDistance, moveDuration)
            .SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo).SetDelay(3f);
    }

}
