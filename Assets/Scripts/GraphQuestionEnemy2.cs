using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphQuestionEnemy2 : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float bulletSpeed = 5f;

    public void ShootAt(Vector3 target)
    {
        Vector3 direction = (target - transform.position).normalized;
        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        bullet.GetComponent<Rigidbody2D>().velocity = direction * bulletSpeed;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
