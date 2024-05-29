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
    [SerializeField] Sprite enemy1;
    [SerializeField] Sprite enemy2;
    [SerializeField] Sprite enemy3;
    [SerializeField] Sprite enemy4;

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
        Vector3 center = player.transform.position;
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
