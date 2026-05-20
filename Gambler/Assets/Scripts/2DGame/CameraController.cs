using UnityEngine;
using UnityEngine.SceneManagement;
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
    private float cameraThreshold = 5f;
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
        Debug.Log(minY + " " + maxY + " " + cameraHalfHeight);
        float playerFeetY = playerTransform.position.y;
        float cameraY = playerFeetY + cameraHalfHeight;
        transform.position = new Vector3(transform.position.x, cameraY, transform.position.z);
    }
    private void LateUpdate()
    {
        float clampedX = Mathf.Clamp(playerTransform.position.x, minX, maxX);
        float clampedY;
        if (playerTransform.position.y > cameraThreshold && SceneManager.GetActiveScene().name != "MapScene")
        {
            clampedY = Mathf.Clamp(minY + playerTransform.position.y - cameraThreshold, minY, maxY);
        }
        else
        {
            clampedY = Mathf.Clamp(playerTransform.position.y, minY, maxY);
        }
            transform.position = new Vector3(clampedX, clampedY,
                transform.position.z);
    }
}
