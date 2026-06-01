using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
public class CameraController : MonoBehaviour
{
    private Transform playerTransform;
    private Transform background;
    private Collider2D mapCollider;
    private float minX;
    private float maxX;
    private float minY;
    private float maxY;
    private float cameraHalfHeight;
    private float cameraHalfWidth;
    [SerializeField]private float cameraThreshold = 5f;
    Vector3 vel = Vector3.zero;
    private void OnEnable()
    {
        GameManager.Instance.OnStageLoaded += InitCamera;
    }

    private void OnDisable()
    {
        GameManager.Instance.OnStageLoaded -= InitCamera;
    }

    private void LateUpdate()
    {
        float clampedX = Mathf.Clamp(playerTransform.position.x, minX, maxX);
        float clampedY;
        if(SceneManager.GetActiveScene().name == "MapScene")
        {
            clampedY = minY;
        }
        else if (playerTransform.position.y > cameraThreshold)
        {
            clampedY = Mathf.Clamp(minY + playerTransform.position.y - cameraThreshold, minY, maxY);
        }
        else
        {
            clampedY = Mathf.Clamp(playerTransform.position.y, minY, maxY);
        }
        Vector3 cameraPosition = new Vector3(clampedX, clampedY, transform.position.z);
        transform.position = Vector3.SmoothDamp(transform.position, cameraPosition, ref vel, 0.5f);
    }
    private void InitCamera()
    {
        background = GameManager.Instance.BackgroundOnly;
        if (background == null) { return; }

        mapCollider = background.GetComponent<BoxCollider2D>();
        cameraHalfHeight = Camera.main.orthographicSize;
        cameraHalfWidth = cameraHalfHeight * Camera.main.aspect;
        minX = mapCollider.bounds.min.x + cameraHalfWidth;
        maxX = mapCollider.bounds.max.x - cameraHalfWidth;
        minY = mapCollider.bounds.min.y + cameraHalfHeight;
        maxY = mapCollider.bounds.max.y - cameraHalfHeight;

        playerTransform = GameManager.Instance.Player.transform;
        float playerFeetY = playerTransform.position.y;
        float cameraY = playerFeetY + cameraHalfHeight;
        transform.position = new Vector3(playerTransform.position.x, cameraY, transform.position.z);
    }
}
