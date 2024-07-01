using UnityEngine;

public class DrawingController : MonoBehaviour
{
    [SerializeField]
    private Camera _mainCamera;
    
    private Brush _brush = new Brush(Color.cyan, 10);

    private TexturePainter _currentPainter;

    public void SetBrushProperties(Color color, int size)
    {
        _brush.Color = color;
        _brush.Size = size;
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

    private Vector2 CalculateSphereUV (RaycastHit hit)
    {
        Vector3 localPoint = hit.transform.InverseTransformPoint(hit.point);
        Vector3 normal = localPoint.normalized;

        float u = 0.5f + Mathf.Atan2(normal.z, normal.x) / (2 * Mathf.PI);
        float v = 1.0f - (0.5f - Mathf.Asin(normal.y) / Mathf.PI);

        return new Vector2(u, v);
    }
}
