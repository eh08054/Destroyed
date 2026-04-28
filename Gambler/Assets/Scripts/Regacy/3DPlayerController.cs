using UnityEngine;

public class My3DPlayerController : MonoBehaviour
{
    private My3DScoreSystem scoreSystem;
    private float speed = 10f;
    public void Init(My3DGameContext context)
    {
        scoreSystem = context.scoreSystem;
    }
    private void Update()
    {
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)){
            transform.Translate(0, 0, speed * Time.deltaTime);  
        }
        if(Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
        {
            transform.Translate(0, 0, -speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            transform.Translate(speed * Time.deltaTime, 0, 0);
        }
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            transform.Translate(-speed * Time.deltaTime, 0, 0);
        }
        if (Input.GetKey(KeyCode.Space))
        {
            transform.Translate(0, speed * Time.deltaTime, 0);
        }
    }
    //마우스가 클릭한 위치의 좌표와 1프레임 이후 이동한 위치의 좌표의 차이를 구하고 이를 큐브 회전에 반영한다. 
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "SmallCube")
        {
            Destroy(collision.gameObject);
            scoreSystem.AddScore(10);
        }
    }
}
