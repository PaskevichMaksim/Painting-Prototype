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
        [SerializeField]
        private Button _undoButton;
        [SerializeField]
        private Button _clearButton;

        private void Start()
        {
            _sizeSlider.value = 0.5f;
            
            _sizeSlider.onValueChanged.AddListener(OnSizeSliderChanged);
            _selectButton.onClick.AddListener(OnSelectButtonClick);
            _undoButton.onClick.AddListener(OnUndoButtonClick);
            _clearButton.onClick.AddListener(OnClearButtonClick);
            
            ColorPickerController.Instance.ColorConfirmed += OnColorConfirmed;
            UpdateBrushSize();
        }

        private void OnDestroy()
        {
            _sizeSlider.onValueChanged.RemoveListener(OnSizeSliderChanged);
            _selectButton.onClick.RemoveListener(OnSelectButtonClick);
            _undoButton.onClick.RemoveListener(OnUndoButtonClick);
            _clearButton.onClick.RemoveListener(OnClearButtonClick);
            
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

        private void OnUndoButtonClick()
        {
            DrawingController.Instance.UndoLastAction();
        }

        private void OnClearButtonClick()
        {
            DrawingController.Instance.ClearDrawing();
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
