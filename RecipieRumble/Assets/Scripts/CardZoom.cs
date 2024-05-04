using UnityEngine;
using UnityEngine.UI;

public class CardZoom : MonoBehaviour
{
    public GameObject Canvas;
    public GameObject ZoomCard;

    private GameObject zoomCard;
    private Sprite zoomSprite;

    private void Awake()
    {
        Canvas = GameObject.Find("Main Canvas");
        zoomSprite = gameObject.GetComponent<Image>().sprite;
    }

    private void OnHoverEnter()
    {
        if (ZoomCard == null)
        {
            Debug.LogError("Zoom card prefab not set in the inspector.", this);
            return;
        }

        Vector2 cardPosition = new Vector2(transform.position.x, transform.position.y + 100);
        zoomCard = Instantiate(ZoomCard, cardPosition, Quaternion.identity, Canvas.transform);
        zoomCard.GetComponent<Image>().sprite = GetComponent<Image>().sprite;
        RectTransform rectTransform = zoomCard.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(140, 240);
        rectTransform.anchoredPosition = Canvas.transform.InverseTransformPoint(cardPosition);
    }

    private void OnHoverExit()
    {
        if (zoomCard != null)
        {
            Destroy(zoomCard);
        }
    }
}
