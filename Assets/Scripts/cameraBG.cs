using UnityEngine;
using System.Collections;

public class cameraBG : MonoBehaviour 
{
	void Update () 
    {
        GetComponent<Camera>().backgroundColor = Color.Lerp(GetComponent<Camera>().backgroundColor, manager.bgColor , Time.deltaTime * 3);	
	}
}
