using UnityEngine;
using System.Collections;

public class whistle : MonoBehaviour 
{
    public float size = 0.35f;
    public SpriteRenderer sRend;
    float x = 0;
    float angle = 0;
    int omeg = 15;

    public bool remove = false;

    Color oldCamColor;

	void Start () 
    {
        transform.localScale = Vector3.zero;
        SetColor();
	}
	
	void Update ()
    {
        if (oldCamColor != Camera.main.backgroundColor)
        {//set oldcamColor inside setcolor
            SetColor();
        }

        transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one * size, Time.deltaTime * 5);


        x += Time.deltaTime * 10;
        angle = omeg *Mathf.Sin(x) + 5;
        transform.eulerAngles = new Vector3(0, 0, angle);

        if (remove)
        {
            Destroy(gameObject);
        }
	}

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "ballAura")
        {
            if (manager.pause)
                return;

            soundManager.CallFunction(soundManager.Action.PlaySound, soundManager.SoundTag.CollectWhistle);
            fxManager.CallFunction(1, transform.position, "+" + (manager.combo * 3).ToString());

            manager.score += manager.combo * 3;
            manager.removeWhistle(gameObject);
        }
    }

    void SetColor()
    {
        oldCamColor = Camera.main.backgroundColor;

        float[] color = new float[3];
        float min = 100;
        int minDex = -1;
        float max = 0;
        int maxDex = -1;

        color[0] = oldCamColor.r;
        color[1] = oldCamColor.g;
        color[2] = oldCamColor.b;

        for (int ct1 = 0; ct1 < 3; ct1++)
        {
            if (color[ct1] > max)
            {
                max = color[ct1];
                maxDex = ct1;
            }

            if (color[ct1] < min)
            {
                min = color[ct1];
                minDex = ct1;
            }
        }

        color[minDex] = 0;

        float rat = 1f / color[maxDex];
        for (int ct1 = 0; ct1 < 3; ct1++)
        {
            if (ct1 != minDex)
            {
                color[ct1] *= rat;
            }
        }

        sRend.color = new Color(color[0], color[1], color[2], 1);
    }
}
