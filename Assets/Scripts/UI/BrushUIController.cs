using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class BrushUIController : MonoBehaviour
    {
        private const float MIN_SIZE = 1f;
        private const float MAX_SIZE = 20f;
    
        [SerializeField]
        private DrawingController _drawingController;
        [SerializeField]
        private ColorPickerController _colorPickerController;
        [SerializeField]
        private Slider _sizeSlider;
        [SerializeField]
        private Button _selectButton;

        private void Start()
        {
            _sizeSlider.value = 0.5f;
            _sizeSlider.onValueChanged.AddListener(OnSizeSliderChanged);
            _selectButton.onClick.AddListener(OnSelectButtonClick);
            _colorPickerController.ColorConfirmed += OnColorConfirmed;
            UpdateBrushSize();
        }

        private void OnDestroy()
        {
            _colorPickerController.ColorConfirmed -= OnColorConfirmed;
        }
    
        private void OnSelectButtonClick()
        {
            _selectButton.gameObject.SetActive(false);
            _colorPickerController.OpenColorPalette();
        }
    

        private void OnSizeSliderChanged(float value)
        {
            UpdateBrushSize();
        }

        private void OnColorConfirmed()
        {
            _drawingController.SetBrushColor(_colorPickerController.SelectedColor);
            _selectButton.gameObject.SetActive(true);
        }
    
        private void UpdateBrushSize()
        {
            float normalizedValue = _sizeSlider.value;
            float size = Mathf.Lerp(MIN_SIZE, MAX_SIZE, normalizedValue);
            _drawingController.SetBrushSize(Mathf.RoundToInt(size));
        }
    }
}
