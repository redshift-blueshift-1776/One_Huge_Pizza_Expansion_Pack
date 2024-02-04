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

    Color og;
    Color transparent;

    [SerializeField] GameObject weapon;

    private AudioSource hit;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        HP = 5;
        og = GetComponent<Renderer>().material.color;
        hit = GameObject.Find("Hit").GetComponent<AudioSource>();
        transparent = new Color(og.r, og.g, og.b, 0.5f);
    }
    
    void Update()
    {
        float x=0, y=0;

        if (Input.GetKey(up)) y = 1;
        if (Input.GetKey(down)) y = -1;
        if (Input.GetKey(left)) x = -1;
        if (Input.GetKey(right)) x = 1;

        // Calculate movement vector
        Vector2 movement = new Vector2(x, y).normalized;

        // Move the player
        MovePlayer(movement);

        if (HP == 0) {
            SceneManager.LoadScene(3);
        }
    }

    void MovePlayer(Vector2 movement)
    {
        // Apply movement to the rigidbody
        rb.velocity = new Vector2(movement.x * moveSpeed, movement.y * moveSpeed);

        
    }

    public void Hit() {
        hit.Play();
        StartCoroutine(iframe());
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
