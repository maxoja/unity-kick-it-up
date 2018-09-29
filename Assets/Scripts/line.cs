using UnityEngine;
using System.Collections;

public class line : MonoBehaviour 
{
	void Start () 
    {
        transform.position = Vector3.up * manager.kickZone;
	}
	
	void Update () 
    {
        transform.position = Vector3.Lerp(transform.position, new Vector3(0, manager.kickZone, 0), Time.deltaTime*2);
	}
}
