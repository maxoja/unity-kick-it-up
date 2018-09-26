using UnityEngine;
using System.Collections;

public class ballBg : MonoBehaviour 
{
    public bool remove = false;
    SpriteRenderer sRen;

    void Awake()
    {
        sRen = GetComponent<SpriteRenderer>();
        sRen.color = manager.stageFloorColor[0];
    }
	
	void Update () 
    {
        sRen.color = Color.Lerp(sRen.color, manager.floorColor*0.8f, Time.deltaTime * 3);

        if (remove)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero,Time.deltaTime*10);
            return;
        }

        transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one * (manager.g / 2 + 1) / ballPhysics.size, Time.deltaTime);
	}
}
