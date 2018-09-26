using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EasterText : MonoBehaviour 
{
    private TextMesh tm;
    private List<string> quoteList;

    private const float transitionDuration = 2 / 3;
    private const float displayDuration = 5;

    private const float maxAlpha = 0.6f;
    private float alpha = 0;

    private void Awake()
    {
        tm = GetComponent<TextMesh>();

        quoteList = new List<string>();
        quoteList.Add("Don't let any ball fall.");
        quoteList.Add("Kick straight to keep combos.");
        quoteList.Add("Collect whistle to earn bonus.");
        quoteList.Add("Reach blue stage to unlock next mode.");
        quoteList.Add("Created by Maxoja.");
        quoteList.Add("More ball, More fun.");
        quoteList.Add("Are you in trouble?");
        quoteList.Add("Keep calm and play cool!");
        quoteList.Add("Look at your ball, it's falling!");
    }

    IEnumerator Start () 
    {
        float r;

        while(true)
        {
            yield return new WaitUntil(() => manager.mode != manager.SceneMode.Game);

            Shuffle(quoteList);
            tm.text = quoteList[Random.Range(0, quoteList.Count - 1)];
            r = 0;

            while(r <= 1)
            {
                r += Time.deltaTime / transitionDuration;
                alpha = Mathf.Lerp(0, maxAlpha, r);
                yield return null;
            }

            yield return new WaitForSeconds(displayDuration);

            while(r >= 0)
            {
                r -= Time.deltaTime / transitionDuration;
                alpha = Mathf.Lerp(0, maxAlpha, r);
                yield return null;
            }

            yield return null;
        }
	}
	

	void Update () 
    {
        tm.color = new Color(1, 1, 1, alpha);
	}

    private void Shuffle<T>(IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, list.Count-1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}
