using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TapArea : MonoBehaviour, IPointerDownHandler
{
    public void OnPointerDown (PointerEventData evenData)
    {
        GameManager.Instance.CollectByTap(evenData.position, transform);
    }
}
