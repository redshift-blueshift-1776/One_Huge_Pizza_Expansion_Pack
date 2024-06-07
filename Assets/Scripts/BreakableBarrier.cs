using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableBarrier : MonoBehaviour
{
    public int maxHP = 10;
    private int currentHP;

    public Sprite fullHealthSprite;
    public Sprite lowHealthSprite;

    private SpriteRenderer spriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        currentHP = maxHP;
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = fullHealthSprite;        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the barrier is hit by a bullet
        if (collision.gameObject.CompareTag("Bullet"))
        {
            TakeDamage(1);
            Destroy(collision.gameObject); // Optional: Destroy the bullet on impact
        }
    }

    void TakeDamage(int damage)
    {
        currentHP -= damage;

        if (currentHP <= 0)
        {
            Destroy(gameObject); // Destroy the barrier if HP is 0 or less
        }
        else if (currentHP <= 5)
        {
            spriteRenderer.sprite = lowHealthSprite; // Change the sprite if HP is 5 or less
        }
    }
}
