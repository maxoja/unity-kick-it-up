using UnityEngine;
using System.Collections;

public class bgmPlayer : MonoBehaviour 
{
    public bool play = false;

	void Update () 
    {
        if (play)
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

    public void Play()
    {
        GetComponent<AudioSource>().Play();
        play = true;
    }

    public void Stop()
    {
        play = false;
    }
}
