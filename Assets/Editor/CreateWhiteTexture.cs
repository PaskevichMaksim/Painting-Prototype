using UnityEngine;
using UnityEditor;
using System.IO;

public class CreateWhiteTexture : MonoBehaviour
{
  [MenuItem("Tools/Create White Texture")]
  private static void CreateTexture()
  {
    const int WIDTH = 512;
    const int HEIGHT = 512;
    Color color = Color.white;

    Texture2D texture = new Texture2D(WIDTH, HEIGHT, TextureFormat.RGBA32, false);

    Color[] pixels = texture.GetPixels();
    for (int i = 0; i < pixels.Length; i++)
    {
      pixels[i] = color;
    }
    texture.SetPixels(pixels);
    texture.Apply();

    byte[] bytes = texture.EncodeToPNG();

    const string BASE_RELATIVE_PATH = "Assets/Textures/WhiteTexture.png";
    string baseFullPath = Path.Combine(Application.dataPath, "../", BASE_RELATIVE_PATH);

    string fullPath = baseFullPath;
    int count = 1;
    while (File.Exists(fullPath))
    {
      string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(BASE_RELATIVE_PATH);
      string extension = Path.GetExtension(BASE_RELATIVE_PATH);
      string numberedFileName = $"{fileNameWithoutExtension}_{count}{extension}";
      fullPath = Path.Combine(Path.GetDirectoryName(baseFullPath) ?? string.Empty, numberedFileName);
      count++;
    }

    Directory.CreateDirectory(Path.GetDirectoryName(fullPath) ?? string.Empty);

    File.WriteAllBytes(fullPath, bytes);

    AssetDatabase.Refresh();
  }
}