using UnityEngine;
public class MySpriteEffect : MonoBehaviour
{
    public void Play(string clipName, int direction = 1)
    {
        transform.localScale = new Vector3(direction, 1, 1);
        GetComponent<Animator>().Play(clipName);
        Destroy(gameObject, 0.25f);
    }
}