using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AchievementController : MonoBehaviour
{
    private static AchievementController _instance = null;

    public static AchievementController Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = FindObjectOfType<AchievementController>();
            }

            return _instance;
        }
    }

    [SerializeField] private Transform _popUpTransform;
    [SerializeField] private Text _popUpText;
    [SerializeField] private float _popUpDuration;
    [SerializeField] private List<AchievementData> _achievementList;

    private float _popUpDurationCounter;

    private void Update()
    {
        if(_popUpDurationCounter >0)
        {
            _popUpDurationCounter -= Time.unscaledDeltaTime;

            _popUpTransform.localScale = Vector3.LerpUnclamped(_popUpTransform.localScale, Vector3.one, 0.5f);
        }
        else
        {
            _popUpTransform.localScale = Vector2.LerpUnclamped(_popUpTransform.localScale, Vector3.right, 0.5f);
        }
    }

    public void UnlockAchievement(AchievementType type, string value)
    {
        AchievementData achievement = _achievementList.Find(a => a.Type == type && a.Value == value);

        if (achievement != null && !achievement.IsUnlocked)

        {

            achievement.IsUnlocked = true;

            ShowAchivementPopUp(achievement);

        }
    }

    private void ShowAchivementPopUp(AchievementData achievement)

    {

        _popUpText.text = achievement.Title;

        _popUpDurationCounter = _popUpDuration;

        _popUpTransform.localScale = Vector2.right;

    }


}

[System.Serializable]
public class AchievementData
{

    public string Title;

    public AchievementType Type;

    public string Value;

    public bool IsUnlocked;

}



public enum AchievementType
{

    UnlockResource

}