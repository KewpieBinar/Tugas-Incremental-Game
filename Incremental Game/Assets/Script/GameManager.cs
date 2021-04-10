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
    public Sprite[] ResourcesSprites;

    public TapText TapTextPrefab;

    public Transform CoinIcon;

    public GameObject ResourcePrefab;
    public Transform ResourceParent;
    public Text AutoCollectInfo;
    public Text GoldInfo;

    private List<ResourceController> _activeResources = new List<ResourceController>();
    private float _collectTimer;

    private List<TapText> _tapTextPool = new List<TapText>();

    private double _totalGold;
    public double TotalGold => _totalGold;

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

        CheckResourceCost();

        CoinIcon.transform.localScale = Vector3.LerpUnclamped(CoinIcon.transform.localScale, Vector3.one * 2f, 0.15f);

        CoinIcon.transform.Rotate(0f, 0f, Time.deltaTime * -100f);
    }

    private void CollectPerSecond()
    {
        double output = 0;
        foreach(ResourceController resource in _activeResources)
        {
            if(resource.IsUnlocked)
            output += resource.GetOutput();
        }

        output *= AutoCollectPercentage;

        AutoCollectInfo.text = $"Auto Collect: {output.ToString("F1")}/second";

        AddGold(output);

    }

    public void AddGold(double value)
    {
        _totalGold += value;
        GoldInfo.text = $"Gold: {_totalGold.ToString("0")}";
    }

    private void AddAllResources()
    {

        bool showResources = true;

        foreach(ResourceConfig config in ResourceConfigs)
        {
            
            GameObject obj = Instantiate(ResourcePrefab.gameObject, ResourceParent, false);
            ResourceController resource = obj.GetComponent<ResourceController>();

            resource.SetConfig(config);

            obj.gameObject.SetActive(showResources);

            if(showResources && !resource.IsUnlocked)
            {
                showResources = false;
            }

            _activeResources.Add(resource);

        }
    }

    public void ShowNextResources()
    {
        foreach(ResourceController resource in _activeResources)
        {
            if(!resource.gameObject.activeSelf)
            {
                resource.gameObject.SetActive(true);
                break;
            }
        }
    }

    private void CheckResourceCost()
    {
        foreach(ResourceController resource in _activeResources)
        {
            bool isBuyable = false;//= TotalGold >= resource.GetUpgradeCost();

            if(resource.IsUnlocked)
            {
                isBuyable = TotalGold >= resource.GetUpgradeCost();
            }
            else
            {
                isBuyable = TotalGold >= resource.GetUnlockCost();
            }
            resource.ResourceImage.sprite = ResourcesSprites[isBuyable ? 1 : 0];
        }
    }

    public void CollectByTap (Vector3 tapPosition, Transform parent)
    {
        double output = 0;

        foreach (ResourceController resource in _activeResources)
        {

            if(resource.IsUnlocked)
                output += resource.GetOutput();

        }



        TapText tapText = GetOrCreateTapText();

        tapText.transform.SetParent(parent, false);

        tapText.transform.position = tapPosition;

        tapText.TextToTap.text = $"+{ output.ToString("0") }";

        tapText.gameObject.SetActive(true);

        CoinIcon.transform.localScale = Vector3.one * 1.75f;



        AddGold(output);
    }

    private TapText GetOrCreateTapText()
    {

        TapText tapText = _tapTextPool.Find(t => !t.gameObject.activeSelf);

        if (tapText == null)

        {

            tapText = Instantiate(TapTextPrefab).GetComponent<TapText>();
            

            _tapTextPool.Add(tapText);

            

        }



        return tapText;

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
