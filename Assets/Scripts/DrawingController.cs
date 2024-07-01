using UnityEngine;

public class DrawingController : MonoBehaviour
{
    public static DrawingController Instance { get; private set; }
    
    [SerializeField]
    private Camera _mainCamera;
    
    private readonly Brush _brush = new Brush(Color.cyan, 10);

    private TexturePainter _currentPainter;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        } else
        {
            Destroy(gameObject);
        }
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
        }

        if (hit.collider is not SphereCollider)
        {
            return;
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
}
