using UnityEngine;
using System.Collections;

public class bumpTextMesh : MonoBehaviour 
{
    bool bumping = false;
    Vector3 normalScale;
    Vector3 newScale;
    float amp = 1.5f;
    float speed = 20;

    float x = -1;

    public void Bump()
    {
        x = 0;
        if (bumping)
        {
            transform.localScale = normalScale;
        }
        else
        {
            normalScale = transform.localScale;
            newScale = normalScale * amp;

            bumping = true;
        }
   }

    void Update()
    {
        if (x != -1)
        {
            x += Time.deltaTime * speed;

            if (x > Mathf.PI)
            {
                x = -1;
                bumping = false;

                transform.localScale = normalScale;
            }
            else
            {
                transform.localScale = Vector3.Lerp(normalScale, newScale, Mathf.Sin(x));
            }
        }
    }
}
