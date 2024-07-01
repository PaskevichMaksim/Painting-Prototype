using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class BrushUIController : MonoBehaviour
    {
        private const float MIN_SIZE = 1f;
        private const float MAX_SIZE = 20f;
        
        [SerializeField]
        private Slider _sizeSlider;
        [SerializeField]
        private Button _selectButton;

        private void Start()
        {
            _sizeSlider.value = 0.5f;
            _sizeSlider.onValueChanged.AddListener(OnSizeSliderChanged);
            _selectButton.onClick.AddListener(OnSelectButtonClick);
            ColorPickerController.Instance.ColorConfirmed += OnColorConfirmed;
            UpdateBrushSize();
        }

        private void OnDestroy()
        {
            ColorPickerController.Instance.ColorConfirmed -= OnColorConfirmed;
        }
    
        private void OnSelectButtonClick()
        {
            _selectButton.gameObject.SetActive(false);
            ColorPickerController.Instance.OpenColorPalette();
        }
    

        private void OnSizeSliderChanged(float value)
        {
            UpdateBrushSize();
        }

        private void OnColorConfirmed()
        {
            DrawingController.Instance.SetBrushColor(ColorPickerController.Instance.SelectedColor);
            _selectButton.gameObject.SetActive(true);
        }
    
        private void UpdateBrushSize()
        {
            float normalizedValue = _sizeSlider.value;
            float size = Mathf.Lerp(MIN_SIZE, MAX_SIZE, normalizedValue);
            DrawingController.Instance.SetBrushSize(Mathf.RoundToInt(size));
        }
    }
}
