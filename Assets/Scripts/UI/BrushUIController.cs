using UnityEngine;
using UnityEngine.UI;

public class BrushUIController : MonoBehaviour
{
    private const float MIN_SIZE = 1f;
    private const float MAX_SIZE = 20f;
    
    [SerializeField] private DrawingController _drawingController;
    [SerializeField] private Slider _sizeSlider;
    [SerializeField] private Image _colorImage;
    
    private Color _selectedColor = Color.green;
    
    private void Start()
    {
        _sizeSlider.value = 0.5f;
        _sizeSlider.onValueChanged.AddListener(OnSizeSliderChanged);
        UpdateBrushSize();
    }

    public void OnColorButtonClicked()
    {
        _selectedColor = Color.green;
        _colorImage.color = _selectedColor;
    }

    private void OnSizeSliderChanged(float value)
    {
        UpdateBrushSize();
    }

    private void UpdateBrushSize()
    {
        float normalizedValue = _sizeSlider.value;
        float size = Mathf.Lerp(MIN_SIZE, MAX_SIZE, normalizedValue);
        _drawingController.SetBrushProperties(_selectedColor, Mathf.RoundToInt(size));
    }
}
