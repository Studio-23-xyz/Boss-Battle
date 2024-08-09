using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{

    void OnTriggerEnter2D(Collider2D other)
    {
        var boss = other.GetComponent<BossHealth>();
          
        if (boss != null)
        {
            boss.ColorChange();
        }
         
    }
}
