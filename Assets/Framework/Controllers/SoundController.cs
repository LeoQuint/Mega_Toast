using UnityEngine;
using System.Collections.Generic;

public class SoundController : MonoBehaviour {

    public static SoundController instance;

    AudioSource aud;

    public List<SoundFXClass> soundFXList;

    void Awake()
    {


        if (instance != null)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(gameObject);

    }

    void Start()
    {
        aud = gameObject.GetComponent<AudioSource>();
        aud.volume = 0.25f;
    }

    public void Mute()
    {
        aud.volume = 0f;
    }
    public void UnMute()
    {
        aud.volume = 0.25f;
    }

    public void PlayClip(int clipPos)
    {
        Debug.Log(clipPos);
        aud.PlayOneShot(soundFXList[clipPos].clip, soundFXList[clipPos].volume);
    }
}
[System.Serializable]
public class SoundFXClass
{
    public AudioClip clip;
    public float volume;
}