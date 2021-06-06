using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class FlagAnimation : MonoBehaviour
{
    [SerializeField]
    private float scale;
    private Camera _mainCamera;
    [SerializeField] private Transform _flag;
    private void Start()
    {
        _mainCamera = Camera.main;
        transform.DOScale(Vector3.one * scale, 2f).SetEase(Ease.InOutElastic).OnComplete(Animation);
    }

    private void Animation()
    {
        transform.DOLocalMoveY(1f, 1f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutQuad);
    }

    private void Update()
    {
        float distance = Vector3.Distance(_flag.position, _mainCamera.transform.position);
        float expodential = Mathf.Pow(scale, distance);
        expodential = Mathf.Clamp(expodential, 0, 5f);
        _flag.localScale = new Vector3(expodential, expodential, expodential);
    }

    private void OnDestroy()
    {
        transform.DOKill();
    }
}
