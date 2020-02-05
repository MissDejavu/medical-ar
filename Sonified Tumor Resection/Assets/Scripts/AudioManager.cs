using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    public static AudioManager instance;
    
    void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        if (sounds.Length <= 0)
        {
            Debug.LogWarning("No sounds defined!");
        }

        DontDestroyOnLoad(gameObject);
        foreach (Sound s in sounds)
        {
            s.gameObject = new GameObject(s.name+"SoundObject");
            s.source = s.gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.loop = s.loop;
            s.source.volume = s.volume = 1;
            s.source.pitch = s.pitch;
            if (s.name == "Margins" || s.name == "Tumor")
            {
                s.source.playOnAwake = false;
            }

            if(s.name == "Margins")
            {
                s.highPassFilter = s.gameObject.AddComponent<AudioHighPassFilter>();
                s.highPassFilter.cutoffFrequency = 100;
                s.lowPassFilter = s.gameObject.AddComponent<AudioLowPassFilter>();
                s.distortionFilter = s.gameObject.AddComponent<AudioDistortionFilter>();
                s.reverbFilter = s.gameObject.AddComponent<AudioReverbFilter>();
                s.reverbFilter.reverbPreset = AudioReverbPreset.Forest;
            }
        }

        Debug.Log("AudioManager awake done");
    }

    void Start()
    {
        SetVolume("Margins", 0.5f);
        Play(Constants.BackgroundSound);
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
            //Debug.Log("Start playing sound: " + name);
            s.source.Play();
        }
        else
        {
            //Debug.Log("Already playing sound: " + name);
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
        //Debug.Log("Stopping sound: " + name);
        s.source.Stop();
    }

    public void SetHighPassFrequency (string name, float frequency)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        s.highPassFilter.cutoffFrequency = frequency;
    }

    public void SetLowPassFrequency(string name, float frequency)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        s.lowPassFilter.cutoffFrequency = frequency;
    }

    public void SetDistortionLevel(string name, float value)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        s.distortionFilter.distortionLevel = value;
    }

    public void SetReverbFilter(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        s.reverbFilter.reverbPreset = AudioReverbPreset.Underwater;
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
