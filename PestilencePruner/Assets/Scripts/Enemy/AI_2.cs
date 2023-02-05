using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_2 : EnemyAI
{
void OnCollisionEnter2D(Collision2D otherCollision){
    
    Collider2D other = otherCollision.collider;
    if(other.CompareTag("Player")){
        print("AI_2 hit");
        Transform player = other.transform;

        if(player.position.y - other.bounds.extents.y > transform.position.y + transform.GetComponent<Collider2D>().bounds.extents.y)
            {
                JumpedOnto(other);
            }
            else
            {
            print("hit side");
                StartCoroutine(PlayerMovement.instance.Knockback(parameter.KBduration, parameter.KBpower, this.transform));
            }

        // if(state == SwingState.Grappling) DeattachHook();
        // state = SwingState.Walking;
    }
    }

    private void JumpedOnto(Collider2D other)
    {
        print("jumped on head");
        gameObject.GetComponent<EnemyAI>().parameter.moveSpeed = 0;
        gameObject.GetComponent<EnemyAI>().parameter.chaseSpeed = 0;
        uprootable = true;
    }
}
