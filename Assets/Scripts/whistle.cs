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

    public void TagToDestroy()
    {
        toBeDestroyed = true;
    }

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

    private void ScaleToZero()
    {
        transform.localScale = Vector3.zero;
    }

    private void UpdateColor()
    {
        if (oldCamColor == Camera.main.backgroundColor)
            return;
        
        oldCamColor = Camera.main.backgroundColor;
        sRend.color = CalculateBrightColor(oldCamColor);
    }

    private void FindWeakestStrongestChannel(Color color, out int weakestIndex, out int strongestIndex)
    {
        weakestIndex = 0;
        strongestIndex = 0;
        float weakestValue = color[weakestIndex];
        float strongestValue = color[strongestIndex];

        for (int i = 1; i < 3; i++)
        {
            if (color[i] > strongestValue)
            {
                strongestValue = color[i];
                strongestIndex = i;
            }

            if (color[i] < weakestValue)
            {
                weakestValue = color[i];
                weakestIndex = i;
            }
        }
    }

    private Color CalculateBrightColor(Color baseColor)
    {
        int strongestChannel, weakestChannel;
        FindWeakestStrongestChannel(baseColor, out weakestChannel, out strongestChannel);

        Color newColor = baseColor;
        newColor[weakestChannel] = 0;
        newColor[(0 + 1 + 2) - (weakestChannel + strongestChannel)] *= 1f / newColor[strongestChannel];
        newColor[strongestChannel] = 1;
        newColor.a = 1;

        return newColor;
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
