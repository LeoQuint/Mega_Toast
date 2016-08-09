using UnityEngine;
using System.Collections.Generic;

public class SoundController : MonoBehaviour {

    public static SoundController instance = null;

    AudioSource aud;

    public List<SoundFXClass> soundFXList;

    void Awake()
    {


        //Check if instance already exists
        if (instance == null)
        {
            //if not, set instance to this
            instance = this;
        }
        //If instance already exists and it's not this:
        else if (instance != this)
        {
            Destroy(gameObject);
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

        aud.PlayOneShot(soundFXList[clipPos].clip, soundFXList[clipPos].volume);
    }
}
[System.Serializable]
public class SoundFXClass
{
    public AudioClip clip;
    public float volume;
}