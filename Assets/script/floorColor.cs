using UnityEngine;
using System.Collections;

public class floorColor : MonoBehaviour 
{
    SpriteRenderer sRen;

    void Awake()
    {
        sRen = GetComponent<SpriteRenderer>();
        sRen.color = manager.stageFloorColor[0];
    }

	void Update () 
    {
        sRen.color = Color.Lerp(sRen.color, manager.floorColor,Time.deltaTime*3);
	}
}
