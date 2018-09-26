using UnityEngine;
using System.Collections;

public class yellowCard : MonoBehaviour 
{
    static public bool show = false;
    static public int n;
    static public int kick;
    static public int score;
    static public float time;

    public TextMesh titleTM;
    public TextMesh kickTM;
    public TextMesh scoreTM;
    public TextMesh timeTM;

    string[] title = new string[4];

    static public float timeCt = 0;

	void Start ()
    {
        title[0] = "- Single Ball -";
        title[1] = "- Double Ball -";
        title[2] = "- Tripple Ball -";
        title[3] = "- Marathon Mode -";
	}
	
	void Update () 
    {
        if (show)
        {
            titleTM.text = title[n - 1];
            kickTM.text = "kicks : " + kick.ToString();
            scoreTM.text = "score : " + score.ToString();
            timeTM.text = "time : " + time.ToString("0.00") + " s";

            if (timeCt >= 1f)
            transform.position = Vector3.Lerp(transform.position, new Vector3(0, 0.5f, -5), Time.deltaTime*5);

            timeCt += Time.deltaTime;
        }
        else
        {
            timeCt = 0;
            transform.position = Vector3.Lerp(transform.position, new Vector3(0, -8, -5), Time.deltaTime*5);
        }
	}
}
