using System;
using System.Collections.Generic;
using UnityEngine;

public class DrawingController : MonoBehaviour
{
    public static DrawingController Instance { get; private set; }
    
    [SerializeField]
    private Camera _mainCamera;
    
    private readonly Brush _brush = new Brush(Color.cyan, 10);

    private TexturePainter _currentPainter;
    private Texture2D _drawingTexture;
    private Stack<Color []> _undoStack = new Stack<Color []>(5);

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
        if (!Input.GetMouseButton(0))
        {
            return;
        }

        Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);

        if (!Physics.Raycast(ray, out RaycastHit hit))
        {
            return;
        }

        Renderer renderer = hit.collider.GetComponent<Renderer>();

        if (renderer == null)
        {
            return;
        }

        if (_currentPainter == null || _currentPainter.gameObject != renderer.gameObject)
        {
            _currentPainter = renderer.GetComponent<TexturePainter>();

            if (_currentPainter == null)
            {
                _currentPainter = renderer.gameObject.AddComponent<TexturePainter>();
            }

            _currentPainter.InitializeTexture(renderer);
            _drawingTexture = _currentPainter.GetTexture();
        }

        if (hit.collider is not SphereCollider)
        {
            return;
        }

        if (_undoStack.Count < 5)
        {
            _undoStack.Push((Color[])_drawingTexture.GetPixels().Clone());
        } else
        {
            _undoStack.Pop();
            _undoStack.Push((Color[])_drawingTexture.GetPixels().Clone());
        }

        Vector2 uv = CalculateSphereUV(hit);
        _currentPainter.Paint(uv, _brush);
    }
    
    public void SetBrushColor(Color color)
    {
        _brush.Color = color;
    }
    
    public void SetBrushSize(int size)
    {
        _brush.Size = size;
    }

    private Vector2 CalculateSphereUV (RaycastHit hit)
    {
        Vector3 localPoint = hit.transform.InverseTransformPoint(hit.point);
        Vector3 normal = localPoint.normalized;

        float u = 0.5f + Mathf.Atan2(normal.z, normal.x) / (2 * Mathf.PI);
        float v = 1.0f - (0.5f - Mathf.Asin(normal.y) / Mathf.PI);

        return new Vector2(u, v);
    }

    public void UndoLastAction()
    {
        if (_undoStack.Count <= 0)
        {
            return;
        }

        Color [] previousPixels = _undoStack.Pop();
        _drawingTexture.SetPixels(previousPixels);
        _drawingTexture.Apply();
        _currentPainter.SetTexture(_drawingTexture);
    }

    public void ClearDrawing()
    {
        Color [] cleaPixels = new Color[_drawingTexture.width * _drawingTexture.height];

        for (int i = 0; i < cleaPixels.Length; i++)
        {
            cleaPixels[i] = Color.white;
            
        }
        _drawingTexture.SetPixels(cleaPixels);
        _drawingTexture.Apply();
        _currentPainter.SetTexture(_drawingTexture);
    }
}
