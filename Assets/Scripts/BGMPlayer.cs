using UnityEngine;
using System.Collections;

public class BGMPlayer: MonoBehaviour 
{
    private bool playing = false;

	void Update () 
    {
        if (playing)
        {
            GetComponent<AudioSource>().volume += Time.deltaTime / 2;
        }
        else
        {
            GetComponent<AudioSource>().volume -= Time.deltaTime / 2;
            if (GetComponent<AudioSource>().volume <= 0)
            {
                GetComponent<AudioSource>().Stop();
            }
        }
	}

    public void FadePlay()
    {
        GetComponent<AudioSource>().Play();
        playing = true;
    }

    public void FadeStop()
    {
        playing = false;
    }

    public bool IsPlaying()
    {
        return playing;
    }
}
