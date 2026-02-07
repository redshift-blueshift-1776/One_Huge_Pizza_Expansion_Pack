using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossEnemyLevel5 : MonoBehaviour
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
    bool startedPhase4 = false;

    public Transform[] spawnpoints;

    [SerializeField] GameObject pepperoni;
    [SerializeField] GameObject mushroom;
    [SerializeField] GameObject tomato;
    [SerializeField] GameObject LineAttack;

    [SerializeField] GameObject sceneSwitcher;

    [SerializeField] AudioSource audioController;

    public AudioClip phase2;
    public AudioClip phase3;
    public AudioClip phase4;

    [SerializeField] Sprite phase1Sprite;
    [SerializeField] Sprite phase2Sprite;
    [SerializeField] Sprite phase34Sprite;
    [SerializeField] Sprite phase34Sprite2;

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
        transparent = new Color(og.r, og.g, og.b, 0.5f);
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

        if ((float)HP / (float)maxHP <= 0.75f && !startedPhase2) {
            phase = 2;
            Debug.Log("phase 2 entered");
            StartCoroutine(changeAttackPhase());
            startedPhase2 = true;
            audioController.clip = phase2;
            audioController.Play();
            GetComponent<SpriteRenderer>().sprite = phase2Sprite;
        }

        if ((float)HP / (float)maxHP <= 0.50f && !startedPhase3) {
            phase = 3;
            Debug.Log("phase 3 entered");
            startedPhase3 = true;
            audioController.clip = phase3;
            audioController.Play();
            GetComponent<SpriteRenderer>().sprite = phase34Sprite;
        }

        if ((float)HP / (float)maxHP <= 0.25f && !startedPhase4) {
            phase = 4;
            Debug.Log("phase 4 entered");
            startedPhase4 = true;
            audioController.clip = phase4;
            audioController.Play();
            GetComponent<SpriteRenderer>().sprite = phase34Sprite2;
        }

        if (phase == 2) {

            if (Time.frameCount % 960 == 0) {
                Instantiate(pepperoni, spawnpoints[1]);
                Instantiate(pepperoni, spawnpoints[6]);
            }

            if ((Time.frameCount) % 1920 == 0) {
                Instantiate(pepperoni, spawnpoints[0]);
                Instantiate(pepperoni, spawnpoints[5]);
            }
        }

        if (phase == 4) {

            if (Time.frameCount % (1920 * 2) == 0) {
                Instantiate(pepperoni, spawnpoints[1]);
                Instantiate(pepperoni, spawnpoints[2]);
                Instantiate(pepperoni, spawnpoints[3]);
                Instantiate(pepperoni, spawnpoints[4]);
                Instantiate(pepperoni, spawnpoints[5]);
            }

            if ((Time.frameCount) % 1920 == 0) {
                Instantiate(pepperoni, spawnpoints[6]);
                Instantiate(pepperoni, spawnpoints[7]);
                Instantiate(pepperoni, spawnpoints[0]);
            }
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
        //Instantiate(tomato, spawnpoints[0]);
        //Instantiate(tomato, spawnpoints[1]);
        //Instantiate(tomato, spawnpoints[6]);
        //Instantiate(tomato, spawnpoints[7]);
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
        if (phase == 1) {
            transform.Translate(Vector3.right * Time.deltaTime * moveSpeed);
            if (transform.position.x < -5) {
                moveSpeed = 5;
            } else if (transform.position.x > 5) {
                moveSpeed = -5;
            }
            if (Time.frameCount % 240 == 0) {
                Vector3 vec = new Vector3(.05f, .05f, .05f);
                shootBullet(vec, new Vector3(0,-1,0), 5f);
                shootBullet(vec, new Vector3(-1,-1,0), 3f);
                shootBullet(vec, new Vector3(1,-1,0), 3f);
            }
            if (Time.frameCount % 60 == 0 || Time.frameCount % 60 == 15) {
                Vector3 vec = new Vector3(.05f, .05f, .05f);
                shootBullet(vec, new Vector3(-2,-2,0), 3f);
                shootBullet(vec, new Vector3(2,-2,0), 3f);
                shootBullet(vec, new Vector3(-1.5f,-2,0), 3f);
                shootBullet(vec, new Vector3(1.5f,-2,0), 3f);
            }
            if (Time.frameCount % 30 == 0) {
                Vector3 vec = new Vector3(.05f, .05f, .05f);
                shootBullet(vec, new Vector3(-1,0,0), 5f);
                shootBullet(vec, new Vector3(1,0,0), 5f);
            }
        } 

        if (phase == 2) {
            int frameDif = Time.frameCount - saveFrame;
            transform.Translate(Vector3.up * Time.deltaTime * moveSpeed);
            if (transform.position.y < -5) {
                moveSpeed = 5;
            } else if (transform.position.y > 5) {
                moveSpeed = -5;
            }
            if (frameDif % 60 == 0) {
                Instantiate(LineAttack, spawnpoints[Random.Range(1,8)]);
            }
            if ((frameDif) % 120 == 0) {
                Vector3 vec = new Vector3(.05f, .05f, .05f);
                shootBullet(vec, new Vector3(1,0,0), 5f);
                shootBullet(vec, new Vector3(0.8f,0.6f,0), 5f);
                shootBullet(vec, new Vector3(0.6f,0.8f,0), 5f);
                shootBullet(vec, new Vector3(0,1,0), 5f);
                shootBullet(vec, new Vector3(-0.6f,0.8f,0), 5f);
                shootBullet(vec, new Vector3(-0.8f,0.6f,0), 5f);
            }
            if ((frameDif) % 120 == 60) {
                Vector3 vec = new Vector3(.05f, .05f, .05f);
                shootBullet(vec, new Vector3(-1,0,0), 5f);
                shootBullet(vec, new Vector3(-0.8f,-0.6f,0), 5f);
                shootBullet(vec, new Vector3(-0.6f,-0.8f,0), 5f);
                shootBullet(vec, new Vector3(0,-1,0), 5f);
                shootBullet(vec, new Vector3(0.6f,-0.8f,0), 5f);
                shootBullet(vec, new Vector3(0.8f,-0.6f,0), 5f);
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
                    shootBullet(vec, new Vector3(0,-1,0), 9f);
                    shootBullet(vec, new Vector3(0,1,0), 9f);
                    shootBullet(vec, new Vector3(1,0,0), 9f);
                    shootBullet(vec, new Vector3(-1,0,0), 9f);
                    shootBullet(vec, new Vector3(-1,-1,0), 9f);
                    shootBullet(vec, new Vector3(1,1,0), 9f);
                    shootBullet(vec, new Vector3(1,-1,0), 9f);
                    shootBullet(vec, new Vector3(-1,1,0), 9f);
                } else {
                    Vector3 vec = new Vector3(.05f, .05f, .05f);
                    float d = Vector3.Distance(transform.position, player.transform.position);
                    shootBullet(vec, new Vector3((player.transform.position.x - transform.position.x) / d,
                    (player.transform.position.y - transform.position.y) / d,0), 6f); //change to point at player
                }
            }
        }

        if (phase == 4) {
            transform.Translate(Vector3.up * Time.deltaTime * moveSpeed);
            if (transform.position.y < -5) {
                moveSpeed = 3;
            } else if (transform.position.y > 5) {
                moveSpeed = -3;
            }
            if ((Time.frameCount % 120 == 0) || (Time.frameCount % 120 == 75) || (Time.frameCount % 120 == 90) || (Time.frameCount % 120 == 105)) {
                Vector3 vec = new Vector3(.05f, .05f, .05f);
                shootBullet(vec, new Vector3(0,-1,0), 9f);
                shootBullet(vec, new Vector3(0,1,0), 9f);
                shootBullet(vec, new Vector3(1,0,0), 9f);
                shootBullet(vec, new Vector3(-1,0,0), 9f);
                shootBullet(vec, new Vector3(-0.5f,-0.5f,0), 9f);
                shootBullet(vec, new Vector3(0.5f,0.5f,0), 9f);
                shootBullet(vec, new Vector3(0.5f,-0.5f,0), 9f);
                shootBullet(vec, new Vector3(-0.5f,0.5f,0), 9f);
                float d = Vector3.Distance(transform.position, player.transform.position);
                shootBullet(vec, new Vector3((player.transform.position.x - transform.position.x) / d,
                (player.transform.position.y - transform.position.y) / d,0), 6f); //change to point at player
            }
        }

        if (phase % 2 == 1) {
            if (Time.frameCount % 540 == 0) {
                changeAttack(2);
            }
        } else {
            if (Time.frameCount % 480 == 0) {
                changeAttack(2);
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
            if ((Time.frameCount + 0) % 240 == 0) {
                Vector3 vec = new Vector3(.05f, .05f, .05f);
                shootBullet(vec, new Vector3(1,0,0), 8f);
                shootBullet(vec, new Vector3(-1,0,0), 8f);
            }
            if ((Time.frameCount + 20) % 240 == 0) {
                Vector3 vec = new Vector3(.05f, .05f, .05f);
                shootBullet(vec, new Vector3(0.5f,-0.3f,0), 8f);
                shootBullet(vec, new Vector3(-0.5f,0.3f,0), 8f);
            }
            if ((Time.frameCount + 40) % 240 == 0) {
                Vector3 vec = new Vector3(.05f, .05f, .05f);
                shootBullet(vec, new Vector3(0.2f,-0.5f,0), 8f);
                shootBullet(vec, new Vector3(-0.2f,0.5f,0), 8f);
            }
            if ((Time.frameCount + 60) % 240 == 0) {
                Vector3 vec = new Vector3(.05f, .05f, .05f);
                shootBullet(vec, new Vector3(-0.2f,-0.5f,0), 8f);
                shootBullet(vec, new Vector3(0.2f,0.5f,0), 8f);
            }
            if ((Time.frameCount + 80) % 240 == 0) {
                Vector3 vec = new Vector3(.05f, .05f, .05f);
                shootBullet(vec, new Vector3(-0.5f,-0.5f,0), 8f);
                shootBullet(vec, new Vector3(0.5f,0.5f,0), 8f);
            }
            if ((Time.frameCount + 100) % 240 == 0) {
                Vector3 vec = new Vector3(.05f, .05f, .05f);
                shootBullet(vec, new Vector3(1,-0.5f,0), 8f);
                shootBullet(vec, new Vector3(-1,0.5f,0), 8f);
            }
            if ((Time.frameCount + 120) % 240 == 0) {
                Vector3 vec = new Vector3(.05f, .05f, .05f);
                shootBullet(vec, new Vector3(-0.5f,-0.5f,0), 8f);
                shootBullet(vec, new Vector3(0.5f,0.5f,0), 8f);
            }
            if ((Time.frameCount + 140) % 240 == 0) {
                Vector3 vec = new Vector3(.05f, .05f, .05f);
                shootBullet(vec, new Vector3(-0.2f,-0.5f,0), 8f);
                shootBullet(vec, new Vector3(0.2f,0.5f,0), 8f);
            }
            if ((Time.frameCount + 160) % 240 == 0) {
                Vector3 vec = new Vector3(.05f, .05f, .05f);
                shootBullet(vec, new Vector3(0.2f,-0.5f,0), 8f);
                shootBullet(vec, new Vector3(-0.2f,0.5f,0), 8f);
            }
            if ((Time.frameCount + 180) % 240 == 0) {
                Vector3 vec = new Vector3(.05f, .05f, .05f);
                shootBullet(vec, new Vector3(0.5f,-0.3f,0), 8f);
                shootBullet(vec, new Vector3(-0.5f,0.3f,0), 8f);
            }
            if ((Time.frameCount + 200) % 240 == 0) {
                Vector3 vec = new Vector3(.05f, .05f, .05f);
                shootBullet(vec, new Vector3(0.7f,-0.2f,0), 8f);
                shootBullet(vec, new Vector3(-0.7f,0.2f,0), 8f);
            }
        }

        if (phase == 2) {
            int frameDif = Time.frameCount - saveFrame;
            transform.Translate(Vector3.up * Time.deltaTime * moveSpeed);
            if (transform.position.y < -5) {
                moveSpeed = 5;
            } else if (transform.position.y > 5) {
                moveSpeed = -5;
            }
            if (frameDif % 60 == 0) {
                Instantiate(LineAttack, spawnpoints[Random.Range(1,8)]);
            }
            if ((frameDif) % 120 == 0) {
                Vector3 vec = new Vector3(.05f, .05f, .05f);
                shootBullet(vec, new Vector3(1,0,0), 5f);
                shootBullet(vec, new Vector3(0.8f,0.6f,0), 5f);
                shootBullet(vec, new Vector3(0.6f,0.8f,0), 5f);
                shootBullet(vec, new Vector3(-1,0,0), 5f);
                shootBullet(vec, new Vector3(-0.8f,-0.6f,0), 5f);
                shootBullet(vec, new Vector3(-0.6f,-0.8f,0), 5f);
            }
            if ((frameDif) % 120 == 60) {
                Vector3 vec = new Vector3(.05f, .05f, .05f);
                shootBullet(vec, new Vector3(0,1,0), 5f);
                shootBullet(vec, new Vector3(-0.6f,0.8f,0), 5f);
                shootBullet(vec, new Vector3(-0.8f,0.6f,0), 5f);
                shootBullet(vec, new Vector3(0,-1,0), 5f);
                shootBullet(vec, new Vector3(0.6f,-0.8f,0), 5f);
                shootBullet(vec, new Vector3(0.8f,-0.6f,0), 5f);
            }
            if ((Time.frameCount + 0) % 60 >= 45) {
            transform.Translate(Vector3.right * 2 * Time.deltaTime * moveSpeed);
            }
            if ((Time.frameCount + 0) % 60 < 15) {
                transform.Translate(Vector3.left * 2 * Time.deltaTime * moveSpeed);
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

        if (phase == 4) {
            transform.Translate(Vector3.up * Time.deltaTime * moveSpeed);
            if (transform.position.y < -5) {
                moveSpeed = 3;
            } else if (transform.position.y > 5) {
                moveSpeed = -3;
            }
            if ((Time.frameCount + 0) % 120 == 0) {
                Vector3 vec = new Vector3(.05f, .05f, .05f);
                shootBullet(vec, new Vector3(1,0,0), 5f);
                shootBullet(vec, new Vector3(0.8f,0.6f,0), 5f);
                shootBullet(vec, new Vector3(0.6f,0.8f,0), 5f);
                shootBullet(vec, new Vector3(0,1,0), 5f);
                shootBullet(vec, new Vector3(-0.6f,0.8f,0), 5f);
                shootBullet(vec, new Vector3(-0.8f,0.6f,0), 5f);
                shootBullet(vec, new Vector3(-1,0,0), 5f);
                shootBullet(vec, new Vector3(-0.8f,-0.6f,0), 5f);
                shootBullet(vec, new Vector3(-0.6f,-0.8f,0), 5f);
                shootBullet(vec, new Vector3(0,-1,0), 5f);
                shootBullet(vec, new Vector3(0.6f,-0.8f,0), 5f);
                shootBullet(vec, new Vector3(0.8f,-0.6f,0), 5f);
            }
            if ((Time.frameCount + 0) % 60 >= 45) {
            transform.Translate(Vector3.right * 2 * Time.deltaTime * moveSpeed);
            }
            if ((Time.frameCount + 0) % 60 < 15) {
                transform.Translate(Vector3.left * 2 * Time.deltaTime * moveSpeed);
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
        myRigidbody.linearVelocity = new Vector2(0f, 0f);
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
        SceneManager.LoadScene(19);
        Destroy(gameObject);
        yield return null;
    }


}
