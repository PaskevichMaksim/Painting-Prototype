using UnityEngine;

namespace UI
{
  public class ColorPalette
  {
    public Texture2D CreateHueTexture(int height)
    {
      Texture2D hueTexture = new Texture2D(1, height)
      {
        wrapMode = TextureWrapMode.Clamp,
        name = "HueTexture"
      };

      for (int i = 0; i < hueTexture.height; i++)
      {
        hueTexture.SetPixel(0, i, Color.HSVToRGB((float)i / hueTexture.height, 1f, 1f));
      }

      hueTexture.Apply();
      return hueTexture;
    }

    public Texture2D CreateSvTexture(int width, int height, float hue)
    {
      Texture2D svTexture = new Texture2D(width, height)
      {
        wrapMode = TextureWrapMode.Clamp,
        name = "SatValTexture"
      };

      UpdateSVTexture(svTexture, hue);
      return svTexture;
    }

    public void UpdateSVTexture(Texture2D svTexture, float hue)
    {
      for (int i = 0; i < svTexture.height; i++)
      {
        for (int j = 0; j < svTexture.width; j++)
        {
          svTexture.SetPixel(j, i, Color.HSVToRGB(hue, (float)j / svTexture.width, (float)i / svTexture.height));
        }
      }

      svTexture.Apply();
    }

    public Texture2D CreateOutputTexture(int height, Color color)
    {
      Texture2D outputTexture = new Texture2D(1, height)
      {
        wrapMode = TextureWrapMode.Clamp,
        name = "OutputTexture"
      };

      for (int i = 0; i < outputTexture.height; i++)
      {
        outputTexture.SetPixel(0, i, color);
      }

      outputTexture.Apply();
      return outputTexture;
    }
  }
}