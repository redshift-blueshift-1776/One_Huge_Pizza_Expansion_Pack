using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthLevel1 : MonoBehaviour
{
    public Slider healthBar;
    public int bossHealth;
    public GameObject boss;

    private void Start()
    {
        bossHealth = boss.GetComponent<BossEnemy>().maxHP;
        healthBar = GetComponent<Slider>();
        healthBar.maxValue = 1500;
        healthBar.value = 1500;
    }

    public void SetHealth(int hp)
    {
        healthBar.value = hp;
    }
}