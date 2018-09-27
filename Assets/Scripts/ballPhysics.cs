using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ballPhysics : MonoBehaviour 
{
    public bool startBall;
    public startCircle startCircle;

    public float radius;
    public float top,buttom,height;
    public float sVelo;
    public float acc;
    static public float size = 1;

    public Vector3 normalSize;

    public ballMesh mesh;
    public ballBg bg;
    private CircleCollider2D collid;

    public bool remove = false;

    private void Awake()
    {
        collid = GetComponent<CircleCollider2D>();
    }

    void Start ()
    {
        if (startBall)
        {
            GetComponent<Rigidbody2D>().gravityScale = 0;
            GetComponent<Rigidbody2D>().angularDrag = 0;
            mesh.angularVelo = -150;
        }
        else
        {
            mesh.angularVelo = Random.Range(-150, 150);
            GetComponent<Rigidbody2D>().gravityScale = manager.g;
        }
        acc = 0;//0.025f;

        transform.localScale = new Vector3(radius*2, radius*2, radius*2);

        top = 8.9f;// Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height, 0)).y - radius;
        buttom = 0.4f;//Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0)).y + radius;
        height = top - buttom;
        //all calculated with radius different
	}

	void Update ()
    {
        normalSize = new Vector3(radius * 2, radius * 2, radius * 2);
        transform.localScale = Vector3.Lerp(transform.localScale, normalSize * size,Time.deltaTime);

        if (startBall)
        {
            mesh.angularVelo = -150;
            GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        }

        if (manager.pause)
            return;

        if (remove)
        {
            RemoveBall();
            return;
        }

        //Left Edge
        if (Camera.main.WorldToScreenPoint(transform.position + Vector3.left * radius).x < 0 && GetComponent<Rigidbody2D>().velocity.x < 0)
        {
            soundManager.CallFunction(soundManager.Action.PlaySound, soundManager.SoundTag.FairKick);
            GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x * -1, GetComponent<Rigidbody2D>().velocity.y);
        }

        if (Camera.main.WorldToScreenPoint(transform.position + Vector3.right * radius).x > Screen.width && GetComponent<Rigidbody2D>().velocity.x > 0)
        {
            soundManager.CallFunction(soundManager.Action.PlaySound, soundManager.SoundTag.FairKick);
            GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x * -1, GetComponent<Rigidbody2D>().velocity.y);
        }

        if (Camera.main.WorldToScreenPoint(transform.position + Vector3.up * radius).y > Screen.height && GetComponent<Rigidbody2D>().velocity.y > 0)
        {
            soundManager.CallFunction(soundManager.Action.PlaySound, soundManager.SoundTag.FairKick);
            GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, GetComponent<Rigidbody2D>().velocity.y * -1);
        }

        //Buttom Edge
        if (transform.position.y < buttom && GetComponent<Rigidbody2D>().velocity.y < 0)
        {
            soundManager.CallFunction(soundManager.Action.PlaySound, soundManager.SoundTag.LongWhistle);
            GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x*0.5f, GetComponent<Rigidbody2D>().velocity.y * -0.5f);
            mesh.angularVelo *= 30;
            transform.position = new Vector3(transform.position.x, buttom, 0);

            manager.StopGame();
        }
	}

    void RemoveBall()
    {
        collid.isTrigger = true;

        GetComponent<Rigidbody2D>().isKinematic = true;
        GetComponent<Rigidbody2D>().gravityScale = 0;
        GetComponent<Rigidbody2D>().velocity = Vector2.zero; 

        if (mesh.sRend.color.r > 0.2f)
        {
            mesh.sRend.color = Color.Lerp(mesh.sRend.color, new Color(0.1f, 0.1f, 0.1f, 1), Time.deltaTime*4);
            mesh.transform.localScale *= 1.005f;
            return;
        }

        mesh.transform.localScale = Vector3.Lerp(mesh.transform.localScale, Vector3.zero, Time.deltaTime * 10);

        if (mesh.transform.localScale.x < 0.01f)
        {
            bg.transform.parent = null;
            bg.remove = true;

            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "ball")
        {
            soundManager.CallFunction(soundManager.Action.PlaySound, soundManager.SoundTag.FairKick);
        }
    }

    public void StartKick()
    {
        soundManager.CallFunction(soundManager.Action.PlaySound, soundManager.SoundTag.PerfectKick);
        
        startCircle.remove = true;

        GetComponent<Rigidbody2D>().gravityScale = manager.g;
        GetComponent<Rigidbody2D>().velocity = Vector2.up * 1;

        startBall = false;
    }

    public void Kick(float sin,float cos)
    {
            //manager.LastBallHit = gameObject;
            manager.HitBall();

            sVelo = Mathf.Sqrt(2 * GetComponent<Rigidbody2D>().gravityScale * 10 * (top - transform.position.y));

            mesh.angularVelo = 2500 * -sin * manager.g;

            if (Mathf.Abs(sin) < 0.4f)
            {
                GetComponent<ParticleSystem>().gravityModifier = manager.g;
                GetComponent<ParticleSystem>().Play();
                manager.combo++;

            soundManager.CallFunction(soundManager.Action.PlaySound, soundManager.SoundTag.PerfectKick);
            }
            else
            {
                manager.combo = 1;
            soundManager.CallFunction(soundManager.Action.PlaySound, soundManager.SoundTag.BallClashed);
            }

            GetComponent<Rigidbody2D>().velocity = new Vector2(sin, cos) * sVelo;

            if (manager.ballHit == 150)
                GooglePlayServiceManager.UnlockAchievement(8);
            if (manager.ballHit == 500)
                GooglePlayServiceManager.UnlockAchievement(9);
            if (manager.ballHit == 1000)
                GooglePlayServiceManager.UnlockAchievement(10);
    }
}
