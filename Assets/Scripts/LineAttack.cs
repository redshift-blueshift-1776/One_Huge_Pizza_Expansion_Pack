using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LineAttack : MonoBehaviour
{
    [SerializeField] GameObject weapon;
    public Transform player;
    public Transform boss;
    [SerializeField] GameObject bullet;
    [SerializeField] Sprite deadSprite;
    [SerializeField] Sprite normalSprite;
    [SerializeField] Sprite flippedSprite;

    Color og;
    Color transparent;

    private AudioSource splat;
    GameObject newBullet;
    private int storeFrame;
    private float rotation = 0;
    private float xstart;
    private float ystart;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").transform;
        boss = GameObject.Find("Boss").transform;
        //myRigidbody = GetComponent<Rigidbody2D>();
        splat = GameObject.Find("Splat").GetComponent<AudioSource>();
        og = GetComponent<Renderer>().material.color;
        transparent = new Color(og.r, og.g, og.b, 0.5f);
        storeFrame = Time.frameCount;
        //float old_rotation = rotation;
        rotation = UnityEngine.Random.Range(0f, 360f);
        transform.RotateAround(transform.position, Vector3.forward, rotation);
        //transform.LookAt(player);
        Vector3 vec = new Vector3(.05f, .05f, .05f);
        shootBullet(vec, transform.up, 5f);
        shootBullet(vec, -1 * transform.up, 5f);
        //rotation = (float) result[2];
        //MoveOverSeconds(gameObject, new Vector3((float) result[0], (float) result[1], 0f), 1f);
        //MoveOverSeconds(gameObject, new Vector3(xstart, ystart, 0f), 1f);
        //transform.position = Vector3.MoveTowards(transform.position, new Vector3(xstart, ystart, 0f), 100f * Time.deltaTime);
    }

    // Update is called once per frame
    void Update()
    {
        int frameDif = Time.frameCount - storeFrame;
        if (frameDif > 60) {
            if (frameDif % 6 == 0) {
                Vector3 vec = new Vector3(.05f, .05f, .05f);
                // shootBullet(vec, new Vector3((float) Math.Sin(Math.PI / 180 * rotation),
                //     - 1 * (float) Math.Cos(Math.PI / 180 * rotation), 0), 10f);
                // shootBullet(vec, new Vector3(-1 * (float) Math.Sin(Math.PI / 180 * rotation),
                //     (float) Math.Cos(Math.PI / 180 * rotation), 0), 10f);
                shootBullet(vec, transform.up, 10f);
                shootBullet(vec, -1 * transform.up, 10f);
            }
        }
        if (frameDif > 120) {
            Destroy(gameObject);
        }
    }

    public static double[] numbersForLineThing(float x1, float y1, float x2, float y2) {
        double[] ret = new double[4];
        double distance = findDistance(x1, y1, x2, y2);
        double angle = findAngle(x1, y1, x2, y2);
        ret[2] = angle + 30; //angle to point at
        ret[3] = angle - 60; //angle to go at
        ret[0] = x1 + distance / 2 * Math.Cos(Math.PI / 180 * ret[3]);
        ret[1] = y1 + distance / 2 * Math.Sin(Math.PI / 180 * ret[3]);
        return ret;
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

    void shootBullet(Vector3 size, Vector3 bulletDirection, float speed) {
        newBullet = Instantiate(bullet, transform.position, transform.rotation);
        newBullet.transform.localScale = size;
        newBullet.GetComponent<Projectile>().setSource(transform);
        newBullet.GetComponent<Projectile>().startingPos = bulletDirection;
        newBullet.GetComponent<Projectile>().speed = speed;
    }

    public static double findAngle(float x1, float y1, float x2, float y2) {
        float dy = y2 - y1;
        float dx = x2 - x1;
        if (dx == 0) {
        if (dy > 0) {
            return 90;
        } else if (dy < 0) {
            return 270;
        } else {
            return 0;
        }
        }
        float slope = dy / dx;
        double ret = Math.Atan((double) slope) * (180 / Math.PI);
        if (dx >= 0) {
        return ret;
        } else {
        return ret + 180;
        }
    }

    public static double findDistance(float x1, float y1, float x2, float y2) {
	    return Math.Sqrt(Math.Pow(x1 - x2, 2) + Math.Pow(y1 - y2, 2));
	  }

}
