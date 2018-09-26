using UnityEngine;
using System.Collections;

public class gui : MonoBehaviour 
{
    int tempScore = 0;
    public TextMesh scoreText;
    public TextMesh comboText;
    public TextMesh levelText;
    public TextMesh[] highscoreText;

    public bumpTextMesh comboBump;

    int oldCombo = 0;

    void Awake()
    {
        scoreText.color = new Color(1, 1, 1, 0);
        comboText.transform.position = new Vector3(manager.edgeScreenPos - 0.2f, comboText.transform.position.y, comboText.transform.position.z);
        for (int ct1 = 0; ct1 < highscoreText.Length; ct1++)
        {
            highscoreText[ct1].transform.position = new Vector3(-manager.edgeScreenPos + 0.2f, manager.topScreenPos - 0.6f - ct1*0.5f, highscoreText[ct1].transform.position.z);
        }
    }

    void Update()
    {
        if (manager.mode == manager.SceneMode.Game)
        {
            int del = manager.score - tempScore;
            if(del != 0)
                tempScore += (del) / Mathf.Abs(del)*(Mathf.Abs(del)/30 + 1);
            if(tempScore > manager.score)
                tempScore = manager.score;
            scoreText.text = tempScore.ToString("#,###,###,###,##0");
            scoreText.color = Color.Lerp(scoreText.color, new Color(1, 1, 1, 0.4f), Time.deltaTime / 2);

            if (manager.modeSub == '4')
            {
                levelText.color = Color.Lerp(levelText.color, new Color(1, 1, 1, 0.8f), Time.deltaTime);
                if (manager.level < 3)
                    levelText.text = "LEVEL " + manager.level.ToString() + ((float)(manager.ballHit - manager.exp[manager.level - 1]) / (float)(manager.exp[manager.level] - manager.exp[manager.level - 1])).ToString(".00");
                else
                    levelText.text = "LEVEL 4.00"; 
            }
            else
            {
                levelText.color = Color.Lerp(levelText.color, new Color(1, 1, 1, 0.8f), Time.deltaTime);
                levelText.text = "kick " + manager.ballHit.ToString("0");
            }

            if (oldCombo != manager.combo && manager.combo > 1)
            {
                if (oldCombo < manager.combo)
                    comboBump.Bump();

                oldCombo = manager.combo;
            }

            highscoreText[0].text = "best : " + PlayerPrefs.GetInt("highScore" + manager.modeSub.ToString()).ToString("#,###,###,###,##0");
        }
        else
        {
            highscoreText[0].text = "game played : " + PlayerPrefs.GetInt("gamePlayed");

            scoreText.text = "0";
            scoreText.color = Color.Lerp(scoreText.color, new Color(1, 1, 1, 0), Time.deltaTime);
            levelText.color =  new Color(1, 1, 1, 0);
        }

        comboText.text = "x " + manager.combo.ToString("#,##0");
    }
    /*
    void OnGUI()
    {
        GUI.skin.box.fontSize = 25;
        GUI.skin.box.alignment = TextAnchor.MiddleCenter;
        GUI.Box(new Rect(100, 0, 100, 30), manager.score.ToString());
        GUI.Box(new Rect(0, 0, 100, 30), PlayerPrefs.GetInt("highScore").ToString());
    }
     */
}
