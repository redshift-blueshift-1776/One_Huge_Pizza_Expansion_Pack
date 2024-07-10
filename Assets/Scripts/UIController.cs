using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] GameObject player;

    public bool[] health = new bool[5];
    public GameObject[] healthObjects;

    public int playerHealth;
    public bool playerHacks;

    public Texture empty_heart;
    public Texture full_heart;
    public Texture hacks;

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        playerHealth = player.GetComponent<PlayerMovement>().HP;
        playerHacks = player.GetComponent<PlayerMovement>().hacks;
        if (playerHacks) {
            for (int i = 0; i < 5; i++) {
                healthObjects[i].GetComponent<RawImage>().texture = hacks;
            }
        } else {
            if (playerHealth >= 0) {
                for (int i = 0; i < playerHealth; i++) {
                healthObjects[i].GetComponent<RawImage>().texture = full_heart;
                }
                for (int i = playerHealth; i < 5; i++) {
                    healthObjects[i].GetComponent<RawImage>().texture = empty_heart;
                }
            }
        }
    }
}
