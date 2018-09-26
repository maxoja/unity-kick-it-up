using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//using GooglePlayGames;
//using UnityEngine.SocialPlatforms;
//using GooglePlayGames.BasicApi;

public class ggManager : MonoBehaviour 
{
    static public bool suc = false;

    void Start()
    {
        //PlayGamesPlatform.Activate();

        SignIn();
    }

    static public void SignIn()
    {
        Debug.LogWarning("SignIn to google game does not support anymore");
        return;

        Social.localUser.Authenticate((bool success) =>
        {
            suc = success;

            if (suc)
            {
                ggManager.UnlockAchievement(5);

                for (int ct1 = 1; ct1 <= 4; ct1++)
                {
                    postScore(ct1);
                }
            }
        });
    }

    static public void postScore(int n)
    {
        Debug.LogWarning("postScore does not suppot anymore");
        return;

        if (!Social.Active.localUser.authenticated)
            return;

        int score = PlayerPrefs.GetInt("highScore" + n.ToString());

        string boardID = "";

        switch(n)
        {
            case 1: boardID = "CgkIlbiS2s8FEAIQBg"; break;
            case 2: boardID = "CgkIlbiS2s8FEAIQBw"; break;
            case 3: boardID = "CgkIlbiS2s8FEAIQCA"; break;
            case 4: boardID = "CgkIlbiS2s8FEAIQCQ"; break;
        }

        if(boardID != "")
        Social.ReportScore(score, boardID, (bool success) =>{});
    }

    static public void showLeaderBoard()
    {
        Debug.LogWarning("showLeaderBoard does not support anymore");
        return;

        if (!Social.Active.localUser.authenticated)
            return;

        Social.ShowLeaderboardUI();
    }

    static public void showAchievement()
    {
        Debug.LogWarning("showAchievement does not support anymore");
        return;

        if (!Social.Active.localUser.authenticated)
            return;

        Social.ShowAchievementsUI();
    }

    static public void UnlockAchievement(int n)
    {
        Debug.LogWarning("UnlockAchievement is currently not support anymore");
        return;

        string boardID = "";

        switch (n)
        {
            case 1: boardID = "CgkIlbiS2s8FEAIQAg"; break;
            case 2: boardID = "CgkIlbiS2s8FEAIQAw"; break;
            case 3: boardID = "CgkIlbiS2s8FEAIQBA"; break;
            case 4: boardID = "CgkIlbiS2s8FEAIQBQ"; break;
            case 5: boardID = "CgkIlbiS2s8FEAIQAQ"; break;//sign in
            case 6: boardID = "CgkIlbiS2s8FEAIQCg"; break;//100 games
            case 7: boardID = "CgkIlbiS2s8FEAIQDA"; break;//250 games
            case 8: boardID = "CgkIlbiS2s8FEAIQDQ"; break;//150 kicks
            case 9: boardID = "CgkIlbiS2s8FEAIQDg"; break;//500 kicks
            case 10: boardID = "CgkIlbiS2s8FEAIQDw"; break;//1000 kicks
        }

        Social.ReportProgress(boardID, 100.0f, (bool success) =>
        {
        });
    }
}
