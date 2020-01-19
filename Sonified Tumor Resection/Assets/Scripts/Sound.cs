using UnityEngine;

//Sound class

[System.Serializable]
public class Sound
{
    public string name;

    public AudioClip clip;

    [Range(Constants.MinVolume, Constants.MaxVolume)]
    public float volume;
    [Range(Constants.MinPitch, Constants.MaxPitch)]
    public float pitch;

    //[HideInInspector]
    public AudioSource source;

    public AudioHighPassFilter highPassFilter;

    public bool loop;
}