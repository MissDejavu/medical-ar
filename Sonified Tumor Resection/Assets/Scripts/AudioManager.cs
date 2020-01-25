using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    public static AudioManager instance;

    private AudioHighPassFilter highPassFilter;
    
    void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.loop = s.loop;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            if (s.name == "Margins" || s.name == "Tumor")
            {
                s.source.playOnAwake = false;
            }
        }

        if (sounds.Length <= 0)
        {
            Debug.LogWarning("No sounds defined!");
        }

        highPassFilter = gameObject.AddComponent<AudioHighPassFilter>();

        Debug.Log("AudioManager awake done");
    }

    void Start()
    {
        SetVolume("Margins", 0.5f);
    }

    public void SetVolume(string name, float value)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        s.source.volume = value;
    }

    public void SetPitch(string name, float value)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        //to smooth the change
        float velocity = 1.5f;
        float smoothTime = 0.0f; //the smaller the faster
        s.source.pitch = Mathf.SmoothDamp(1, value, ref velocity, smoothTime);
    }

    public void Play (string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        // check if sound is already playing - avoid restarting
        if (!s.source.isPlaying)
        {
            Debug.Log("Start playing sound: " + name);
            s.source.Play();
        }
        else
        {
            Debug.Log("Already playing sound: " + name);
        }
    }

  public void Stop (string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        Debug.Log("Stopping sound: " + name);
        s.source.Stop();
    }

    public void SetHighPassFrequency (float frequency)
    {
        highPassFilter.cutoffFrequency = frequency;
    }

    public void StopAll()
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            sounds[i].source.Stop();
        }
    }

    public void SetLoop(string name, bool value)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        s.source.loop = value;
    }
}
