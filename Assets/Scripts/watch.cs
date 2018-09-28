using UnityEngine;
using System.Collections;

public class watch : MonoBehaviour 
{
    private const float size = 0.3f;
    private const float normalZRotation = 90;
    private const float spinMultiplier = 2.5f * 360;
    private const float margin = size * 1.5f;
    private const float textGap = 0.5f;

    private const string playText = "play";
    private const string pauseText = "pause";

    private Color hideColor = new Color(1, 1, 1, 0);
    private Color showColor = new Color(1, 1, 1, 1);

    public TextMesh textMesh;

	void Awake () 
    {
        UpdatePosition();
	}
	
	void Update () 
    {
        UpdateText();
        UpdatePosition();
        UpdateRotation();
	}

    void UpdatePosition()
    {
        float posX = manager.edgeScreenPos - margin;
        float posY = manager.topScreenPos - margin;
        transform.position = new Vector2(posX, posY);

        Vector3 offset = Vector3.up * textGap;
        textMesh.transform.position = transform.position - offset;
    }

    void UpdateText()
    {
        if (manager.pause)
            textMesh.text = playText;
        else
            textMesh.text = pauseText;
    }

    void UpdateRotation()
    {
        if (manager.pause)
            return;
        
        if (manager.mode == manager.SceneMode.Game)
        {
            transform.Rotate(-transform.forward, manager.g * spinMultiplier * Time.deltaTime);
            textMesh.color = Color.Lerp(textMesh.color, showColor, Time.deltaTime);
        }
        else
        {
            transform.eulerAngles = new Vector3(0, 0, normalZRotation);
            textMesh.color = Color.Lerp(textMesh.color, hideColor, Time.deltaTime);
        }
    }

    void OnMouseDown()
    {
        if (manager.mode == manager.SceneMode.Game)
            manager.pause = !manager.pause;
    }
}
