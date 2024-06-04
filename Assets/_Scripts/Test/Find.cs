using System;
using UnityEngine;

public class Find : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            GameObject g = SpawnObject.instance.list.Find(x => x.CompareTag("wardrobe_1") && !x.activeSelf);
            g.SetActive(true);
        }
    }
}
