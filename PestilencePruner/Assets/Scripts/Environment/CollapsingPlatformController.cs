using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollapsingPlatformController : PlatformController
{
    private Rigidbody2D rb;
    //private Collider2D collider;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        // collider = GetComponent<Collider2D>();
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.name.Equals("Character"))
        {
            Collider2D col = other.collider;
            Transform player = other.transform;

            if(player.position.y - col.bounds.extents.y > transform.position.y + transform.GetComponent<Collider2D>().bounds.extents.y){
                print("jump reset");
                col.GetComponent<PlayerMovement>().HitGround();
                //Invoke("DropPlatform", 0.5f);
                Destroy(gameObject, 2f);
                PlatformManager.Instance.StartCoroutine("SpawnPlatform", new Vector2(transform.position.x, transform.position.y));
            }
        }
    }


    void DropPlatform()
    {
        rb.isKinematic = false;
    }
}
