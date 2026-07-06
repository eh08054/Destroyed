using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GraphicBlink
{
    public IEnumerator FadeGraphic(Graphic graphicObject, float start, float end, float fadeTime, string SceneName = null)
    {
        float current = 0;
        float percent = 0;

        while (percent < 1)
        {
            current += Time.deltaTime;
            percent = current / fadeTime;

            Color color = graphicObject.color;
            color.a = Mathf.Lerp(start, end, percent);
            graphicObject.color = color;

            yield return null;
        }
    }

    //MS = Move & Shrink
    public IEnumerator MSGraphic(RectTransform transform, Vector2 StartPos, Vector2 EndPos, Vector3 StartScale,
        Vector3 EndScale, float moveTime)
    {
        float current = 0;
        float percent = 0;

        while (percent < 1)
        {
            current += Time.deltaTime;
            percent = current / moveTime;

            transform.anchoredPosition = Vector2.Lerp(StartPos, EndPos, percent);
            transform.localScale = Vector3.Lerp(StartScale, EndScale, percent);
            yield return null;
        }
        transform.anchoredPosition = EndPos;
    }
}
