using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthLevel2 : MonoBehaviour
{
    public Slider healthBar;
    public int bossHealth;
    public GameObject boss;

    private void Start()
    {
        bossHealth = boss.GetComponent<BossEnemy>().HP;
        healthBar = GetComponent<Slider>();
        healthBar.maxValue = 2000;
        healthBar.value = 2000;
    }

    public void SetHealth(int hp)
    {
        healthBar.value = hp;
    }
}