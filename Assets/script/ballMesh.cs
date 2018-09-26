using UnityEngine;
using System.Collections;

public class ballMesh : MonoBehaviour 
{
    public float angularVelo = 0;
    public SpriteRenderer sRend;
	
	void Update () 
    {
        transform.Rotate(transform.forward, angularVelo * Time.deltaTime);

        if(manager.pause == false)
        angularVelo *= 0.99f;
	}
}
