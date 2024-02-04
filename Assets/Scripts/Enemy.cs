using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState
{
  Wander,
  Follow,
  Die,
  Attack1,
  Attack2,
  Attack3
};

public class Enemy : MonoBehaviour
{
    [SerializeField] GameObject weapon;
    [SerializeField] int enemyType;
    // 0: pepperoni
    // 1: mushroom

    public EnemyState currState = EnemyState.Wander;
    public float range = 200f;
    public float moveSpeed = 1f;

    Rigidbody2D myRigidbody;

    public int HP;
    Color og;
    Color transparent;

    private bool chooseDir = false;
    public int randomDir;

    public Transform player;

    private bool gotHit = false;
    GameObject newBullet;
    float storeFrame;

    [SerializeField] GameObject bullet;
    [SerializeField] Sprite deadSprite;
    [SerializeField] Sprite normalSprite;
    [SerializeField] Sprite flippedSprite;

    // Start is called before the first frame update
    void Start()
    {
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
            case (EnemyState.Wander):
                Wander();
                break;
            case (EnemyState.Follow):
                Follow();
                break;
            case (EnemyState.Die):
                Die();
                break;
        }    

        if(IsPlayerInRange(range) && currState != EnemyState.Die)
        {
            if (currState != EnemyState.Follow) {
                storeFrame = Time.frameCount;
            }

            if (enemyType != 3) {
                currState = EnemyState.Follow;
            } else {
                currState = EnemyState.Wander;
            }
        }
        else if(!IsPlayerInRange(range)&& currState != EnemyState.Die)
        {
            currState = EnemyState.Wander;
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
        if (c.name == "WeaponBlade" && currState != EnemyState.Die) {
            StartCoroutine(Hit());
            HP -= 10;
        } else if (c.name == "Player" && currState != EnemyState.Die) {
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

    void Wander()
    {
        if (!chooseDir)
        {
            StartCoroutine(ChooseDirection());
        }

        if (enemyType == 3) {
            if ((Time.frameCount + storeFrame) % 120 == 0) {
                Vector3 vec = new Vector3(.05f, .05f, .05f);
                shootBullet(vec, new Vector3(-1,1,0), 3f);
                shootBullet(vec, new Vector3(1,1,0), 3f);
                shootBullet(vec, new Vector3(1,-1,0), 3f);
                shootBullet(vec, new Vector3(-1,-1,0), 3f);
            }
        }

        switch(randomDir)
        {
            case 8:
            GetComponent<SpriteRenderer>().sprite = flippedSprite;
            transform.position += -transform.right * moveSpeed * Time.deltaTime;
            transform.position += -transform.up * moveSpeed * Time.deltaTime;
            break;
            
            case 7:
            GetComponent<SpriteRenderer>().sprite = flippedSprite;
            transform.position += -transform.right * moveSpeed * Time.deltaTime;
            transform.position += transform.up * moveSpeed * Time.deltaTime;
            break;
            
            case 6:
            GetComponent<SpriteRenderer>().sprite = normalSprite;
            transform.position += transform.right * moveSpeed * Time.deltaTime;
            transform.position += -transform.up * moveSpeed * Time.deltaTime;
            break;
            
            case 5:
            GetComponent<SpriteRenderer>().sprite = normalSprite;
            transform.position += transform.right * moveSpeed*Time.deltaTime;
            transform.position += transform.up * moveSpeed * Time.deltaTime;
            break;
            
            case 4:
            GetComponent<SpriteRenderer>().sprite = normalSprite;
            transform.position += transform.up * moveSpeed*Time.deltaTime;
            break;
            
            case 3:
            GetComponent<SpriteRenderer>().sprite = normalSprite;
            transform.position += transform.up * moveSpeed *Time.deltaTime;
            break;
            
            case 2:
            GetComponent<SpriteRenderer>().sprite = flippedSprite;
            transform.position += -transform.right * moveSpeed*Time.deltaTime;
            break;
            
            case 1:
            GetComponent<SpriteRenderer>().sprite = normalSprite;
            transform.position += transform.right * moveSpeed*Time.deltaTime;
            break;
            
            default:
            // Insert a fail-safe code to insure something happens
            // Maybe pick a new random direction?
            break;
        }
    }

    void Follow()
    {
        if (transform.position.x > player.position.x)
        {
            //target is left
            GetComponent<SpriteRenderer>().sprite = flippedSprite;
            myRigidbody.velocity = new Vector2(-moveSpeed, 0f);
            transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
        }
        else if (transform.position.x < player.position.x)
        {
            //target is right
            GetComponent<SpriteRenderer>().sprite = normalSprite;
            myRigidbody.velocity = new Vector2(moveSpeed, 0f);
            transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
        }
        
        if (enemyType == 0) {
            if ((Time.frameCount + storeFrame) % 120 == 0) {
                Vector3 vec = new Vector3(.05f, .05f, .05f);
                shootBullet(vec, player.position, 3f);
            }
        } else if (enemyType == 1) {
            if ((Time.frameCount + storeFrame) % 120 == 0) {
                Vector3 vec = new Vector3(.05f, .05f, .05f);
                shootBullet(vec, player.position, 3f);
                shootBullet(vec, new Vector3(player.position.x, (float) player.position.y + 1, 0f), 3f);
                shootBullet(vec, new Vector3(player.position.x, (float) player.position.y - 1, 0f), 3f);
            }
        }
    }

    void shootBullet(Vector3 size, Vector3 bulletDirection, float speed) {
        newBullet = Instantiate(bullet, transform.position, transform.rotation);
        newBullet.transform.localScale = size;
        newBullet.GetComponent<Projectile>().setSource(transform);
        newBullet.GetComponent<Projectile>().startingPos = bulletDirection;
        newBullet.GetComponent<Projectile>().speed = speed;
    }

    void Die() {
        StartCoroutine(DieCoroutine());
    }

    private IEnumerator DieCoroutine()
    {
        myRigidbody.velocity = new Vector2(0f, 0f);
        GetComponent<SpriteRenderer>().sprite = deadSprite;
        for (int i = 0; i < 5; i++) {
            GetComponent<Renderer>().material.color = transparent;
            yield return new WaitForSeconds(0.1f);
            GetComponent<Renderer>().material.color = og;
            yield return new WaitForSeconds(0.1f);
        }
        GetComponent<Renderer>().material.color = Color.red;
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
        yield return null;
    }


    private IEnumerator ChooseDirection()
    {
        chooseDir = true;
        yield return new WaitForSeconds(Random.Range(1f, 3f));
        randomDir = Random.Range(1, 8);
        chooseDir = false;
    }

}
