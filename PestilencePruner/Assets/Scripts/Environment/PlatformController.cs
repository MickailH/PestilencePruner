using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour
{

    void OnTriggerEnter2D(Collider2D other){
    //print("plat hit");
    if(other.CompareTag("Player")){
        Transform player = other.transform;

        if(player.position.y - other.bounds.extents.y > transform.position.y + transform.GetComponent<Collider2D>().bounds.extents.y){
            print("jump reset");
            other.GetComponent<PlayerMovement>().HitGround();
        }

        // if(state == SwingState.Grappling) DeattachHook();
        // state = SwingState.Walking;
    }
    }

   
}
