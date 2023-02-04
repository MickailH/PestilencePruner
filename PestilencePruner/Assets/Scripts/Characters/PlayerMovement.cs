using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5;
    public float jumpForce = 5;
    private Rigidbody2D rb;
    private Collider2D collider;
    private Vector2 movement;
    public bool onGround = true;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float inputX = Input.GetAxis("Horizontal");
        
        if(Input.GetKeyDown("space") && onGround) {
            onGround = false;
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
        //TODO: Fix the bug that the character will stick on the square when pressing "a" or "d" when jumping over it
        rb.velocity = new Vector2(inputX * moveSpeed, rb.velocity.y);

    }

    void OnTriggerEnter2D(Collider2D other){
        if(other.CompareTag("Floor") || other.CompareTag("Enemy")){
            onGround = true;
        }
    }

}
