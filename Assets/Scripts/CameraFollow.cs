using UnityEngine;

public class CameraFollow : MonoBehaviour{
  public Transform target;
  public float smoothSpeed = 0.125f;
  public Vector3 offset = new Vector3(0f, 0f, -10f);

  void LateUpdate() {
    if (!target) return;
    
    Vector3 desiredPosition = target.position + offset;
    Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
    transform.position = smoothedPosition;
  }
  
}