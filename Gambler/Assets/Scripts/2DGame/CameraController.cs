using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Transform playerTransform;
    private Collider2D mapCollider;
    private float minX;
    private float maxX;
    private float minY;
    private float maxY;
    private float cameraHalfHeight;
    private float cameraHalfWidth;
    private void Start()
    {
        mapCollider = GameManager.Instance.BackGround.GetComponent<BoxCollider2D>();
        playerTransform = GameManager.Instance.Player.transform;
        cameraHalfHeight = Camera.main.orthographicSize;
        cameraHalfWidth = cameraHalfHeight * Camera.main.aspect;
        minX = mapCollider.bounds.min.x + cameraHalfWidth;
        maxX = mapCollider.bounds.max.x - cameraHalfWidth;
        minY = mapCollider.bounds.min.y + cameraHalfHeight;
        maxY = mapCollider.bounds.max.y - cameraHalfHeight;

        float playerFeetY = playerTransform.position.y;
        float cameraY = playerFeetY + cameraHalfHeight;
        transform.position = new Vector3(transform.position.x, playerFeetY + (mapCollider.bounds.max.y - mapCollider.bounds.min.y) / 2, transform.position.z);
    }
    private void LateUpdate()
    {
        float clampedX = Mathf.Clamp(playerTransform.position.x, minX, maxX);
        transform.position = new Vector3(clampedX, transform.position.y,
            transform.position.z);
    }
}
