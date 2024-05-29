using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphQuestionAttack : MonoBehaviour
{
    public int framesToAttack;

    public Transform player;
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

    private AudioSource splat;
    // Start is called before the first frame update
    void Start()
    {
        og = GetComponent<Renderer>().material.color;
        player = GameObject.Find("Player").transform;
        splat = GameObject.Find("Splat").GetComponent<AudioSource>();
        transparent = new Color(og.r, og.g, og.b, 0.5f);
        Instantiate(enemy1, transform.position, transform.rotation);
        Instantiate(enemy2, transform.position, transform.rotation);
        Instantiate(enemy3, transform.position, transform.rotation);
        Instantiate(enemy4, transform.position, transform.rotation);
        enemy1.GetComponent<GraphQuestionEnemy>().framesToAttack = framesToAttack;
        enemy2.GetComponent<GraphQuestionEnemy>().framesToAttack = framesToAttack;
        enemy3.GetComponent<GraphQuestionEnemy>().framesToAttack = framesToAttack;
        enemy4.GetComponent<GraphQuestionEnemy>().framesToAttack = framesToAttack;
        int offset = 1;
        Vector3 top = new Vector3(player.position.x, (float) player.position.y + offset, 0f);
        Vector3 right = new Vector3((float) player.position.x + offset, player.position.y, 0f);
        Vector3 bottom = new Vector3(player.position.x, (float) player.position.y - offset, 0f);
        Vector3 left = new Vector3((float) player.position.x - offset, player.position.y, 0f);
        if (permutation <= 6) {
            enemy1.GetComponent<GraphQuestionEnemy>().goToPosition = top;
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

    private void OnTriggerEnter2D(Collider2D c)
    {
        if (c.name == "Player") {
            if (!player.GetComponent<PlayerMovement>().isInvincible) {
                player.GetComponent<PlayerMovement>().Hit();
                player.GetComponent<PlayerMovement>().HP -= 1;
                Destroy(gameObject);
            }
            Destroy(gameObject);
        }
        //Debug.Log("hit");
    }

    void shootBullet(Vector3 size, Vector3 bulletDirection, float speed) {
        newBullet = Instantiate(bullet, transform.position, transform.rotation);
        newBullet.transform.localScale = size;
        newBullet.GetComponent<Projectile>().setSource(transform);
        newBullet.GetComponent<Projectile>().startingPos = bulletDirection;
        newBullet.GetComponent<Projectile>().speed = speed;
    }

    public IEnumerator MoveOverSeconds (GameObject objectToMove, Vector3 end, float seconds)
    {
        float elapsedTime = 0;
        Vector3 startingPos = objectToMove.transform.position;
        while (elapsedTime < seconds)
        {
            objectToMove.transform.position = Vector3.Lerp(startingPos, end, (elapsedTime / seconds));
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        objectToMove.transform.position = end;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
