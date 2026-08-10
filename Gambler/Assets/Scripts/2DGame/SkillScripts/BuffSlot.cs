using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuffSlot : MonoBehaviour
{
    public Image icon;
    public bool IsEmpty => icon.sprite == null;
    public TMP_Text remainedTime;
    private float time;

    public void StartBuff(ActiveSkill skill, Action OnEnd)
    {
        StartCoroutine(StartDuration(skill.Duration, OnEnd));
    }
    public void StartBuff(ItemData item, Action OnEnd)
    {
        StartCoroutine(StartDuration(item.durationTime, OnEnd));
    }
    public IEnumerator StartDuration(float buffTime, Action OnEnd)
    {
        remainedTime.gameObject.SetActive(true);
        time = buffTime;

        while (time > 0f)
        {
            time -= Time.deltaTime;
            time = Mathf.Max(0f, time);

            remainedTime.text = time.ToString("F0");

            yield return null;
        }
        OnEnd?.Invoke();
        Destroy(gameObject);
    } 
}
