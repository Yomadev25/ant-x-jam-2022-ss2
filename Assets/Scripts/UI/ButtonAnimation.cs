using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonAnimation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private bool isShake;

    public void OnPointerEnter(PointerEventData eventData)
    {
        this.transform.LeanScale(new Vector3(1.1f, 1.1f, 1.1f), 0.1f).setEaseInSine();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        this.transform.LeanScale(Vector3.one, 0.1f).setEaseOutSine();
    }
}
