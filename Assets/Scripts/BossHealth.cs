using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealth : MonoBehaviour
{
    public Slider healthBar;
    public int bossHealth;
    public GameObject boss;

    private void Start()
    {
        bossHealth = boss.GetComponent<BossEnemy>().maxHP;
        healthBar = GetComponent<Slider>();
        healthBar.maxValue = 4000;
        healthBar.value = 4000;
    }

    public void SetHealth(int hp)
    {
        healthBar.value = hp;
    }
}