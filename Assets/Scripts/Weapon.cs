using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{

    [SerializeField] Transform weapon;
    [SerializeField] Transform player;

    void Update()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 direction = new Vector3(mousePos.x - 960f, mousePos.y - 540f, 0f);
        //Debug.Log(player.position);

        float angle = Vector3.Angle(direction, weapon.position - player.position);
        Vector3 normal = Vector3.Cross(direction, weapon.position - player.position);
        if (normal.z < 0) {
            transform.Rotate(0.0f, 0.0f, angle, Space.Self);
        } else {
            transform.Rotate(0.0f, 0.0f, -angle, Space.Self);
        }
    }
}
