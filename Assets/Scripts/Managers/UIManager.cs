using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    private Dictionary<string, GameObject> _uiList = new Dictionary<string, GameObject>();
    private void Awake()
    {
        Instance = this;

        InitUIList();
    }

    void InitUIList()   
    {
        int uiCount = transform.childCount;
        for (int i = 0; i < uiCount; i++)
        {
            var tr = transform.GetChild(i);
            _uiList.Add(tr.name, tr.gameObject);
            tr.gameObject.SetActive(false);
        }
    }

    public T OpenUI<T>()
    {
        var obj = _uiList[typeof(T).Name];
        obj.SetActive(true);

        return obj.GetComponent<T>();
    }
}