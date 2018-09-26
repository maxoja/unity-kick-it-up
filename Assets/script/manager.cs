using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class manager : MonoBehaviour
{
    public class FunctionCall
    {
        public float timeLeft = -1;
        public int methodID = -1;

        public FunctionCall(int mid, float t)
        {
            timeLeft = t;
            methodID = mid;
        }
    }

    /*
     * 1 = launchSingleBall
     * 2 = launchDoubleBall
     * 3 = launchTrippleBall
     * 4 = launchGame
     * 5 = addball
     * 6 = removeball
     * 7 = createWhistle
    */

    public enum SceneMode
    {
        Game = 's',
        Pause = 'p',
        Menu = 'm',
    }

    static public List<FunctionCall> fCallList = new List<FunctionCall>();

    static public float topScreenPos, buttomScreenPos, edgeScreenPos;

    static public List<GameObject> ballList = new List<GameObject>(3);
    static public List<GameObject> whistleList = new List<GameObject>();
    static public int maxWhistle = 1;
    static public int score = 0;
    static public int ballHit = 0;
    static public int combo = 0;
    static public int level = 1;
    static public int maxLevel = 3;//9
    static public int ballCt = 1;
    //static public GameObject LastBallHit;
    //static public int[] exp = new int[4] { 0, 2, 4, 94 };
    static public int[] exp = new int[4] { 0, 60, 180, 9999 };//new int[10]{0,30,90,180,205,255,330,355,405,999999};
    static public float[] stageKickZone = new float[10] { 0, 4, 4.75f, 5.5f, 3, 3.75f, 4.5f, 2, 2.75f, 3.5f };
    static public float g = 0.2f;
    static public SceneMode mode = SceneMode.Menu;//s = start | m = menu | p = pause
    static public char modeSub = '1';

    static public GameObject ballPref;
    static public GameObject startBallPref;
    static public GameObject forceUpEffect;
    static public GameObject whistlePrefs;

    static public float kickZone = 0;

    static public Color bgColor;
    static public Color floorColor;
    static public Color[] stageColor = new Color[3] { new Color(166f / 255f, 188f / 255f, 106f / 255f), new Color(206f / 255f, 198f / 255f, 106f / 255f), new Color(106f / 255f, 188f / 255f, 181f / 255f) };
    static public Color[] stageFloorColor = new Color[3] { new Color(99f / 255f, 200f / 255f, 0), new Color(229f / 255f, 93f / 255, 0), new Color(0, 157f / 255f, 171f / 255f) };
    static public bool transStage = false;
    static public int stageID = 0;

    static public float startTime;

    static public bool pause = false;
    bool dataInStored = false;
    Vector2[] tempVelo = new Vector2[3];

    void Awake()
    {
        Shader.WarmupAllShaders();
        bgColor = stageColor[0];
        floorColor = stageFloorColor[0];

        topScreenPos = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height, 0)).y;
        buttomScreenPos = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0)).y;
        edgeScreenPos = Mathf.Abs(Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0)).x);
    }

    void Start()
    {
        ballPref = Resources.Load("prefab/ball") as GameObject;
        startBallPref = Resources.Load("prefab/startBall") as GameObject;
        forceUpEffect = Resources.Load("prefab/forceUp") as GameObject;
        whistlePrefs = Resources.Load("prefab/whistle") as GameObject;
        warmParticle();

        LaunchMenu();
    }

    static public void Update_TouchCheck()
    {
        if (pause)
            return;

        if (Input.touchCount > 0)
        {
            touchCheck_Touch();
            return;
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Vector2 mFloorPos = Input.mousePosition;
            mFloorPos.y = 0;

            ForceUpEffect(mFloorPos);
            touchCheck_Mouse();
        }
    }
    static public void touchCheck_Touch()
    {
        List<int> newTouchIndex = new List<int>();
        for (int ct1 = 0; ct1 < Input.touchCount; ct1++)
        {
            if (Input.touches[ct1].phase == TouchPhase.Began)
            {
                Vector2 tFLoorPos = Input.touches[ct1].position;
                tFLoorPos.y = 0;

                ForceUpEffect(tFLoorPos);
                newTouchIndex.Add(ct1);
            }
        }

        for (int ct1 = 0; ct1 < newTouchIndex.Count; ct1++)
        {
            bool[] hash = new bool[ballList.Count];
            float[] sins = new float[hash.Length];
            float[] coss = new float[hash.Length];

            for (int ct2 = 0; ct2 < ballList.Count; ct2++)
            {
                hash[ct2] = false;

                GameObject bo = ballList[ct2];
                ballPhysics bp = bo.GetComponent<ballPhysics>();

                Vector3 mouseV3 = Camera.main.ScreenToWorldPoint(Input.touches[newTouchIndex[ct1]].position) - new Vector3(0, 0, Camera.main.ScreenToWorldPoint(Input.touches[newTouchIndex[ct1]].position).z);

                Vector3 pos = bo.transform.position;
                float dist = Vector3.Distance(pos, mouseV3);
                float dx = pos.x - mouseV3.x;
                float dy = pos.y - mouseV3.y;

                if (bp.startBall)
                {
                    if (dist < bp.radius * 2)
                    {
                        LaunchGame(PlayerPrefs.GetInt("lastGameType") + 1);
                        return;
                    }
                }
                else
                {
                    if (bo.transform.position.y < manager.kickZone + bp.radius)
                    {
                        if (Mathf.Abs(dx) <= bp.radius * (1 + (bo.GetComponent<Rigidbody2D>().gravityScale)))// && mouseV3.y < kickZone)
                        {
                            hash[ct2] = true;
                            coss[ct2] = Mathf.Abs(dy / dist);
                            sins[ct2] = dx / dist;
                        }
                    }
                }
            }

            int hitDex = -1;
            float lowest = 100;
            for (int ct2 = 0; ct2 < ballList.Count; ct2++)
            {
                if (hash[ct2])
                {
                    if (ballList[ct2].transform.position.y < lowest)
                    {
                        lowest = ballList[ct2].transform.position.y;
                        hitDex = ct2;
                    }
                }
            }

            if (hitDex != -1)
                ballList[hitDex].GetComponent<ballPhysics>().Kick(sins[hitDex], coss[hitDex]);
        }
    }
    static public void touchCheck_Mouse()
    {
        bool[] hash = new bool[ballList.Count];
        float[] sins = new float[hash.Length];
        float[] coss = new float[hash.Length];

        Vector3 mouseV3 = Camera.main.ScreenToWorldPoint(Input.mousePosition) - new Vector3(0, 0, Camera.main.ScreenToWorldPoint(Input.mousePosition).z);

        for (int ct1 = 0; ct1 < ballList.Count; ct1++)
        {
            GameObject bo = ballList[ct1];
            ballPhysics bp = bo.GetComponent<ballPhysics>();

            Vector3 pos = bo.transform.position;
            float dist = Vector3.Distance(pos, mouseV3);
            float dx = pos.x - mouseV3.x;
            float dy = pos.y - mouseV3.y;

            if (bp.startBall)
            {
                if (dist < bp.radius * 2)
                {
                    LaunchGame(PlayerPrefs.GetInt("lastGameType") + 1);
                    return;
                }
            }
            else
            {
                if (bo.transform.position.y < manager.kickZone + bp.radius)
                {
                    if (Mathf.Abs(dx) <= bp.radius * (1 + (bo.GetComponent<Rigidbody2D>().gravityScale)))// && mouseV3.y < kickZone)
                    {
                        hash[ct1] = true;
                        coss[ct1] = Mathf.Abs(dy / dist);
                        sins[ct1] = dx / dist;
                    }
                }
            }
        }

        int hitDex = -1;
        float lowest = 100;
        for (int ct1 = 0; ct1 < ballList.Count; ct1++)
        {
            if (hash[ct1])
            {
                if (ballList[ct1].transform.position.y < lowest)
                {
                    lowest = ballList[ct1].transform.position.y;
                    hitDex = ct1;
                }
            }
        }

        if (hitDex != -1)
            ballList[hitDex].GetComponent<ballPhysics>().Kick(sins[hitDex], coss[hitDex]);
    }

    void Update_FunctionCall()
    {
        for (int ct1 = 0; ct1 < fCallList.Count; ct1++)
        {
            fCallList[ct1].timeLeft -= Time.deltaTime;

            if (fCallList[ct1].timeLeft <= 0)
            {
                if (fCallList[ct1].methodID <= 4)
                {
                    LaunchGame(fCallList[ct1].methodID);
                }
                else
                    switch (fCallList[ct1].methodID)
                    {
                        case 5: addBall(); break;
                            //case 6: removeBall(1); break;
                    }

                fCallList.RemoveAt(ct1);
            }
        }
    }

    static public void Update_InGame()
    {
        if (mode != SceneMode.Game)
        {
            ballPhysics.size = 1;
            if (fCallList.Count > 0)
                fCallList.Clear();
            return;
        }

        if (modeSub == '3' || ballList.Count == 3)
            ballPhysics.size = 0.5f;
        else
            ballPhysics.size = 1;

        if (whistleList.Count < maxWhistle)
        {
            addWhistle();
        }
    }

    void Update()
    {
        if (Input.GetKeyDown("escape"))
        {
            if (manager.mode == SceneMode.Game)
                manager.pause = !manager.pause;

            if (manager.mode == SceneMode.Menu)
            {
                if (YellowCard.IsShowing())
                    YellowCard.Hide();
                else
                    Application.Quit();
            }
        }

        Update_InGame();

        Update_FunctionCall();
        Update_TouchCheck();

        if (pause)
        {
            if (dataInStored == false)
            {
                for (int ct = 0; ct < 3; ct++)
                {
                    if (ballList.Count >= ct + 1)
                    {
                        tempVelo[ct] = ballList[ct].GetComponent<Rigidbody2D>().velocity;
                        ballList[ct].GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                        ballList[ct].GetComponent<Rigidbody2D>().gravityScale = 0;
                    }
                }

                dataInStored = true;
            }
        }
        else
        {
            if (dataInStored)
            {
                for (int ct = 0; ct < 3; ct++)
                {
                    if (ballList.Count >= ct + 1)
                    {
                        ballList[ct].GetComponent<Rigidbody2D>().velocity = tempVelo[ct];
                        ballList[ct].GetComponent<Rigidbody2D>().gravityScale = g;
                    }
                }
            }
            dataInStored = false;
        }

        if (Input.touchCount >= 5)
            StopGame();

        if (transStage == true)
        {
            StartCoroutine(Transition());
            transStage = false;
        }
    }

    void OnApplicationPause(bool p)
    {
        if (p && mode == SceneMode.Game)
            pause = true;
    }

    static public void HitBall()
    {
        int hitCountInLevel = ballHit - exp[level - 1];

        if (modeSub == '4')
        {
            maxWhistle = level;
            if (level <= 3)
            {
                if (level == 1)
                {
                    g = 0.5f + Mathf.Sqrt(4 / 60f * (float)hitCountInLevel);

                    if (g <= 0.5f)
                        g = 0.5f;
                }

                if (level == 2)
                {
                    g = 0.2f + Mathf.Sqrt(4 / 120f * (float)hitCountInLevel);

                    if (g <= 0.2f)
                        g = 0.2f;
                }

                if (level == 3)
                    g = 0.1f + Mathf.Sqrt(1.5f * 1.5f / 250 * (float)hitCountInLevel);
                /*original
                if ((level - 1) % 3 != 2)
                {
                    g = Mathf.Sqrt(0.14f * (float)hitCountInLevel / ballList.Count);//max = 2
                }
                else
                {
                    g = Mathf.Sqrt(1f/90f * (float)hitCountInLevel);//max = 1
                }
                 */
            }
            else
            {
                if ((level - 1) % 3 != 2)
                {
                    g = Mathf.Sqrt(0.14f * (float)hitCountInLevel / ballList.Count);//max = 2
                }
                else
                {
                    //g = 0.1f + hitCountInLevel * 0.9f/30;
                    g = Mathf.Sqrt(1f / 90f * (float)hitCountInLevel);//max = 1
                }
            }
        }
        else
        {
            if (modeSub == '1')
            {
                g = 0.2f + Mathf.Sqrt(1.8f * 1.8f / 150f * (float)hitCountInLevel);
            }

            if (modeSub == '2')
            {
                g = 0.2f + Mathf.Sqrt(1.8f * 1.8f / 300f * (float)hitCountInLevel);
            }

            if (modeSub == '3')
            {
                g = 0.1f + Mathf.Sqrt(0.9f * 0.9f / 450f * (float)hitCountInLevel);
            }
        }

        ballHit++;
        score += combo;

        if (modeSub == '4')
        {
            if (level < maxLevel)
                if (ballHit == exp[level])
                {
                    g = 0.1f;

                    level++;
                    transStage = true;
                    stageID = (level - 1);
                    kickZone = stageKickZone[level];

                    if ((level - 1) % 3 != 0)//minor lv up
                    {
                        addBall();
                    }
                    else                    //major lv up
                    {
                        //removeBall(2);
                    }

                    if (level == 4)
                        ggManager.UnlockAchievement(4);
                }
        }
        else
        {
            if (ballHit >= 25 * ballList.Count)
            {
                if (stageID == 0)
                {
                    maxWhistle = 2;

                    stageID = 1;
                    transStage = true;
                }
            }

            if (ballHit >= (75) * ballList.Count)
            {
                if (stageID == 1)
                {
                    maxWhistle = 3;

                    stageID = 2;
                    transStage = true;

                    if (int.Parse(modeSub.ToString()) > PlayerPrefs.GetInt("clear"))
                        PlayerPrefs.SetInt("clear", int.Parse(modeSub.ToString()));
                }
            }
        }

        SetGravity();

        Flash();
    }

    //SCENE FUNCTION
    static public void LaunchMenu()
    {
        if (Social.Active.localUser.authenticated)
        {
            ggManager.UnlockAchievement(5);
        }
        mode = SceneMode.Menu;

        stageID = 0;
        whistleList = new List<GameObject>();
        maxWhistle = 1;
        bgColor = stageColor[0];
        floorColor = stageFloorColor[0];
        g = 0.4f;
        kickZone = stageKickZone[0];
        level = 1;
        ballCt = 1;
        GameObject newBall = Instantiate(startBallPref, new Vector3(0, 7.25f, 0), Quaternion.identity) as GameObject;
        ballList.Add(newBall);
    }
    static public void LaunchGame(int n)
    {
        PlayerPrefs.SetInt("gamePlayed", PlayerPrefs.GetInt("gamePlayed") + 1);
        if (PlayerPrefs.GetInt("gamePlayed") >= 100)
            ggManager.UnlockAchievement(6);
        if (PlayerPrefs.GetInt("gamePlayed") >= 250)
            ggManager.UnlockAchievement(7);

        YellowCard.Hide();
        PlayerPrefs.SetInt("lastGameType", n - 1);

        startTime = Time.time;

        combo = 1;
        mode = SceneMode.Game;
        ballList[0].GetComponent<ballPhysics>().StartKick();

        if (n == 4)
        {
            modeSub = '4';
            maxLevel = 3;
            g = 0.2f;
            kickZone = stageKickZone[1];
        }

        if (n == 1)
        {
            modeSub = '1';
            maxLevel = 1;
            g = 0.2f;
            kickZone = stageKickZone[1];
        }


        if (n == 2)
        {
            fCallList.Add(new FunctionCall(5, 3));

            modeSub = '2';
            maxLevel = 1;
            g = 0.1f;
            kickZone = stageKickZone[2];
        }

        if (n == 3)
        {
            fCallList.Add(new FunctionCall(5, 3));
            fCallList.Add(new FunctionCall(5, 5));

            modeSub = '3';
            maxLevel = 1;
            g = 0.1f;
            kickZone = stageKickZone[3];
        }

        button.enable = false;
        SetGravity();
    }
    static public void UpdateHighScore()
    {
        int n = int.Parse(modeSub.ToString());

        if (score > PlayerPrefs.GetInt("highScore" + n.ToString()))
            PlayerPrefs.SetInt("highScore" + n.ToString(), score);

        ggManager.postScore(n);
    }
    static public void StopGame()
    {
        UpdateHighScore();
        if (Social.Active.localUser.authenticated)
        {
            for (int ct = 1; ct <= PlayerPrefs.GetInt("clear"); ct++)
                ggManager.UnlockAchievement(ct);
        }

        YellowCard.SetCardContent(int.Parse(modeSub.ToString()), ballHit, score, Time.time - startTime);
        YellowCard.Show();

        button.enable = true;
        modeSub = '1';

        stageID = 0;
        score = 0;
        combo = 0;
        ballHit = 0;
        level = 1;
        ballCt = 1;
        removeAllBall();
        removeAllWhistle();
        LaunchMenu();
    }

    //SYSTEM FUNCTION
    void warmParticle()
    {
        GameObject b = Instantiate(ballPref, Vector2.right * 500, Quaternion.identity) as GameObject;
        b.GetComponent<Rigidbody2D>().gravityScale = 0;
        Destroy(b.GetComponent<CircleCollider2D>());
        b.GetComponent<ballPhysics>().enabled = false;
        b.GetComponent<ParticleSystem>().Play();
    }
    static public void SetGravity()
    {
        for (int ct1 = 0; ct1 < ballList.Count; ct1++)
        {
            GameObject ball = ballList[ct1];

            float oldG = ball.GetComponent<Rigidbody2D>().gravityScale;

            ball.GetComponent<Rigidbody2D>().gravityScale = g;
            if (g < oldG)
                ball.GetComponent<Rigidbody2D>().velocity *= g / oldG;
        }
    }
    /*
    static public void removeBall(int n)
    {
        int dex = 0;

        while (n > 0)
        {
            if (ballList.Count == 0)
                break;

            if (ballList[dex] != LastBallHit)
            {
                ballList[dex].GetComponent<ballPhysics>().remove = true;
                ballList.RemoveAt(dex);

                n--;
            }
            else
            {
                dex++;
            }
        }
    }*/
    static public void removeAllBall()
    {
        foreach (GameObject b in ballList)
        {
            b.GetComponent<ballPhysics>().remove = true;
        }

        ballList.Clear();
    }
    static public void removeAllWhistle()
    {
        while (whistleList.Count > 0)
        {
            whistleList[0].GetComponent<whistle>().remove = true;
            whistleList.RemoveAt(0);
        }
    }
    static public void removeWhistle(GameObject wt)
    {
        whistleList.Remove(wt);
        wt.GetComponent<whistle>().remove = true;
    }
    static public void addBall()
    {
        float x = 0;

        if (ballList.Count == 1)
        {
            x = 1.5f;
        }

        if (ballList.Count == 2)
        {
            x = -1.5f;
        }

        ballCt++;
        GameObject newBall = Instantiate(ballPref, new Vector3(x, 11, 0), Quaternion.identity) as GameObject;

        ballList.Add(newBall);
    }
    static public void addWhistle()
    {
        int n = 30;

        float minDistBall;
        float minDistWhist;

        Vector3 pos;

        do
        {
            minDistBall = 1000000;
            minDistWhist = 1000000;
            pos = new Vector3(Random.Range(-edgeScreenPos + 0.5f, edgeScreenPos - 0.5f), topScreenPos - 1.5f, 0);

            foreach (GameObject g in ballList)
            {
                float dist = Vector3.Distance(g.transform.position, pos);
                if (dist < minDistBall)
                    minDistBall = dist;
            }

            foreach (GameObject g in whistleList)
            {
                float dist = Vector3.Distance(g.transform.position, pos);
                if (dist < minDistWhist)
                    minDistWhist = dist;
            }

            n--;
        }
        while ((minDistBall < 1.125f || minDistWhist < 0.9f) && n > 0);

        if (minDistBall >= 1.1f && minDistWhist >= 0.9f)
            whistleList.Add(Instantiate(whistlePrefs, pos, Quaternion.identity) as GameObject);
    }


    //EFFECT
    static public void Flash()
    {
        Camera.main.backgroundColor += Color.white * 0.1f;
        return;
    }
    IEnumerator Transition()
    {
        bgColor = Color.white;
        yield return new WaitForSeconds(0.5f);
        bgColor = stageColor[stageID % stageColor.Length];
        floorColor = stageFloorColor[stageID % stageFloorColor.Length];
    }
    static public void ForceUpEffect(Vector2 pos)
    {
        if (mode == SceneMode.Menu || pause)
            return;

        Vector3 mouseV3;

        if (Input.touchCount != 0)
            mouseV3 = Camera.main.ScreenToWorldPoint(pos) - new Vector3(0, 0, Camera.main.ScreenToWorldPoint(pos).z);
        else
            mouseV3 = Camera.main.ScreenToWorldPoint(pos) - new Vector3(0, 0, Camera.main.ScreenToWorldPoint(pos).z);

        fxManager.CallFunction(2, mouseV3 + Vector3.forward * 11, "");
    }
}
