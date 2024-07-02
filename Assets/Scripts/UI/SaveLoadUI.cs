using UnityEngine;
using UnityEngine.UI;
namespace UI
{
    public class SaveLoadUI : MonoBehaviour
    {
        [SerializeField]
        private Button _saveButton;
        [SerializeField]
        private Button _loadButton;
        [SerializeField]
        private TextureManager _textureManager;

        private void Awake()
        {
          _saveButton.onClick.AddListener(SaveDrawing);
          _loadButton.onClick.AddListener(LoadDrawing);
        }

        private void SaveDrawing()
        {
            SaveLoadManager.Instance.SaveTexture(_textureManager.GetTexture());
        }
        
        private void LoadDrawing()
        {
            Texture2D loadedTexture = SaveLoadManager.Instance.LoadTexture();

            if (loadedTexture != null)
            {
                _textureManager.SetTexture(loadedTexture);
            }
        }
    }
}
