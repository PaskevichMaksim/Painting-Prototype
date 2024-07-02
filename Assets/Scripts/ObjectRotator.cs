using UnityEngine;

public class ObjectRotator : MonoBehaviour
{
    [SerializeField]
    private float _speed = 30f;

    private Vector3 _lastMousePosition;
    private Vector2 _lastTouchPosition1;
    private Vector2 _lastTouchPosition2;
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
      
      if (Input.touchCount == 2)
      {
        RotateObjectWithTouch();
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
    
    private void RotateObjectWithTouch()
    {
      Touch touch1 = Input.GetTouch(0);
      Touch touch2 = Input.GetTouch(1);

      if (touch1.phase == TouchPhase.Moved || touch2.phase == TouchPhase.Moved)
      {
        Vector2 currentTouchPosition1 = touch1.position;
        Vector2 currentTouchPosition2 = touch2.position;

        Vector2 deltaTouchPosition1 = currentTouchPosition1 - _lastTouchPosition1;
        Vector2 deltaTouchPosition2 = currentTouchPosition2 - _lastTouchPosition2;

        float rotationX = (deltaTouchPosition1.x + deltaTouchPosition2.x) * _speed * Time.deltaTime;
        float rotationY = (deltaTouchPosition1.y + deltaTouchPosition2.y) * _speed * Time.deltaTime;

        transform.Rotate(Vector3.up, -rotationX, Space.World);
        transform.Rotate(Vector3.right, rotationY, Space.Self);

        _lastTouchPosition1 = currentTouchPosition1;
        _lastTouchPosition2 = currentTouchPosition2;
      } else
      {
        _lastTouchPosition1 = touch1.position;
        _lastTouchPosition2 = touch2.position;
      }
    }
}
