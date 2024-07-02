using System.IO;
using UnityEngine;

public class SaveLoadManager : MonoBehaviour
{
    public static SaveLoadManager Instance { get; private set; }
    
    private const string FILE_NAME = "DrawingTexture.png";

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void SaveTexture (Texture2D texture)
    {
        byte [] bytes = texture.EncodeToPNG();
        File.WriteAllBytes(Path.Combine(Application.persistentDataPath,FILE_NAME),bytes);
    }

    public Texture2D LoadTexture()
    {
        var filepath = Path.Combine(Application.persistentDataPath, FILE_NAME);

        if (!File.Exists(filepath))
        {
            return null;
        }

        byte [] bytes = File.ReadAllBytes(filepath);
        Texture2D texture = new Texture2D(2, 2);
        texture.LoadImage(bytes);
        return texture;
    }
}
