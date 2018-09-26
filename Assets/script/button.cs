using UnityEngine;
using System.Collections;

public class button : MonoBehaviour 
{
    public int id = -1;
    static public bool enable = true;

    Vector3 sPos;

    SpriteRenderer sRend;
    TextMesh tm;
    
    Color nColor1;
    Color nColor2;

    bool move = false;
    bool applied = false;
    void Start()
    {
        sPos = transform.position;

        sRend = GetComponentInChildren<SpriteRenderer>();
        tm = GetComponentInChildren<TextMesh>();

        nColor1 = sRend.color;
        nColor2 = tm.color;
    }

	void OnMouseDown () 
    {
        if (!enable)
            return;

        if (PlayerPrefs.GetInt("clear") < id - 1 && id <= 4)
        {
           return;
        }

        switch (id)
        {
            case -1: 
                Debug.Log("unknown button id on : " + gameObject.name); 
                    return;
            case 5: //achievement
                    if (!Social.Active.localUser.authenticated)
                        ggManager.SignIn();
                    else
                        ggManager.showAchievement();
                    return;
            case 6: //leaderboard
                    if (!Social.Active.localUser.authenticated)
                        ggManager.SignIn();
                    else
                        ggManager.showLeaderBoard();
                    return;
        }

        manager.LaunchGame(id);
	}

    void Update()
    {
        if (enable == false)
        {
            if (applied == false)
            {
                Invoke("Move",(float)id/10f);

                applied = true;
            }

            if (move)
            {
                transform.position = Vector3.Lerp(transform.position, sPos + Vector3.right * 8, Time.deltaTime);

                sRend.color -= new Color(0, 0, 0, Time.deltaTime);
                tm.color -= new Color(0, 0, 0, Time.deltaTime);
            }
        }
        else
        {
            move = false;
            applied = false;
            transform.position = sPos;

            //if (yellowCard.show)
            {
            //    if (yellowCard.timeCt >= 1)
                {
                    sRend.color = Color.Lerp(sRend.color, nColor1, Time.deltaTime * 10f);
                    tm.color = Color.Lerp(tm.color, nColor2, Time.deltaTime * 5f);
                }
            }
            //else
            {
            //    sRend.color = Color.Lerp(sRend.color, nColor1, Time.deltaTime * 10f);
            //    tm.color = Color.Lerp(tm.color, nColor2, Time.deltaTime * 10f);
            }
        }
    }

    void Move()
    {
        move = true;
    }
}
