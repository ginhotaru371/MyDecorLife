using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObject : Singleton<SpawnObject>
{
    [SerializeField] private GameObject prefab;

    private GameObject _decor;

    public List<GameObject> list;

    public override void Awake()
    {
        base.KeepAlive(false);
        base.Awake();
    }

    private void OnMouseDown()
    {
        _decor = Instantiate(prefab, transform.position, Quaternion.identity);
        _decor.tag = "wardrobe_1";
        _decor.SetActive(false);
        list.Add(_decor);
    }
}
