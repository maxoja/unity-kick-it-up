using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class soundManager : MonoBehaviour 
{
    public BGMPlayer bgmSource;
    public GameObject[] sources;
    public SoundItem[] clips;

    static private Queue<Action> callQueue = new Queue<Action>();
    static private Queue<SoundTag> paramQueue = new Queue<SoundTag>();

    private int playingCount = 0;
	
	void Update () 
    {
        while (callQueue.Count > 0)
        {
            Action action = callQueue.Dequeue();
            SoundTag param = paramQueue.Dequeue();

            switch (action)
            {
                case Action.PlaySound: PlaySound(param); break;
                case Action.PlayBGM: PlayBGM(); break;
                case Action.StopAll: StopAll(); break;
            }
        }
	}

    static public void CallFunction(Action action, SoundTag tag)
    {
        callQueue.Enqueue(action);
        paramQueue.Enqueue(tag);
    }

    private void PlaySound(SoundTag soundTag)
    {
        foreach (GameObject s in sources)
        {
            if (!s.GetComponent<AudioSource>().isPlaying)
            {
                s.GetComponent<AudioSource>().clip = GetClip(soundTag);
                s.GetComponent<AudioSource>().Play();

                break;
            }
        }
    }

    private AudioClip GetClip(SoundTag soundTag)
    {
        foreach (SoundItem s in clips)
            if (s.tag == soundTag)
                return s.clip;
        
        return null;
    }

    private void PlayBGM()
    {
        bgmSource.FadeStop();
        bgmSource.FadePlay();
    }

    private void StopAll()
    {
        bgmSource.FadeStop();

        foreach (GameObject g in sources)
            g.GetComponent<AudioSource>().Stop();
    }

    [System.Serializable]
    public class SoundItem
    {
        public SoundTag tag;
        public AudioClip clip;
    }

    public enum SoundTag
    {
        PerfectKick,
        FairKick,
        BallClashed,
        CollectWhistle,
        LongWhistle,

        None,
    }

    public enum Action
    {
        PlaySound,
        PlayBGM,
        StopAll,
    }
}
