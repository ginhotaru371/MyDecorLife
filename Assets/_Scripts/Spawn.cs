using DG.Tweening;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private Transform parent;

    private GameObject _decor;

    private void OnMouseDown()
    {
        _decor = Instantiate(prefab, transform.position, Quaternion.identity, parent);

        var newPos = transform.position;
        newPos.y = 1.0f;
        _decor.transform.DOLocalMove(newPos, 0.5f).SetEase(Ease.OutBack);
    }
}
