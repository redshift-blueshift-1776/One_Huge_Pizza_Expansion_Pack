using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed;

    GameObject player;
    Transform enemy;
    public Vector3 startingPos;

    public int bulletType;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        //startingPos = player.transform.position - enemy.position;
        //if (player.transform.position.x < enemy.position.x) {
        //    startingPos = new Vector3(1 * startingPos.x, startingPos.y, 0);
        //}
    }

    public void setSource(Transform t) {
        enemy = t;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(startingPos * Time.deltaTime * speed);
        StartCoroutine(waitAndDespawn());
    }

    private IEnumerator waitAndDespawn() {
        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
        yield return null;
    }

    private void OnTriggerEnter2D(Collider2D c)
    {
        if (c.name == "Central Barrier") {
            Destroy(gameObject);
        } else if (c.name == "1HP_MOZZARELLA2") {
            Destroy(gameObject);
        } else if (c.name == "1HP_MOZZARELLA2 (1)") {
            Destroy(gameObject);
        } else if (c.name == "1HP_MOZZARELLA2 (2)") {
            Destroy(gameObject);
        } else if (c.name == "Player") {
            if (!player.GetComponent<PlayerMovement>().isInvincible) {
                player.GetComponent<PlayerMovement>().Hit();
                player.GetComponent<PlayerMovement>().HP -= 1;
                Destroy(gameObject);
            }
            Destroy(gameObject);
        }
        //Debug.Log("hit");
    }
}
