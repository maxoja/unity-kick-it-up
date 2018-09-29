using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPool : MonoBehaviour {

    AudioSource[] audioSources;

    void Awake()
    {
        audioSources = GetComponentsInChildren<AudioSource>();
    }

    public bool IsFull()
    {
        return GetAvailableAudioSource() == null;
    }

    public AudioSource GetAvailableAudioSource()
    {
        foreach (AudioSource s in audioSources)
            if (s.isPlaying == false)
                return s;

        return null;
    }

    public void StopAll()
    {
        foreach (AudioSource s in audioSources)
            s.Stop();
    }
}
