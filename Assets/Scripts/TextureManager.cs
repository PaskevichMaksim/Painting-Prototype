using UnityEngine;

public class TextureManager : MonoBehaviour
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

  public Texture2D GetTexture()
  {
    return _texture;
  }

  public void SetTexture(Texture2D texture)
  {
    _texture = texture;
    GetComponent<Renderer>().material.mainTexture = _texture;
  }
}