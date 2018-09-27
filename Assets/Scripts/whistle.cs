using UnityEngine;
using System.Collections;

public class Whistle : MonoBehaviour 
{
    private SpriteRenderer sRend;

    private const float size = 0.25f;
    private const float swingAngle = 15;
    private const float angularOffset = 5;
    private const float scalingSpeed = 5;
    private const float swingingSpeed = 10;

    private float x = 0;
    private Color oldCamColor;
    private bool toBeDestroyed = false;

    private void Awake()
    {
        sRend = GetComponent<SpriteRenderer>();
    }

    private void Start () 
    {
        ScaleToZero();
        UpdateColor();
	}
	
	private void Update ()
    {
        UpdateColor();
        UpdateScale();
        UpdateRotation();

        if (toBeDestroyed)
        {
            StopAllCoroutines();
            Destroy(gameObject);
        }
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "ballAura")
        {
            if (manager.pause)
                return;

            SoundManager.CallFunction(SoundManager.Action.PlaySound, SoundManager.SoundTag.CollectWhistle);
            fxManager.CallFunction(1, transform.position, "+" + (manager.combo * 3).ToString());

            manager.score += manager.combo * 3;
            manager.RemoveOneWhistle(this);
        }
    }

    public void TagToDestroy()
    {
        toBeDestroyed = true;
    }

    private void ScaleToZero()
    {
        transform.localScale = Vector3.zero;
    }

    void UpdateColor()
    {
        if (oldCamColor == Camera.main.backgroundColor)
            return;
        
        oldCamColor = Camera.main.backgroundColor;

        float min = 100;
        int minDex = -1;
        float max = 0;
        int maxDex = -1;

        Color color = oldCamColor;

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

        float ratio = 1f / color[maxDex];
        for (int ct1 = 0; ct1 < 3; ct1++)
        {
            if (ct1 != minDex)
            {
                color[ct1] *= ratio;
            }
        }

        color.a = 1;
        sRend.color = color;
    }

    private void UpdateScale()
    {
        Vector3 oldScale = transform.localScale;
        Vector3 targetScale = Vector3.one * size;
        Vector3 updatedScale = Vector3.Lerp(oldScale, targetScale, Time.deltaTime * scalingSpeed);

        transform.localScale = updatedScale;
    }

    private void UpdateRotation()
    {
        x += Time.deltaTime * swingingSpeed;
        float resultedAngle = swingAngle * Mathf.Sin(x) + angularOffset;
        transform.eulerAngles = new Vector3(0, 0, resultedAngle);
    }

}
