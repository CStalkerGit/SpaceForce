using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour
{
    public AudioClip clip;

    public static Music instance = null;

    void Start()
    {
        AudioSource source = GetComponent<AudioSource>();

        if (!instance)
        {
            instance = this;
            Play();
            DontDestroyOnLoad(this);
        }
        else
        {
            if (instance.GetComponent<AudioSource>().clip != clip)
            {
                instance.clip = clip;
                instance.Play();
            }
            DestroyImmediate(this.gameObject);
        };
    }

    void Play()
    {
        GetComponent<AudioSource>().clip = clip;
        if (clip == null)
        {
            GetComponent<AudioSource>().Stop();
            return;
        }
        GetComponent<AudioSource>().Play();
        GetComponent<AudioSource>().loop = true;
    }

    public void Volume(float vol)
    {
        GetComponent<AudioSource>().volume = vol;
    }

    public float GetVolume()
    {
        return GetComponent<AudioSource>().volume;
    }

    public static void GlobalVolume(float volume)
    {
        if (instance) instance.Volume(volume);
    }
}