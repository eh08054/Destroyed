using UnityEngine;
public class BackgroundScroll : MonoBehaviour
{
    [SerializeField][Range(1f, 200f)] float speed;
    [SerializeField] float posValue;
    Vector2 startPos;
    float newPos;

    void Start()
    {
        startPos = transform.position; 
    }

    void Update()
    {
        newPos = Mathf.Repeat(Time.time * speed, posValue);
        transform.position = startPos + Vector2.left * newPos;
    }
}
