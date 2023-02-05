using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_3 : EnemyAI
{
void OnCollisionEnter2D(Collision2D otherCollision){
    Collider2D other = otherCollision.collider;
    if(other.CompareTag("Player")){
        print("AI_3 hit");
        Transform player = other.transform;

        if(player.GetComponent<PlayerMovement>().state == SwingState.Grappling){
            SwungInto();
        }
        else{
            print("hit side");
            StartCoroutine(PlayerMovement.instance.Knockback(parameter.KBduration, parameter.KBpower, this.transform));
            }

        // if(state == SwingState.Grappling) DeattachHook();
        // state = SwingState.Walking;
    }


    }

    public void SwungInto(){
        print("swung into");
        uprootable=true;
    }
}
