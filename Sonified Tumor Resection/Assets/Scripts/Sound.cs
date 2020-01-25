using UnityEngine;

//Sound class

[System.Serializable]
public class Sound
{
    //sound settings
    public string name;
    public bool loop;
    public AudioClip clip;
    [Range(Constants.MinVolume, Constants.MaxVolume)]
    public float volume;
    [Range(Constants.MinPitch, Constants.MaxPitch)]
    public float pitch;

    //[HideInInspector]
    public AudioSource source;
    public GameObject gameObject;

    //Audio filters
    public AudioHighPassFilter highPassFilter;
    public AudioLowPassFilter lowPassFilter;
    public AudioDistortionFilter distortionFilter;
    public AudioReverbFilter reverbFilter; 
}