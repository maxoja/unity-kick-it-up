using UnityEngine;
using System.Collections;

public class mask : MonoBehaviour 
{
    private SpriteRenderer sRen;

	void Awake () 
    {
        sRen = GetComponent<SpriteRenderer>();
        sRen.color = Camera.main.backgroundColor;
	}
	
	void Update () 
    {
        // replicate the color of camera background
        sRen.color = Camera.main.backgroundColor;
	}
}
