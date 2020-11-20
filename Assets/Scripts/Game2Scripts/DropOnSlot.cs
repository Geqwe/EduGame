using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DropOnSlot : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData) {
        if(eventData.pointerDrag!=null) {
            Debug.Log("Drops "+eventData.pointerDrag+" and I am "+gameObject.name);
            if(eventData.pointerDrag.name.StartsWith(gameObject.name)) {
                eventData.pointerDrag.GetComponent<DragDrop>().right = true;
                eventData.pointerDrag.SetActive(false);
                Color temp = GetComponent<Image>().color;
                temp.a=1f;
                temp = Color.green;
                GetComponent<Image>().color = temp;
                SecondGameManager.instance.Right();
            }
        }

        // if(eventData.pointerDrag!=null) {
        //     Debug.Log("Drops "+eventData.pointerDrag);
        //     eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
        //     Color temp = GetComponent<Image>().color;
        //     temp.a=1f;
        //     GetComponent<Image>().color = temp;
        //     Debug.Log("alpha "+GetComponent<Image>().color.a);
        // }
    }
}
