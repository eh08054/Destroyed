using UnityEngine;
using TMPro;
using System.Collections;
public class FloatingText : MonoBehaviour
{
    [SerializeField] private TMP_Text floatingText;
    [SerializeField] private float baseScale;

    public void InitText(int damage, Vector2 position)
    {
        transform.position = position;
        floatingText.text = damage.ToString();

        float t = Mathf.Clamp01(damage / 100f);
        floatingText.color = Color.Lerp(Color.white, Color.red, t);

        float damageScale = damage / baseScale;
        gameObject.transform.localScale *= damageScale;
        StartCoroutine(PlayFloatingText());
    }
    private IEnumerator PlayFloatingText()
    {
        float duration = 1f;
        float elapsed = 0f;
        Vector2 startPos = transform.position;
        Vector2 endPos = startPos + new Vector2(0, 1.5f);

        while(elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            transform.position = Vector2.Lerp(startPos, endPos, t);
            floatingText.alpha = Mathf.Lerp(1f, 0f, t);
            yield return null;
        }
        Destroy(gameObject);
    }
}
