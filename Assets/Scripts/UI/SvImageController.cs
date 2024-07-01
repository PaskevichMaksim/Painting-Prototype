using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
  public class SvImageController : MonoBehaviour,IDragHandler,IPointerClickHandler
  {
    [SerializeField]
    private Image _pickerImage;
    [SerializeField]
    private ColorPickerController _colorPickerController;

    private RawImage _svImage;
    private RectTransform _rectTransform,
      _pickerTransform;

    private void Awake()
    {
      _svImage = GetComponent<RawImage>();
      _rectTransform = GetComponent<RectTransform>();

      _pickerTransform = _pickerImage.GetComponent<RectTransform>();
      _pickerTransform.position = new Vector2(-(_rectTransform.sizeDelta.x *.5f), -(_rectTransform.sizeDelta.y *.5f));
    }

    private void UpdateColor (PointerEventData eventData)
    {
      Vector3 position = _rectTransform.InverseTransformPoint(eventData.position);
      float deltaX = _rectTransform.sizeDelta.x * .5f;
      float deltaY = _rectTransform.sizeDelta.y * .5f;

      if (position.x < -deltaX)
      {
        position.x = -deltaX;
      } else if (position.x > deltaX)
      {
        position.x = deltaX;
      }
      
      if (position.y < -deltaY)
      {
        position.y = -deltaY;
      } else if (position.y > deltaY)
      {
        position.y = deltaY;
      }

      float x = position.x + deltaX;
      float y = position.y + deltaY;

      float xNormalized = x / _rectTransform.sizeDelta.x;
      float yNormalized = y / _rectTransform.sizeDelta.y;

      _pickerTransform.localPosition = position;
      _pickerImage.color = Color.HSVToRGB(0, 0, 1 - yNormalized);
      
      _colorPickerController.SetSV(xNormalized, yNormalized);

    }

    public void OnDrag (PointerEventData eventData)
    {
      UpdateColor(eventData);
    }

    public void OnPointerClick (PointerEventData eventData)
    {
      UpdateColor(eventData);
    }
  }
}
