using UnityEngine;

public class TexturePainter : MonoBehaviour
{
  private Texture2D _texture;

  public void InitializeTexture(Renderer renderer)
  {
    Texture2D originalTexture = renderer.material.mainTexture as Texture2D;
    _texture = new Texture2D(originalTexture.width, originalTexture.height, TextureFormat.RGBA32, false);
    _texture.SetPixels(originalTexture.GetPixels());
    _texture.Apply();

    renderer.material.mainTexture = _texture;
  }

  public void Paint(Vector2 uv, Brush brush)
  {
    int x = Mathf.FloorToInt(uv.x * _texture.width);
    int y = Mathf.FloorToInt(uv.y * _texture.height);

    int brushSize = brush.Size;
    int brushSizeSquared = brushSize * brushSize;

    for (int i = -brushSize; i <= brushSize; i++)
    {
      for (int j = -brushSize; j <= brushSize; j++)
      {
        if (i * i + j * j > brushSizeSquared)
        {
          continue;
        }

        int px = x + i;
        int py = y + j;
        if (px >= 0 && px < _texture.width && py >= 0 && py < _texture.height)
        {
          _texture.SetPixel(px, py, brush.Color);
        }
      }
    }
    _texture.Apply();
  }

  public Texture2D GetTexture()
  {
    return _texture;
  }

  public void SetTexture (Texture2D texture)
  {
    _texture = texture;
    GetComponent<Renderer>().material.mainTexture = _texture;
  }
}