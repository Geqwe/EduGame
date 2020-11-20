using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DragDrop : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
{
    private RectTransform rectTransform;
    [SerializeField] private Canvas canvas;
    [SerializeField] private GameObject panel;
    private CanvasGroup canvasGroup;
    private Image img;
    public bool right = false;

    private void Awake() {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        img = GetComponent<Image>();
    }
    public void OnBeginDrag(PointerEventData eventData) {
        panel.GetComponent<HorizontalLayoutGroup>().enabled = false;
        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData) {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData) {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
        if(!right) {
            img.color = Color.red;
            SecondGameManager.instance.Wrong();
        }
    }

    public void OnPointerDown(PointerEventData eventData) {

    }

    public void OnDrop(PointerEventData eventData) {
        Debug.Log("Dropped on "+eventData.pointerDrag);
    }


}
