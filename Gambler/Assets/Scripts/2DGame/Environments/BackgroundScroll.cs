using UnityEngine;
public class BackgroundScroll : MonoBehaviour
{
    [SerializeField][Range(1f, 200f)] float speed;
    [SerializeField] float posValue;
    RectTransform rect;
    Vector2 startPos;
    float newPos;

    void Start()
    {
        rect = GetComponent<RectTransform>();
        startPos = rect.anchoredPosition;
    }

    void Update()
    {
        newPos = Mathf.Repeat(Time.time * speed, posValue);
        rect.anchoredPosition = startPos + Vector2.left * newPos;
    }
}
