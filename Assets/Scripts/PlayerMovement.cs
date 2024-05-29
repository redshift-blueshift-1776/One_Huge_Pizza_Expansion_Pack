using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody2D rb;

    public string up;
    public string down;
    public string left;
    public string right;

    public int HP;

    public bool isInvincible = false;

    private bool hacks = false;

    Color og;
    Color transparent;

    [SerializeField] GameObject weapon;

    public AudioSource hit;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        HP = 5;
        og = GetComponent<Renderer>().material.color;
        hit = GameObject.Find("hit").GetComponent<AudioSource>();
        transparent = new Color(og.r, og.g, og.b, 0.5f);
        hacks = false;
    }
    
    void Update()
    {
        float x=0, y=0;

        if (Input.GetKey(up)) y = 1;
        if (Input.GetKey(down)) y = -1;
        if (Input.GetKey(left)) x = -1;
        if (Input.GetKey(right)) x = 1;

        // if ((Input.GetKey(KeyCode.R) && Input.GetKey(KeyCode.F)) && Input.GetKey(KeyCode.L)) {
        //     hacks = true;
        //     Debug.Log("hacks on.");
        // }
        if (Input.GetKey(KeyCode.R) && Input.GetKey(KeyCode.F)) {
            hacks = true;
            Debug.Log("hacks on.");
        }
        if (Input.GetKey(KeyCode.Q) && Input.GetKey(KeyCode.K)) {
            hacks = false;
            Debug.Log("hacks off.");
        }

        // Calculate movement vector
        Vector2 movement = new Vector2(x, y).normalized;

        // Move the player
        MovePlayer(movement);

        if ((HP <= 0) && (!hacks)) {
            int sceneID = SceneManager.GetActiveScene().buildIndex;
            if (sceneID == 1) {
                SceneManager.LoadScene(3);
            } else if (sceneID == 5) {
                SceneManager.LoadScene(6);
            } else if (sceneID == 7) {
                SceneManager.LoadScene(8);
            }
        }
    }

    void MovePlayer(Vector2 movement)
    {
        // Apply movement to the rigidbody
        rb.velocity = new Vector2(movement.x * moveSpeed, movement.y * moveSpeed);

        
    }

    public void Hit() {
        if (!hacks) {
            hit.Play();
            StartCoroutine(iframe());
        }
    }

    IEnumerator iframe()
    { 
        isInvincible = true;
        for (int i = 0; i < 10; i++) {
            weapon.GetComponent<Renderer>().material.color = transparent;
            yield return new WaitForSeconds(0.1f);
            weapon.GetComponent<Renderer>().material.color = og;
            yield return new WaitForSeconds(0.1f);
        }
        GetComponent<Renderer>().material.color = Color.white;
        isInvincible = false;
        yield return null;
    }
}
