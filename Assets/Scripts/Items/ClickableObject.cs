using UnityEngine;
using UnityEngine.EventSystems;

public class ClickableObject : MonoBehaviour,
    IPointerClickHandler, IPointerDownHandler, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public InventorySlot slot;
    public RectTransform icon;

    Canvas canvas;
    CanvasGroup canvasGroup;

    void Start()
    {
        canvas = GameObject.FindWithTag("Canvas").GetComponent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left && Input.GetKey(KeyCode.LeftShift))
        {
            //Debug.Log("Shift + left click");
            slot.DropItem();
        }
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            //Debug.Log("left click");
            slot.UseItem();
        }
        else if (eventData.button == PointerEventData.InputButton.Middle)
            Debug.Log("Middle click");
        else if (eventData.button == PointerEventData.InputButton.Right)
            Debug.Log("Right click");
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            //Debug.Log("left click down");
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        //Debug.Log("OnDrag");
        icon.anchoredPosition += eventData.delta / canvas.scaleFactor;
        //icon.anchoredPosition += eventData.position;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("OnBeginDrag");
        canvasGroup.alpha = 0.7f;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("OnEndDrag");
        icon.anchoredPosition = Vector2.zero;
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
    }
}
