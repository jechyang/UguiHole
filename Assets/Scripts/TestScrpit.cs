using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TestScrpit : MonoBehaviour
{
    private HoleMgr _holeMgr;
    public Button AddHoleBtn;
    private List<Image> _images;
    private int _currentIndex = 1;
    private void Awake()
    {
        _holeMgr = transform.Find("HoleMask").GetComponent<HoleMgr>();
        AddHoleBtn.onClick.AddListener(OnClickHole);
    }

    private void OnClickHole()
    {
        _holeMgr.SetHoles(_images.GetRange(0,_currentIndex));
        _holeMgr.RefreshHole();
        _currentIndex += 1;
        if (_currentIndex > _images.Count)
        {
            AddHoleBtn.interactable = false;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _images = transform.Find("GameObject").GetComponentsInChildren<Image>().ToList();
    }
    
}
