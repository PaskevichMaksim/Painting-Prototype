using UnityEngine;

public class TexturePainter : MonoBehaviour
{
  private TextureManager _textureManager;

  private void Awake()
  {
    _textureManager = GetComponent<TextureManager>();
  }

  public void Paint(Vector2 uv, Brush brush)
  {
    Texture2D texture = _textureManager.GetTexture();

    int x = Mathf.FloorToInt(uv.x * texture.width);
    int y = Mathf.FloorToInt(uv.y * texture.height);

    int brushSize = brush.Size;
    int brushSizeSquared = brushSize * brushSize;

    for (int i = -brushSize; i <= brushSize; i++)
    {
      for (int j = -brushSize; j <= brushSize; j++)
      {
        if (i * i + j * j <= brushSizeSquared)
        {
          int px = x + i;
          int py = y + j;
          if (px >= 0 && px < texture.width && py >= 0 && py < texture.height)
          {
            texture.SetPixel(px, py, brush.Color);
          }
        }
      }
    }
    texture.Apply();
  }
}