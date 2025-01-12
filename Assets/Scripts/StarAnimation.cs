using UnityEngine;
using DG.Tweening;

public class StarAnimation : MonoBehaviour
{
    [SerializeField] private float _scaleDuration = 0.4f;
    [SerializeField] private float _rotateDuration = 0.6f;
    [SerializeField] private float _pulseDuration = 0.8f;
    [SerializeField] private float _pulseScale = 1.1f;
    private Vector3 _startScale;
    private Quaternion _startRotation;
    private Sequence _pulseSequence;

    private void Awake()
    {
        _startScale = transform.localScale;
        _startRotation = transform.localRotation;

    }
    private void OnEnable()
    {
        // Reset scale and rotation
        transform.localScale = Vector3.zero;
        transform.localRotation = _startRotation;
        // Animate the scale
        transform.DOScale(_startScale, _scaleDuration).SetEase(Ease.OutBack);
        //Animate rotation
        transform.DOLocalRotate(transform.localRotation.eulerAngles + new Vector3(0, 0, -360), _rotateDuration, RotateMode.FastBeyond360).SetEase(Ease.OutCubic).OnComplete(StartPulsing);
    }
    private void OnDisable()
    {
        StopPulsing();
    }
    private void StartPulsing()
    {
        _pulseSequence = DOTween.Sequence();
        _pulseSequence.Append(transform.DOScale(_startScale * _pulseScale, _pulseDuration / 2).SetEase(Ease.OutQuad));
        _pulseSequence.Append(transform.DOScale(_startScale, _pulseDuration / 2).SetEase(Ease.OutQuad));
        _pulseSequence.SetLoops(-1);
    }
    private void StopPulsing()
    {
        _pulseSequence?.Kill();
    }
}