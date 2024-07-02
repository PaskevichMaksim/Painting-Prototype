using System.Collections.Generic;
using UnityEngine;

public class DrawingController : MonoBehaviour
{
    public static DrawingController Instance { get; private set; }

    private const int MAX_UNDO_STEPS = 5;
    private readonly Stack<Color[]> _undoStack = new Stack<Color[]>(MAX_UNDO_STEPS);
    
    [SerializeField]
    private Camera _mainCamera;
    
    private readonly Brush _brush = new Brush(Color.cyan, 10);
    private TexturePainter _currentPainter;
    private TextureManager _currentTextureManager;
    private Texture2D _drawingTexture;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        _drawingTexture = new Texture2D(512, 512);
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            HandleDrawing(Input.mousePosition);
        } else if(Input.touchCount == 1)
        {
            HandleTouchDrawing();
        }
    }
    
    private void HandleTouchDrawing()
    {
        if (Input.GetTouch(0).phase == TouchPhase.Moved || Input.GetTouch(0).phase == TouchPhase.Stationary)
        {
            HandleDrawing(Input.GetTouch(0).position);
        }
    }

    private void HandleDrawing(Vector3 screenPosition)
    {
        if (!TryGetHit(screenPosition, out RaycastHit hit))
        {
            return;
        }

        if (!TryGetPainter(hit, out Renderer renderer))
        {
            return;
        }


        if (hit.collider is not SphereCollider)
        {
            return;
        }

        Vector2 uv = CalculateSphereUV(hit);
        _currentPainter.Paint(uv, _brush);
        
        SaveTextureState();
    }

    private bool TryGetHit(Vector3 screenPosition, out RaycastHit hit)
    {
        Ray ray = _mainCamera.ScreenPointToRay(screenPosition);
        return Physics.Raycast(ray, out hit);
    }

    private bool TryGetPainter(RaycastHit hit, out Renderer renderer)
    {
        renderer = hit.collider.GetComponent<Renderer>();
        if (renderer == null)
        {
            return false;
        }

        if (_currentTextureManager == null || _currentTextureManager.gameObject != renderer.gameObject)
        {
            _currentTextureManager = renderer.GetComponent<TextureManager>();
            _currentTextureManager.InitializeTexture(renderer);
            _currentPainter = renderer.GetComponent<TexturePainter>();
            _drawingTexture = _currentTextureManager.GetTexture();
        }

        return true;
    }

    private void SaveTextureState()
    {
        if (_undoStack.Count >= MAX_UNDO_STEPS)
        {
            _undoStack.Pop();
        }
        _undoStack.Push((Color[])_drawingTexture.GetPixels().Clone());
    }

    private Vector2 CalculateSphereUV(RaycastHit hit)
    {
        Vector3 localPoint = hit.transform.InverseTransformPoint(hit.point);
        Vector3 normal = localPoint.normalized;

        float u = 0.5f + Mathf.Atan2(normal.z, normal.x) / (2 * Mathf.PI);
        float v = 1.0f - (0.5f - Mathf.Asin(normal.y) / Mathf.PI);

        return new Vector2(u, v);
    }

    public void SetBrushColor(Color color)
    {
        _brush.Color = color;
    }

    public void SetBrushSize(int size)
    {
        _brush.Size = size;
    }

    public void UndoLastAction()
    {
        if (_undoStack.Count <= 0)
        {
            return;
        }

        Color[] previousPixels = _undoStack.Pop();
        _drawingTexture.SetPixels(previousPixels);
        _drawingTexture.Apply();
        _currentTextureManager.SetTexture(_drawingTexture);
    }

    public void ClearDrawing()
    {
        Color[] clearPixels = new Color[_drawingTexture.width * _drawingTexture.height];
        for (int i = 0; i < clearPixels.Length; i++)
        {
            clearPixels[i] = Color.white;
        }
        _drawingTexture.SetPixels(clearPixels);
        _drawingTexture.Apply();
        _currentTextureManager.SetTexture(_drawingTexture);
    }
}
