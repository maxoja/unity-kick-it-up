using UnityEngine;
using System.Collections;

public class floatText : MonoBehaviour
{
    public TextMesh tm;
    public bool isActive = false;
    float timeCount = 0;

	void Update () 
    {
        timeCount += Time.deltaTime;
        transform.position += Vector3.up *Time.deltaTime;
        
        if(timeCount > 1)
        tm.color -= new Color(0, 0, 0, Time.deltaTime*5f);

        if (tm.color.a <= 0)
            isActive = false;
        else
            isActive = true;
	}

    public void FLoat(Vector3 pos,string text)
    {
        isActive = true;
        tm.text = text;
        transform.position = pos;
        timeCount = 0;

        tm.color = Color.white;
    }
}
