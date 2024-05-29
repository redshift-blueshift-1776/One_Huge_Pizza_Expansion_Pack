using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossEnemyLevel3 : MonoBehaviour
{
    [SerializeField] GameObject weapon;
    [SerializeField] int enemyType;
    // 0: pepperoni
    // 1: mushroom

    public EnemyState currState;
    public float range;
    public float moveSpeed;

    Rigidbody2D myRigidbody;

    public int HP;
    public int maxHP;
    Color og;
    Color transparent;

    public Transform player;

    private bool gotHit = false;
    GameObject newBullet;

    [SerializeField] GameObject bullet;
    [SerializeField] Sprite deadSprite;
    [SerializeField] Sprite normalSprite;
    [SerializeField] Sprite flippedSprite;

    public int phase;

    [SerializeField] GameObject bossHealthBar;

    public int xBound;
    bool direction;

    public bool isInvincible = false;

    bool startedPhase2 = false;
    bool startedPhase3 = false;

    public Transform[] spawnpoints;

    [SerializeField] GameObject pepperoni;
    [SerializeField] GameObject mushroom;
    [SerializeField] GameObject tomato;
    [SerializeField] GameObject graphQuestionAttack;

    [SerializeField] GameObject sceneSwitcher;

    [SerializeField] AudioSource audioController;

    public AudioClip phase2;
    public AudioClip phase3;

    [SerializeField] Sprite phase1Sprite;
    [SerializeField] Sprite phase2Sprite;
    [SerializeField] Sprite phase34Sprite;

    private AudioSource splat;

    private int saveFrame;

    // Start is called before the first frame update
    void Start()
    {
        //phase = 1;
        //maxHP = 4000;
        //HP = 4000;
        //moveSpeed = 5;
        //currState = EnemyState.Attack1;
        og = GetComponent<Renderer>().material.color;
        player = GameObject.Find("Player").transform;
        splat = GameObject.Find("Splat").GetComponent<AudioSource>();
        spawnpoints = GameObject.Find("Spawnpoints").transform.GetComponentsInChildren<Transform>();
        myRigidbody = GetComponent<Rigidbody2D>();
        saveFrame = Time.frameCount;
    }

    // Update is called once per frame
    void Update()
    {
        switch (currState)
        {
            case (EnemyState.Attack1):
                Attack1();
                break;
            case (EnemyState.Attack2):
                Attack2();
                break;
            case (EnemyState.Attack3):
                Attack3();
                break;
            case (EnemyState.Die):
                Die();
                break;
        }    

        if (HP <= 0) {
            currState = EnemyState.Die;
        }

        if (gotHit) {
            transform.position += new Vector3(Mathf.Sin(Time.time * 100f) * 0.03f, 0f, 0f);
        }
    }

    private void OnTriggerEnter2D(Collider2D c)
    {
        if (c.name == "WeaponBlade" && currState != EnemyState.Die && !isInvincible) {
            StartCoroutine(Hit());
            HP -= 20;
            bossHealthBar.GetComponent<BossHealth>().SetHealth(HP);
            splat.Play();
        } else if (c.name == "Player") {
            if (!player.GetComponent<PlayerMovement>().isInvincible) {
                player.GetComponent<PlayerMovement>().HP -= 1;
                player.GetComponent<PlayerMovement>().Hit();
            }
        }
    }

    IEnumerator Hit()
    { 
        gotHit = true;
        GetComponent<Renderer>().material.color = Color.red;
        yield return new WaitForSeconds(.1f);
        GetComponent<Renderer>().material.color = og;
        gotHit = false;
        yield return null;
    }

    void changePhase(int i) {
        phase = i;
    }

    private IEnumerator changeAttackPhase()
    {
        isInvincible = true;
        yield return new WaitForSeconds(.5f);
        Instantiate(pepperoni, spawnpoints[0]);
        Instantiate(pepperoni, spawnpoints[1]);
        Instantiate(pepperoni, spawnpoints[6]);
        Instantiate(pepperoni, spawnpoints[7]);
        isInvincible = false;
        yield return null;
    }

    private bool IsPlayerInRange(float range)
    {
        return Vector3.Distance(transform.position, player.transform.position) <= range;
    }

    bool isFacingRight()
    {
        return transform.localScale.x > 0;
    }

    void Attack1()
    {
        transform.Translate(Vector3.right * Time.deltaTime * moveSpeed);
        if (transform.position.x < -5) {
            moveSpeed = 5;
        } else if (transform.position.x > 5) {
            moveSpeed = -5;
        }
        int frameDif = Time.frameCount - saveFrame;
        if (frameDif >= 240) {
            if (frameDif % 60 == 30) {
                Vector3 vec = new Vector3(.05f, .05f, .05f);
                shootBullet(vec, new Vector3(0,-1,0), 8f);
                shootBullet(vec, new Vector3(0,1,0), 8f);
                shootBullet(vec, new Vector3(1,0,0), 8f);
                shootBullet(vec, new Vector3(-1,0,0), 8f);
            }
            if (frameDif % 60 == 0) {
                Vector3 vec = new Vector3(.05f, .05f, .05f);
                float d = Vector3.Distance(transform.position, player.transform.position);
                shootBullet(vec, new Vector3((player.transform.position.x - transform.position.x) / d,
                (player.transform.position.y - transform.position.y) / d,0), 8f);
            }
            if (frameDif % 840 == 0) {
                Instantiate(graphQuestionAttack, transform.position, transform.rotation);
                graphQuestionAttack.GetComponent<GraphQuestionEnemy>().framesToAttack = 240;
            }
        }
    }

    void changeAttack (int i) {
        switch (i)
        {
            case (1):
                currState = EnemyState.Attack1;
                break;
            case (2):
                currState = EnemyState.Attack2;
                break;
        }    
    }

    void shootBullet(Vector3 size, Vector3 bulletDirection, float speed) {
        newBullet = Instantiate(bullet, transform.position, transform.rotation);
        newBullet.transform.localScale = size;
        newBullet.GetComponent<Projectile>().setSource(transform);
        newBullet.GetComponent<Projectile>().startingPos = bulletDirection;
        newBullet.GetComponent<Projectile>().speed = speed;
    }

    private IEnumerator MoveObject (Transform thisTransform, Vector3 startPos, Vector3 endPos, float time) {
        float i = 0.0f;
        float rate = 1.0f / time;
        while (i < 1.0f) {
            i += Time.deltaTime * rate;
            thisTransform.position = Vector3.Lerp(startPos, endPos, i);
            yield return null;
        }
    }

    void Attack2()
    {
        if (phase == 1) {
            transform.Translate(Vector3.right * Time.deltaTime * moveSpeed);
            if (transform.position.x < -5) {
                moveSpeed = 5;
            } else if (transform.position.x > 5) {
                moveSpeed = -5;
            }
            if ((Time.frameCount + 0) % 120 == 0) {
                Vector3 vec = new Vector3(.05f, .05f, .05f);
                shootBullet(vec, new Vector3(1,0,0), 8f);
            }
            if ((Time.frameCount + 10) % 120 == 0) {
                Vector3 vec = new Vector3(.05f, .05f, .05f);
                shootBullet(vec, new Vector3(0.5f,-0.3f,0), 8f);
            }
            if ((Time.frameCount + 20) % 120 == 0) {
                Vector3 vec = new Vector3(.05f, .05f, .05f);
                shootBullet(vec, new Vector3(0.2f,-0.5f,0), 8f);
            }
            if ((Time.frameCount + 30) % 120 == 0) {
                Vector3 vec = new Vector3(.05f, .05f, .05f);
                shootBullet(vec, new Vector3(-0.2f,-0.5f,0), 8f);
            }
            if ((Time.frameCount + 40) % 120 == 0) {
                Vector3 vec = new Vector3(.05f, .05f, .05f);
                shootBullet(vec, new Vector3(-0.5f,-0.5f,0), 8f);
            }
            if ((Time.frameCount + 50) % 120 == 0) {
                Vector3 vec = new Vector3(.05f, .05f, .05f);
                shootBullet(vec, new Vector3(1,-0.5f,0), 8f);
            }
            if ((Time.frameCount + 60) % 120 == 0) {
                Vector3 vec = new Vector3(.05f, .05f, .05f);
                shootBullet(vec, new Vector3(-0.5f,-0.5f,0), 8f);
            }
            if ((Time.frameCount + 70) % 120 == 0) {
                Vector3 vec = new Vector3(.05f, .05f, .05f);
                shootBullet(vec, new Vector3(-0.2f,-0.5f,0), 8f);
            }
            if ((Time.frameCount + 80) % 120 == 0) {
                Vector3 vec = new Vector3(.05f, .05f, .05f);
                shootBullet(vec, new Vector3(0.2f,-0.5f,0), 8f);
            }
            if ((Time.frameCount + 90) % 120 == 0) {
                Vector3 vec = new Vector3(.05f, .05f, .05f);
                shootBullet(vec, new Vector3(0.5f,-0.3f,0), 8f);
            }
            if ((Time.frameCount + 100) % 120 == 0) {
                Vector3 vec = new Vector3(.05f, .05f, .05f);
                shootBullet(vec, new Vector3(0.7f,-0.2f,0), 8f);
            }
        }

        if (phase == 2) {
            transform.Translate(Vector3.up * Time.deltaTime * moveSpeed);
            if (transform.position.y < -5) {
                moveSpeed = 5;
            } else if (transform.position.y > 5) {
                moveSpeed = -5;
            }
        }

        if (phase == 3) {
            transform.Translate(Vector3.right * Time.deltaTime * moveSpeed);
            if (transform.position.x < -5) {
                moveSpeed = 3;
            } else if (transform.position.x > 5) {
                moveSpeed = -3;
            }
            if (Time.frameCount % 15 == 0) {
                if ((Time.frameCount % 270 == 0) || (Time.frameCount % 270 == 45) || (Time.frameCount % 270 == 90)
                    || (Time.frameCount % 270 == 120) || (Time.frameCount % 270 == 165) || (Time.frameCount % 270 == 210) || (Time.frameCount % 270 == 240)) {
                    Vector3 vec = new Vector3(.05f, .05f, .05f);
                    shootBullet(vec, new Vector3(0,-1,0), 2f);
                    shootBullet(vec, new Vector3(0,1,0), 2f);
                    shootBullet(vec, new Vector3(1,0,0), 2f);
                    shootBullet(vec, new Vector3(-1,0,0), 2f);
                    shootBullet(vec, new Vector3(-1,-1,0), 2f);
                    shootBullet(vec, new Vector3(1,1,0), 2f);
                    shootBullet(vec, new Vector3(1,-1,0), 2f);
                    shootBullet(vec, new Vector3(-1,1,0), 2f);
                } else {
                    Vector3 vec = new Vector3(.05f, .05f, .05f);
                    float d = Vector3.Distance(transform.position, player.transform.position);
                    shootBullet(vec, new Vector3((player.transform.position.x - transform.position.x) / d,
                    (player.transform.position.y - transform.position.y) / d,0), 9f); //change to point at player
                }
            }
        }

        if (phase % 2 == 1) {
            if (Time.frameCount % 540 == 0) {
                changeAttack(1);
            }
        } else {
            if (Time.frameCount % 480 == 0) {
                changeAttack(1);
            }
        }
    }

    void Attack3()
    {
        if (phase == 2) {

        }
    }
    void Die() {
        StartCoroutine(DieCoroutine());
    }

    private IEnumerator DieCoroutine()
    {
        myRigidbody.velocity = new Vector2(0f, 0f);
        Debug.Log("test");
        player.GetComponent<PlayerMovement>().isInvincible = true;
        GetComponent<SpriteRenderer>().sprite = deadSprite;
        for (int i = 0; i < 5; i++) {
            GetComponent<Renderer>().material.color = transparent;
            yield return new WaitForSeconds(0.1f);
            GetComponent<Renderer>().material.color = og;
            yield return new WaitForSeconds(0.1f);
        }
        GetComponent<Renderer>().material.color = Color.red;
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(2);
        Destroy(gameObject);
        yield return null;
    }


}
