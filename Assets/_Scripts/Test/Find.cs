using System;
using System.Collections.Generic;
using UnityEngine;

public class Find : MonoBehaviour
{
    public List<int> list;

    private void Start()
    {
        list.Add(1);
        list.Add(2);
        list.Add(3);
        list.Add(4);
        list.Add(5);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log(list[0]);
            list.RemoveAt(0);
        }
    }
}
