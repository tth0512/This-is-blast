using DG.Tweening;
using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float shootDuration = 0.5f;

    public void FireTo(Vector3 targetWorldPos, Sequence seq, Action onHit = null)
    {
        seq.Append(transform.DOMove(targetWorldPos, shootDuration).SetEase(Ease.OutQuad).OnComplete(() =>
        {
            onHit?.Invoke();
            BlockPool.Instance.ReturnBullet(this);
        }));
    }
}
