using UnityEngine;

public class My3DCameraController : MonoBehaviour
{
    public GameObject Target;
   
    private float offsetX = 0.0f;      
    private float offsetY = 5.0f;
    private float offsetZ = -5.0f;
    public float cameraSpeed = 10.0f;
    Vector3 targetPos;

    private float mouseSensitivity = 1600f;
    private float mouseX = 0f;   
    private float mouseY = 0f;
    private float mouseXInc = 0f;  //x방향 변화량
    private float mouseYInc = 0f;  //y방향 변화량

    private void Start()
    {
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
    }
    private void Update()
    {
        Move();
        Rotate();    
    }
    private void Move()
    {
        Vector3 offset = new Vector3(offsetX, offsetY, offsetZ);
        Vector3 rotatedOffset = Quaternion.Euler(0, mouseX, 0) * offset;
        targetPos = Target.transform.position + rotatedOffset;
        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * cameraSpeed);
    }
    private void Rotate()
    {
        mouseXInc = Input.GetAxisRaw("Mouse X") * mouseSensitivity * Time.deltaTime;
        mouseYInc = Input.GetAxisRaw("Mouse Y") * mouseSensitivity * Time.deltaTime;
        mouseX += mouseXInc;
        mouseY -= mouseYInc;
        mouseY = Mathf.Clamp(mouseY, -90f, 90f);

        transform.localRotation = Quaternion.Euler(mouseY, mouseX, 0f);
        Target.transform.Rotate(Vector3.up * mouseXInc * 2);
    }
}
