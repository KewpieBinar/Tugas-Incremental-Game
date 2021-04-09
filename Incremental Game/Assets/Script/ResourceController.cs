using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceController : MonoBehaviour
{
    public Text ResourceDescription;
    public Text ResourceUnlockCost;
    public Text ResourceUpgradeCost;

    private ResourceConfig _config;

    private int _level = 1;
    
    public void SetConfig(ResourceConfig config)
    {
        _config = config;

        ResourceDescription.text = $"{_config.Name}Lv.{_level}\n+{GetOutput().ToString("0")}";
        ResourceUnlockCost.text = $"Unlock Cost\n{_config.UnlockCost}";
        ResourceUpgradeCost.text = $"Upgrade Cost\n{GetUpgradeCost()}";

    }

    public double GetOutput()
    {
        return _config.Output * _level;
    }

    public double GetUpgradeCost()
    {
        return _config.UpgradeCost * _level;
    }
    
    public double GetUnlockCost()
    {
        return _config.UnlockCost;
    }

    private void OnMouseDown()
    {
        Debug.Log("mouseDown");
    }
}
