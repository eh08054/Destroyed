using System;
using UnityEngine;

[CreateAssetMenu(fileName = "MyEffectManager", menuName = "Scriptable Objects/MyEffectManager")]
public class MyEffectManager : ScriptableObject
{
    public MySpriteEffect SpriteEffectPrefab;
    public static MyEffectManager Instance;

    [RuntimeInitializeOnLoadMethod]
    static void Initialize()
    {
        Instance = Resources.Load<MyEffectManager>("2DGame/MyEffectManager");
    }

    public MySpriteEffect CreateSpriteEffect(GameObject player, string clipName, int direction = 0, Transform parent = null)
    {
        var instance = Instantiate(SpriteEffectPrefab, player.transform.position, Quaternion.identity, parent);

        instance.name = clipName;
        instance.transform.position = parent == null ? player.transform.position : parent.transform.position;
        instance.GetComponent<SpriteRenderer>().sortingOrder = player.GetComponent<SpriteRenderer>().sortingOrder + 1;
        instance.Play(clipName, direction == 0 ? Math.Sign(player.transform.localScale.x) : direction);

        return instance;
    }
}