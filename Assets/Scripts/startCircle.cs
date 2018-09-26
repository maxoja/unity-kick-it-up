using UnityEngine;
using System.Collections;

public class startCircle : MonoBehaviour 
{
    public bool remove = false;

    Vector3 scale;

    void Start()
    {
        scale = transform.lossyScale;
    }

    void Update () 
    {
        transform.Rotate(0, 0, 20 * Time.deltaTime);

        if (remove)
        {
            Remove();
        }
	}

    bool bigged = false;
    void Remove()
    {
        transform.parent = null;

        if (transform.localScale.x < scale.x*1.2f && bigged == false)
        {
            transform.localScale *= 1 + 3*Time.deltaTime;
            return;
        }

        bigged = true;
        transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, Time.deltaTime * 10);

        if (transform.localScale.x < 0.05f)
        {
            Destroy(gameObject);
        }
    }
}
