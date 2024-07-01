using UnityEngine;
using UnityEngine.UI;

public class BrushUIController : MonoBehaviour
{
    [SerializeField] private DrawingController _drawingController;
    [SerializeField] private Slider _sizeSlider;
    [SerializeField] private Image _colorImage;
    
    private Color _selectedColor = Color.green;
    private const float MinSize = 1f;
    private const float MaxSize = 20f;
    
    private void Start()
    {
        _sizeSlider.onValueChanged.AddListener(OnSizeSliderChanged);
    }

    public void OnColorButtonClicked()
    {
        _selectedColor = Color.green;
        _colorImage.color = _selectedColor;
    }

    private void OnSizeSliderChanged(float value)
    {
        float normalizedValue = _sizeSlider.value;
        float size = Mathf.Lerp(MinSize, MaxSize, normalizedValue);
        _drawingController.SetBrushProperties(_selectedColor, Mathf.RoundToInt(size));
    }
}
