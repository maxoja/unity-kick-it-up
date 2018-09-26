using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class YellowCard : MonoBehaviour 
{
    static private bool isShowing = false;
    static private string titleText = "";
    static private string kickCountText = "";
    static private string scoreText = "";
    static private string timeText = "";

    static private string[] titleArray = new string[]{
        "- Single Ball -",
        "- Double Ball -",
        "- Tripple Ball -",
        "- Marathon Kick -"
    };

    const float delayTime = 1f;
    const float toggleSpeed = 5;
    const float showPosY = 0.5f;
    const float hidePosY = -8f;
    const float fixedX = 0;
    const float fixedZ = -5;

    [SerializeField]
    private TextMesh titleTM;
    [SerializeField]
    private TextMesh kickTM;
    [SerializeField]
    private TextMesh scoreTM;
    [SerializeField]
    private TextMesh timeTM;

    private float timeCounter = 0;

    void Start()
    {
        isShowing = false;
        StartCoroutine(AnimateCardForever(isShowing));
    }

    static public bool IsShowing() { return isShowing; }

    static public void Show() { isShowing = true; }

    static public void Hide() { isShowing = false; }

    static public void SetCardContent(int gameMode, int kickCount, int score, float time)
    {
        titleText = titleArray[gameMode - 1];
        kickCountText = "kicks : " + kickCount.ToString();
        scoreText = "score : " + score.ToString();
        timeText = "time : " + time.ToString("0.00") + " s";
    }

    private IEnumerator AnimateCardForever(bool show)
    {
        UpdateCardText();

        yield return new WaitForSeconds(show ? delayTime : 0);

        while(isShowing == show)
        {
            transform.position = Vector3.Lerp(
                transform.position, 
                new Vector3(fixedX, show ? showPosY : hidePosY, fixedZ), 
                Time.deltaTime*toggleSpeed
            );

            yield return null;
        }

        StartCoroutine(AnimateCardForever(!show));
    }

    private void UpdateCardText()
    {
        titleTM.text = titleText;
        kickTM.text = kickCountText;
        scoreTM.text = scoreText;
        timeTM.text = timeText;
    }



}
