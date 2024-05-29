using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphQuestionAttack : MonoBehaviour
{
    public int framesToAttack;

    public Transform player;
    //public GameObject player;
    Transform enemy;
    public Vector3 startingPos;

    public int permutation;

    [SerializeField] GameObject weapon;
    Color og;
    Color transparent;

    private bool gotHit = false;
    GameObject newBullet;
    float storeFrame;

    [SerializeField] GameObject bullet;
    [SerializeField] Sprite deadSprite;
    [SerializeField] GameObject enemy1;
    [SerializeField] GameObject enemy2;
    [SerializeField] GameObject enemy3;
    [SerializeField] GameObject enemy4;
    public Transform[] spawnpoints;

    private AudioSource splat;
    // Start is called before the first frame update
    void Start()
    {
        og = GetComponent<Renderer>().material.color;
        player = GameObject.Find("Player").transform;
        spawnpoints = GameObject.Find("Spawnpoints").transform.GetComponentsInChildren<Transform>();
        splat = GameObject.Find("Splat").GetComponent<AudioSource>();
        transparent = new Color(og.r, og.g, og.b, 0.5f);
        int offset = 5;
        Vector3 top = new Vector3(player.transform.position.x,
            (float) player.transform.position.y + offset, 0f);
        Vector3 right = new Vector3((float) player.transform.position.x + offset,
            player.transform.position.y, 0f);
        Vector3 bottom = new Vector3(player.transform.position.x,
            (float) player.transform.position.y - offset, 0f);
        Vector3 left = new Vector3((float) player.transform.position.x - offset,
            player.transform.position.y, 0f);
        Instantiate(enemy1, spawnpoints[7]);
        Instantiate(enemy2, spawnpoints[6]);
        Instantiate(enemy3, spawnpoints[2]);
        Instantiate(enemy4, spawnpoints[3]);
        enemy1.GetComponent<GraphQuestionEnemy>().framesToAttack = framesToAttack;
        enemy2.GetComponent<GraphQuestionEnemy>().framesToAttack = framesToAttack;
        enemy3.GetComponent<GraphQuestionEnemy>().framesToAttack = framesToAttack;
        enemy4.GetComponent<GraphQuestionEnemy>().framesToAttack = framesToAttack;
        enemy1.GetComponent<GraphQuestionEnemy>().attackNumber = 1;
        enemy2.GetComponent<GraphQuestionEnemy>().attackNumber = 2;
        enemy3.GetComponent<GraphQuestionEnemy>().attackNumber = 3;
        enemy4.GetComponent<GraphQuestionEnemy>().attackNumber = 4;
        if (permutation <= 6) {
            enemy1.GetComponent<GraphQuestionEnemy>().goToPosition = top;
            int remainder = permutation % 6;
            if (remainder == 0) {
                enemy2.GetComponent<GraphQuestionEnemy>().goToPosition = right;
                enemy3.GetComponent<GraphQuestionEnemy>().goToPosition = left;
                enemy4.GetComponent<GraphQuestionEnemy>().goToPosition = bottom;
                enemy1.GetComponent<GraphQuestionEnemy>().attackDirection = new Vector3(1, -1, 0);
                enemy2.GetComponent<GraphQuestionEnemy>().attackDirection = new Vector3(-2, 0, 0);
                enemy3.GetComponent<GraphQuestionEnemy>().attackDirection = new Vector3(1, -1, 0);
                enemy4.GetComponent<GraphQuestionEnemy>().attackDirection = new Vector3(0, 2, 0);
            } else if (remainder == 1) {
                enemy2.GetComponent<GraphQuestionEnemy>().goToPosition = right;
                enemy3.GetComponent<GraphQuestionEnemy>().goToPosition = bottom;
                enemy4.GetComponent<GraphQuestionEnemy>().goToPosition = left;
                enemy1.GetComponent<GraphQuestionEnemy>().attackDirection = new Vector3(1, -1, 0);
                enemy2.GetComponent<GraphQuestionEnemy>().attackDirection = new Vector3(-1, -1, 0);
                enemy3.GetComponent<GraphQuestionEnemy>().attackDirection = new Vector3(-1, 1, 0);
                enemy4.GetComponent<GraphQuestionEnemy>().attackDirection = new Vector3(1, 1, 0);
            } else if (remainder == 2) {
                enemy2.GetComponent<GraphQuestionEnemy>().goToPosition = bottom;
                enemy3.GetComponent<GraphQuestionEnemy>().goToPosition = right;
                enemy4.GetComponent<GraphQuestionEnemy>().goToPosition = left;
            } else if (remainder == 3) {
                enemy2.GetComponent<GraphQuestionEnemy>().goToPosition = bottom;
                enemy3.GetComponent<GraphQuestionEnemy>().goToPosition = left;
                enemy4.GetComponent<GraphQuestionEnemy>().goToPosition = right;
            } else if (remainder == 4) {
                enemy2.GetComponent<GraphQuestionEnemy>().goToPosition = left;
                enemy3.GetComponent<GraphQuestionEnemy>().goToPosition = right;
                enemy4.GetComponent<GraphQuestionEnemy>().goToPosition = bottom;
            } else if (remainder == 5) {
                enemy2.GetComponent<GraphQuestionEnemy>().goToPosition = left;
                enemy3.GetComponent<GraphQuestionEnemy>().goToPosition = bottom;
                enemy4.GetComponent<GraphQuestionEnemy>().goToPosition = right;
            }
        } else if (permutation <= 12) {
            enemy1.GetComponent<GraphQuestionEnemy>().goToPosition = right;
        } else if (permutation <= 18) {
            enemy1.GetComponent<GraphQuestionEnemy>().goToPosition = bottom;
        } else {
            enemy1.GetComponent<GraphQuestionEnemy>().goToPosition = left;
        }
    }

    public void setSource(Transform t) {
        enemy = t;
    }

    void shootBullet(Vector3 size, Vector3 bulletDirection, float speed) {
        newBullet = Instantiate(bullet, transform.position, transform.rotation);
        newBullet.transform.localScale = size;
        newBullet.GetComponent<Projectile>().setSource(transform);
        newBullet.GetComponent<Projectile>().startingPos = bulletDirection;
        newBullet.GetComponent<Projectile>().speed = speed;
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(waitAndDespawn());
    }

    private IEnumerator waitAndDespawn() {
        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
        yield return null;
    }
}
