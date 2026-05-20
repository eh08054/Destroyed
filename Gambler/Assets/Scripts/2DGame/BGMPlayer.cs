using UnityEngine;

public class BGMPlayer : MonoBehaviour
{
    private void Start()
    {
        AudioManager.instance.PlayBGM(BGM.TITLE); 
    }
}
