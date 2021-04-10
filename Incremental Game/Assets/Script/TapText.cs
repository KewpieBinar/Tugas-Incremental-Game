using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TapText : MonoBehaviour
{
    public float SpawnTime = 0.5f;
    public Text TextToTap;

    private float _spawnTime;

    private void OnEnable()
    {
        _spawnTime = SpawnTime;
    }

    private void Update()
    {
        _spawnTime -= Time.unscaledDeltaTime;
        if(_spawnTime <= 0f)
        {
            
            gameObject.SetActive(false);
        }
        else
        {
            transform.position = transform.position + new Vector3(0, 1, 0);
            gameObject.SetActive(true);
        }
    }
}
