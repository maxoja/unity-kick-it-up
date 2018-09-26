using UnityEngine;
using System.Collections;

public class forceUpEffect : MonoBehaviour 
{
    public bool isActive = false;
    float time = 0;

    public SpriteRenderer sRen = null;

	void Update () 
    {
        time += Time.deltaTime;

        transform.Translate(Vector3.up * 30 * Time.deltaTime);
        sRen.color -= new Color(0, 0, 0, 2 * Time.deltaTime);

        if (sRen.color.a <= 0)
            isActive = false;
        else
            isActive = true;
	}

    public void Force(Vector3 pos)
    {
        isActive = true;
        transform.position = pos;
        sRen.color = Color.white;
    }
}
