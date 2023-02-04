using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_2 : EnemyAI
{
void OnCollisionEnter2D(Collision2D otherCollision){
    //print("AI_2 hit");
    Collider2D other = otherCollision.collider;
    if(other.CompareTag("Player")){
        Transform player = other.transform;

        if(player.position.y - other.bounds.extents.y > transform.position.y + transform.GetComponent<Collider2D>().bounds.extents.y)
            {
                JumpedOnto(other);
            }
            else
            {
            print("hit side");
        }

        // if(state == SwingState.Grappling) DeattachHook();
        // state = SwingState.Walking;
    }
    }

    private void JumpedOnto(Collider2D other)
    {
        print("jumped on head");
        other.GetComponent<PlayerMovement>().state = SwingState.Walking;
        uprootable = true;
    }
}
