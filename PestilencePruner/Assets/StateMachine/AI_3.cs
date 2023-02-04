using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_3 : EnemyAI
{
void OnCollisionEnter2D(Collision2D otherCollision){
    //print("AI_2 hit");
    Collider2D other = otherCollision.collider;
    if(other.CompareTag("Player")){
        Transform player = other.transform;

        if(player.GetComponent<PlayerMovement>().state == SwingState.Grappling){
            print("swung into");
            uprootable=true;
        }
        else{
            print("hit side");
        }

        // if(state == SwingState.Grappling) DeattachHook();
        // state = SwingState.Walking;
    }
    }
}
