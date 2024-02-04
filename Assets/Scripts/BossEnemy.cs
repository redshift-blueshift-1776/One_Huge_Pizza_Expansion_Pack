using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemy : MonoBehaviour
{
    [SerializeField] GameObject weapon;
    [SerializeField] int enemyType;
    // 0: pepperoni
    // 1: mushroom

    public EnemyState currState;
    public float range = 200f;
    public float moveSpeed = 1f;

    Rigidbody2D myRigidbody;

    public int HP;
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

    // Start is called before the first frame update
    void Start()
    {
        phase = 1;
        currState = EnemyState.Attack1;
        og = GetComponent<Renderer>().material.color;
        player = GameObject.Find("Player").transform;
        myRigidbody = GetComponent<Rigidbody2D>();
        transparent = new Color(og.r, og.g, og.b, 0.5f);
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

        /*
        if(IsPlayerInRange(range) && currState != EnemyState.Die)
        {
            currState = EnemyState.Follow;
        }
        else if(!IsPlayerInRange(range)&& currState != EnemyState.Die)
        {
            currState = EnemyState.Wander;
        }
        */

        if (HP <= 0) {
            currState = EnemyState.Die;
        }

        if (gotHit) {
            transform.position += new Vector3(Mathf.Sin(Time.time * 100f) * 0.03f, 0f, 0f);
        }
    }

    private void OnTriggerEnter2D(Collider2D c)
    {
        if (c.name == "WeaponBlade" && currState != EnemyState.Die) {
            StartCoroutine(Hit());
            HP -= 10;
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
            if (Time.frameCount % 100 == 0) {
                newBullet = Instantiate(bullet, transform.position, transform.rotation);
                newBullet.GetComponent<Projectile>().setSource(transform);
                newBullet.GetComponent<Projectile>().bulletType = 0;
                newBullet = Instantiate(bullet, transform.position, transform.rotation);
                newBullet.GetComponent<Projectile>().setSource(transform);
                newBullet.GetComponent<Projectile>().bulletType = 1;
                newBullet = Instantiate(bullet, transform.position, transform.rotation);
                newBullet.GetComponent<Projectile>().setSource(transform);
                newBullet.GetComponent<Projectile>().bulletType = 2;
            }
        }
    }

    void Attack2()
    {
        if (phase == 1) {

        }
    }

    void Attack3()
    {
        if (phase == 1) {
            
        }
    }
    void Die() {
        StartCoroutine(DieCoroutine());
    }

    private IEnumerator DieCoroutine()
    {
        yield return null;
    }


}
