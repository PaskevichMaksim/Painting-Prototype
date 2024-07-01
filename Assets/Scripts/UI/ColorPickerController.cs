using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
namespace UI
{
   public class ColorPickerController : MonoBehaviour
   {
      [SerializeField]
      private RawImage _hueImage,
         _satValImage,
         _outputImage;
      [SerializeField]
      private Slider _hueSlider;
      [SerializeField]
      private TMP_InputField _hexInputField;

      private Texture2D _hueTexture,
         _svTexture,
         _outputTexture;
      private float _currentHue,
         _currentSat,
         _currentValue;

      public float CurrentHue => _currentHue;
      public float CurrentSat => _currentSat;
      public float CurrentValue => _currentValue;
   
      [SerializeField]
      private MeshRenderer _objectToChange;
      private static readonly int baseColor = Shader.PropertyToID("_BaseColor");

      private void Awake()
      {
         _hueSlider.onValueChanged.AddListener(UpdateSVImage);
         _hexInputField.onEndEdit.AddListener(OnTextInput);
      }

      private void Start()
      {
         CreateHueImage();
         CreateSvImage();
         CreateOutputImage();
         UpdateOutputColorImage();
      }

      private void CreateHueImage()
      {
         _hueTexture = new Texture2D(1, 16)
         {
            wrapMode = TextureWrapMode.Clamp,
            name = "HueTexture"
         };

         for (int i = 0; i < _hueTexture.height; i++)
         {
            _hueTexture.SetPixel(0,i, Color.HSVToRGB((float)i / _hueTexture.height, 1, 0.05f));
         }

         _hueTexture.Apply();
         _currentHue = 0;

         _hueImage.texture = _hueTexture;
      }
   
      private void CreateSvImage()
      {
         _svTexture = new Texture2D(16, 16)
         {
            wrapMode = TextureWrapMode.Clamp,
            name = "SatValTexture"
         };

         for (int i = 0; i < _svTexture.height; i++)
         {
            for (int j = 0; j < _svTexture.width; j++)
            {
               _svTexture.SetPixel(i,j,Color.HSVToRGB(_currentHue, (float)i / _svTexture.width, (float)j / _svTexture.height));
            }
         }

         _svTexture.Apply();
         _currentSat = 0;
         _currentValue = 0;

         _satValImage.texture = _svTexture;
      }
   
      private void CreateOutputImage()
      {
         _outputTexture = new Texture2D(1, 16)
         {
            wrapMode = TextureWrapMode.Clamp,
            name = "OutputTexture"
         };

         Color currentColor = Color.HSVToRGB(_currentHue, _currentSat, _currentValue);

         for (int i = 0; i < _outputTexture.height; i++)
         {
            _outputTexture.SetPixel(0,i, currentColor);
         }

         _outputTexture.Apply();

         _outputImage.texture = _hueTexture;
      }

      private void UpdateOutputColorImage()
      {
         Color currentColor = Color.HSVToRGB(_currentHue, _currentSat, _currentValue);
      
         for (int i = 0; i < _outputTexture.height; i++)
         {
            _outputTexture.SetPixel(0,i, currentColor);
         }

         _outputTexture.Apply();

         _hexInputField.text = ColorUtility.ToHtmlStringRGB(currentColor);
         _objectToChange.material.SetColor(baseColor, currentColor);
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

         for (int i = 0; i < _svTexture.height; i++)
         {
            for (int j = 0; j < _svTexture.width; j++)
            {
               _svTexture.SetPixel(i,j,Color.HSVToRGB(_currentHue, (float)i / _svTexture.width, (float)j / _svTexture.height));
            }
         }

         _svTexture.Apply();
      
         UpdateOutputColorImage();
      }

      private void OnTextInput(string arg0)
      {
         const int MAX_LENGTH = 6;
         
         if (_hexInputField.text.Length < MAX_LENGTH)
         {
            return;
         }

         if (ColorUtility.TryParseHtmlString("#" + _hexInputField.text, out Color newColor))
         {
            Color.RGBToHSV(newColor,out _currentHue,out _currentSat, out _currentValue);
         }

         _hueSlider.value = _currentHue;
         _hexInputField.text = "";
         UpdateOutputColorImage();
      }
   }
}
