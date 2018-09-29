using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoundManager : MonoBehaviour 
{
    public SoundItem[] clips;

    private AudioPool audioSourcePool;
    private BGMPlayer bgmSource;
    private int playingCount = 0;

    static private Queue<Action> callQueue = new Queue<Action>();
    static private Queue<SoundTag> paramQueue = new Queue<SoundTag>();
	
    void Awake()
    {
        audioSourcePool = GetComponentInChildren<AudioPool>();
        bgmSource = GetComponentInChildren<BGMPlayer>();
    }

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
        AudioSource player = audioSourcePool.GetAvailableAudioSource();

        if (player == null)
        {
            Debug.LogWarning("there is no available audio source left available");
            return;
        }

        player.clip = GetClip(soundTag);
        player.Play();
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
        bgmSource.FadePlay();
    }

    private void StopAll()
    {
        bgmSource.FadeStop();
        audioSourcePool.StopAll();
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
