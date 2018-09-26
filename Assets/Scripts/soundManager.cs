using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class soundManager : MonoBehaviour 
{
    static public List<int> fCall = new List<int>();
    static public List<int> fParam = new List<int>();

    int playingCount = 0;
    public GameObject[] sources;
    public bgmPlayer[] bgmSources;
    public AudioClip[] clips;
	
	void Update () 
    {
        foreach (GameObject s in sources)
        {
            playingCount = 0;

            if (s.GetComponent<AudioSource>().isPlaying)
                playingCount++;
        }

        while (fCall.Count > 0)
        {
            switch (fCall[0])
            {
                case 1: PlaySound(fParam[0]); break;
                case 2: PlayBGM(fParam[0]); break;
                case 3: StopAll(); break;
            }

            fCall.RemoveAt(0);
            fParam.RemoveAt(0);
        }
	}

    static public void CallFunction(int id,int param)
    {
        fCall.Add(id);
        fParam.Add(param);
    }

    void PlaySound(int n)
    {
        foreach (GameObject s in sources)
        {
            if (!s.GetComponent<AudioSource>().isPlaying)
            {
                s.GetComponent<AudioSource>().clip = clips[n];
                s.GetComponent<AudioSource>().Play();

                break;
            }

        }
    }

    void PlayBGM(int index)
    {
        foreach (bgmPlayer b in bgmSources)
        {
            b.Stop();
        }

        bgmSources[index].Play();
    }

    void StopAll()
    {
        foreach (GameObject g in sources)
        {
            g.GetComponent<AudioSource>().Stop();
        }
        foreach(bgmPlayer b in bgmSources)
        {
            b.Stop();
        }
    }
}
