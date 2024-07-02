using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
   public class ColorPickerController : MonoBehaviour
   {
      public static ColorPickerController Instance { get; private set; }
      
      public event Action ColorConfirmed;
      
      private const int MAX_INPUT_LENGTH = 6;
      
      public Color SelectedColor { get; private set; }
      
      [SerializeField]
      private RawImage _hueImage,
         _satValImage,
         _outputImage;
      [SerializeField]
      private Slider _hueSlider;
      [SerializeField]
      private TMP_InputField _hexInputField;
      [SerializeField]
      private RectTransform _colorPickerUI;
      [SerializeField]
      private Button _confirmButton;

      private ColorPalette _colorPalette;
      private Texture2D _hueTexture,
         _svTexture,
         _outputTexture;
      private float _currentHue,
         _currentSat,
         _currentValue;
      
      
      private void Awake()
      {
         if (Instance != null)
         {
            Destroy(gameObject);
            return;
         }

         Instance = this;
         
         _colorPalette = new ColorPalette();
         
         _hueSlider.onValueChanged.AddListener(UpdateSVImage);
         _hexInputField.onEndEdit.AddListener(OnTextInput);
         _hexInputField.onValueChanged.AddListener(LimitHexInputLength);
         _confirmButton.onClick.AddListener(OnConfirmButtonClicked);
      }

      private void Start()
      {
         _hueTexture = _colorPalette.CreateHueTexture(16);
         _svTexture = _colorPalette.CreateSvTexture(16,16,_currentHue);
         _outputTexture = _colorPalette.CreateOutputTexture(16, SelectedColor);
         
         _hueImage.texture = _hueTexture;
         _satValImage.texture = _svTexture;
         _outputImage.texture = _outputTexture;
      }

      private void UpdateOutputColorImage()
      {
         Color currentColor = Color.HSVToRGB(_currentHue, _currentSat, _currentValue);
         _colorPalette.UpdateSVTexture(_svTexture,_currentHue);
      
         for (int i = 0; i < _outputTexture.height; i++)
         {
            _outputTexture.SetPixel(0,i, currentColor);
         }

         _outputTexture.Apply();
         _outputImage.texture = _outputTexture;
         _hexInputField.text = ColorUtility.ToHtmlStringRGB(currentColor);

         SelectedColor = currentColor;
      }

      public void SetSV (float saturation, float value)
      {
         _currentSat = saturation;
         _currentValue = value;
      
         UpdateOutputColorImage();
      }

      private void UpdateSVImage(float arg)
      {
         _currentHue = _hueSlider.value;

        _colorPalette.UpdateSVTexture(_svTexture, _currentHue);
        UpdateOutputColorImage();
      }

      private void LimitHexInputLength (string input)
      {
         if (_hexInputField.text.Length > MAX_INPUT_LENGTH)
         {
            _hexInputField.text = _hexInputField.text.Substring(0, MAX_INPUT_LENGTH);
         }
      }
      
      private void OnTextInput(string arg0)
      {
         if (ColorUtility.TryParseHtmlString("#" + _hexInputField.text, out Color newColor))
         {
            Color.RGBToHSV(newColor,out _currentHue,out _currentSat, out _currentValue);
         }

         _hueSlider.value = _currentHue;
         _hexInputField.text = "";
         UpdateOutputColorImage();
      }

      public void OpenColorPalette()
      {
         _colorPickerUI.gameObject.SetActive(true);
         UpdateUIWithBrushColor();
      }

      private void CloseColorPalette()
      {
         _colorPickerUI.gameObject.SetActive(false);
      }

      private void UpdateUIWithBrushColor()
      {
         Color.RGBToHSV(SelectedColor, out _currentHue, out _currentSat, out _currentValue);
         _hueSlider.value = _currentHue;
         UpdateSVImage(_currentHue);
      }
      
      private void OnConfirmButtonClicked()
      {
         SelectedColor = Color.HSVToRGB(_currentHue, _currentSat, _currentValue);
         ColorConfirmed?.Invoke();
         CloseColorPalette();
      }
   }
}
