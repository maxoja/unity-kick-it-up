using UnityEngine;
using System.Collections;

public class stageButtonHide : MonoBehaviour 
{
    public int id;
    public string title;
    bool setted = false;
    public TextMesh tm;

	
	void Update () 
    {
        if (manager.mode == manager.SceneMode.Menu)
        {
            if (setted == false)
            {
                if (PlayerPrefs.GetInt("clear") >= id -1)
                {
                    tm.fontSize = 50;
                    tm.text = title;
                }
                else
                {
                    tm.fontSize = 35;
                    tm.text = (75 * (id-1)).ToString() + " kicks to unlock";
                }

                setted = true;
            }
        }
        else
            setted = false;
	}

}
