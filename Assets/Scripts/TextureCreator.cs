using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class TextureCreator : MonoBehaviour
{
  [SerializeField]
  private int _textureWidth = 512;
  [SerializeField]
  private int _textureHeight = 512;
  [SerializeField]
  private Color _initialColor = Color.white;

  private Renderer _objectRenderer;

  private void Start()
  {
    _objectRenderer = GetComponent<Renderer>();

    if (_objectRenderer.material.mainTexture == null)
    {
      CreateAndAssignTexture();
    }
  }

  private void CreateAndAssignTexture()
  {
    Texture2D newTexture = new Texture2D(_textureWidth, _textureHeight, TextureFormat.RGBA32, false);
    Color[] colorArray = new Color[_textureWidth * _textureHeight];

    for (int i = 0; i < colorArray.Length; i++)
    {
      colorArray[i] = _initialColor;
    }

    newTexture.SetPixels(colorArray);
    newTexture.Apply();

    _objectRenderer.material.mainTexture = newTexture;
  }
}