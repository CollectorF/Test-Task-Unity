using DG.Tweening;
using UnityEngine;

public class TutorialUI : MonoBehaviour
{
    [SerializeField]
    private RectTransform target;
    [SerializeField]
    private float rotateSpeed;
    [SerializeField]
    private float scaleSpeed;

    public void Start()
    {
        if (target != null)
        {
        Sequence sequence = DOTween.Sequence()
            .Append(target.transform.DOLocalRotate(new Vector3(0, 0, 360), rotateSpeed, RotateMode.FastBeyond360).SetRelative())
            .Join(target.transform.DOScale(new Vector3(1.5f, 1.5f, 1.5f), scaleSpeed));
        sequence.SetLoops(-1, LoopType.Restart);
        }
    }
}
