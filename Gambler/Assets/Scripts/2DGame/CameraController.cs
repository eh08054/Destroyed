using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Transform playerTransform;
    private Collider2D mapCollider;
    private float minX;
    private float maxX;
    private float cameraHalfWidth;
    private void Start()
    {
        mapCollider = GameManager.Instance.BackGround.GetComponent<BoxCollider2D>();
        playerTransform = GameManager.Instance.Player.transform;
        cameraHalfWidth = Camera.main.orthographicSize * Camera.main.aspect;
        Debug.Log(Camera.main.orthographicSize + " " + Camera.main.aspect + " " + cameraHalfWidth);
        minX = mapCollider.bounds.min.x + cameraHalfWidth;
        maxX = mapCollider.bounds.max.x - cameraHalfWidth;
    }
    private void LateUpdate()
    {
        float clampedX = Mathf.Clamp(playerTransform.position.x, minX, maxX);
        transform.position = new Vector3(clampedX, transform.position.y,
            transform.position.z);
    }
}
