using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
  public class SvImageController : MonoBehaviour, IDragHandler, IPointerClickHandler
  {
    [SerializeField]
    private Image _pickerImage;

    private RectTransform _rectTransform, _pickerTransform;

    private void Awake()
    {
      _rectTransform = GetComponent<RectTransform>();
      _pickerTransform = _pickerImage.GetComponent<RectTransform>();
    }

    private void UpdateColor(PointerEventData eventData)
    {
      RectTransformUtility.ScreenPointToLocalPointInRectangle(_rectTransform, eventData.position, eventData.pressEventCamera, out Vector2 localPoint);

      float deltaX = _rectTransform.sizeDelta.x * 0.5f;
      float deltaY = _rectTransform.sizeDelta.y * 0.5f;

      localPoint.x = Mathf.Clamp(localPoint.x, -deltaX, deltaX);
      localPoint.y = Mathf.Clamp(localPoint.y, -deltaY, deltaY);

      _pickerTransform.localPosition = localPoint;
      float xNormalized = (localPoint.x + deltaX) / _rectTransform.sizeDelta.x;
      float yNormalized = (localPoint.y + deltaY) / _rectTransform.sizeDelta.y;

      _pickerImage.color = Color.HSVToRGB(0, 0, 1 - yNormalized);
      ColorPickerController.Instance.SetSV(xNormalized, yNormalized);
    }

    public void OnDrag(PointerEventData eventData)
    {
      UpdateColor(eventData);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
      UpdateColor(eventData);
    }
  }
}