using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum BGM
{
    TITLE,
    MAP,
    BATTLE,
}

public enum SFX
{
    Slash,
    Slash_Hit,
    Jab,
    Jab_Hit,
    Shot,
}

[System.Serializable]
public class Sound
{
    public int SoundLimit;
    public int PlayedNum;
    public AudioClip audioClip;
}
public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    private AudioSource bgmPlayer;

    [SerializeField] private Sound[] BGMs;
    [SerializeField] private Sound[] SFXs;

    [SerializeField] private int poolsize = 10;

    private Dictionary<BGM, Sound> bgmDict;
    private Dictionary<SFX, Sound> sfxDict;
    private Queue<AudioSource> audioSourcePool;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            Init();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Init()
    {
        bgmDict = new Dictionary<BGM, Sound>();
        for (int i = 0; i < BGMs.Length; i++)
        {
            bgmDict[(BGM)i] = BGMs[i];
        }
        sfxDict = new Dictionary<SFX, Sound>();
        for (int i = 0; i < SFXs.Length; i++)
        {
            sfxDict[(SFX)i] = SFXs[i];
        }

        bgmPlayer = gameObject.AddComponent<AudioSource>();
        bgmPlayer.loop = true;

        InitPool();
    }

    private void InitPool()
    {
        audioSourcePool = new Queue<AudioSource>();

        for(int i = 0; i < poolsize; i++)
        {
            AudioSource source = gameObject.AddComponent<AudioSource>();
            source.playOnAwake = false;
            source.enabled = false;
            audioSourcePool.Enqueue(source);
        }
    }

    public void PlayBGM(BGM bgmType)
    {
        if(bgmDict.TryGetValue(bgmType, out var clip))
        {
            if(bgmPlayer.clip != clip.audioClip)
            {
                bgmPlayer.clip = clip.audioClip;
                bgmPlayer.Play();
            }
        }
        else
        {
            Debug.LogWarning("BGM NOT FOUND");
        }
    }

    public void PlaySFX(SFX sfxType)
    {
        if (sfxDict.TryGetValue(sfxType, out var clip))
        {
            if (audioSourcePool.Count > 0)
            {
                if (sfxDict[sfxType].PlayedNum < sfxDict[sfxType].SoundLimit)
                {
                    AudioSource source = audioSourcePool.Dequeue();
                    source.clip = clip.audioClip;
                    source.enabled = true;
                    source.Play();
                    sfxDict[sfxType].PlayedNum++;

                    StartCoroutine(ReturnToPool(source, clip));
                }
            }
            else
            {
                if (sfxDict[sfxType].PlayedNum < sfxDict[sfxType].SoundLimit)
                {
                    AudioSource newSource = new AudioSource();
                    newSource.clip = clip.audioClip;
                    newSource.playOnAwake = false;
                    newSource.enabled = true;
                    newSource.Play();
                    sfxDict[sfxType].PlayedNum++;

                    StartCoroutine(ReturnToPool(newSource, clip));
                }
            }
        }
        else
        {
            Debug.LogWarning("SFX NOT FOUND");
        }
    }

    public void PlaySoundOnObject(SFX sfxType, GameObject gameObject)
    {
        if(sfxDict.TryGetValue(sfxType, out var clip))
        {
            AudioSource source = gameObject.GetComponent<AudioSource>();
            if(source == null)
            {
                source = gameObject.AddComponent<AudioSource>();
            }
            source.clip = clip.audioClip;
            source.Play();
        }
    }
    private IEnumerator ReturnToPool(AudioSource source, Sound clip)
    {
        yield return new WaitForSeconds(clip.audioClip.length);
        source.enabled = false;
        clip.PlayedNum--;
        audioSourcePool.Enqueue(source);
    }
}
