using UnityEngine;

public class ObjectRotator : MonoBehaviour
{
    [SerializeField]
    private float _speed = 30f;

    private Vector3 _lastMousePosition;
    private bool _isRotating;

    private void Update()
    {
     HandleRotation();   
    }

    private void HandleRotation()
    {
      if (Input.GetMouseButtonDown(1))
      {
        _isRotating = true;
        _lastMousePosition = Input.mousePosition;
      } else if (Input.GetMouseButtonUp(1))
      {
        _isRotating = false;
      }

      if (_isRotating)
      {
        RotateObject();
      }
    }

    private void RotateObject()
    {
      Vector3 deltaMousePosition = Input.mousePosition - _lastMousePosition;
      float rotationX = deltaMousePosition.x * _speed * Time.deltaTime;
      float rotationY = deltaMousePosition.y * _speed * Time.deltaTime;
      
      transform.Rotate(Vector3.up, -rotationX,Space.World);
      transform.Rotate(Vector3.right, rotationY,Space.Self);

      _lastMousePosition = Input.mousePosition;
    }
}
