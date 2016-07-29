using UnityEngine;
using System.Collections.Generic;

public class SoundController : MonoBehaviour {

    public static SoundController instance;

    AudioSource aud;

    public List<AudioClip> soundFXList;

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
        aud.volume = 0.7f;
    }

    public void Mute()
    {
        aud.volume = 0f;
    }
    public void UnMute()
    {
        aud.volume = 0.7f;
    }

    public void PlayClip(int clipPos)
    {
        Debug.Log(clipPos);
        aud.PlayOneShot(soundFXList[clipPos]);
    }
}
