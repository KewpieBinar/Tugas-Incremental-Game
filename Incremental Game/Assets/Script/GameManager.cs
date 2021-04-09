using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance = null;

    public static GameManager Instance
    {
        get 
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameManager>();
            }
                    
            return _instance;
                
        }
    }

    [Range(0f, 1f)]
    public float AutoCollectPercentage = 0.1f;
    public ResourceConfig[] ResourceConfigs;

    public GameObject ResourcePrefab;
    public Transform ResourceParent;
    public Text AutoCollectInfo;
    public Text GoldInfo;

    private List<ResourceController> _activeResources = new List<ResourceController>();
    private float _collectTimer;

    private double _totalGold;

    private void Start()
    {
        AddAllResources();
    }

    private void Update()
    {
        _collectTimer += Time.unscaledDeltaTime;
        if(_collectTimer >= 1f)
        {
            CollectPerSecond();
            _collectTimer = 0;
        }
    }

    private void CollectPerSecond()
    {
        double output = 0;
        foreach(ResourceController resource in _activeResources)
        {
            output += resource.GetOutput();
        }

        output *= AutoCollectPercentage;

        AutoCollectInfo.text = $"Auto Collect: {output.ToString("F1")}/second";

        AddGold(output);

    }

    private void AddGold(double value)
    {
        _totalGold += value;
        GoldInfo.text = $"Gold: {_totalGold.ToString("0")}";
    }

    private void AddAllResources()
    {
        foreach(ResourceConfig config in ResourceConfigs)
        {
            GameObject obj = Instantiate(ResourcePrefab.gameObject, ResourceParent, false);
            ResourceController resource = obj.GetComponent<ResourceController>();

            resource.SetConfig(config);
            _activeResources.Add(resource);

        }
    }
}

[System.Serializable]
public struct ResourceConfig
{
    public string Name;
    public double UnlockCost;
    public double UpgradeCost;
    public double Output;
}
