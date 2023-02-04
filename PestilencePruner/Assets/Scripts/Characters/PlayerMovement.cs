using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5;
    public float jumpForce = 5;
    private Rigidbody2D rb;
    private Collider2D collider;
    private Vector2 movement;
    public float inputX;


    // public bool onGround = true;


    public SwingState state; 


    public SpringJoint2D joint;
    private Vector2 hookPos;

    public LineRenderer grappleLine;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
        state = SwingState.Walking;
    }

    // Update is called once per frame
    void Update()
    {
        inputX = Input.GetAxis("Horizontal"); 
        // float inputX = Input.GetAxis("Horizontal");
        
        // if(Input.GetKeyDown("space") && onGround) {
        //     onGround = false;
        //     rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        // }
        // //TODO: Fix the bug that the character will stick on the square when pressing "a" or "d" when jumping over it
        // rb.velocity = new Vector2(inputX * moveSpeed, rb.velocity.y);

        switch (state)
        {
            
            case SwingState.Walking:

            if(Input.GetKeyDown("space")) {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                state = SwingState.InAir;
            }
            // if(Input.GetKeyDown("space") && onGround) {
            //     onGround = false;
            //     rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            //     state = SwingState.InAir;
            // }
            rb.velocity = new Vector2(inputX * moveSpeed, rb.velocity.y);
            break;

            case SwingState.InAir:
            if(Input.GetMouseButtonDown(0)){
                if(HookPosFromMousePos(getMousePos())){
                    AttachHook(hookPos);
                    state = SwingState.Grappling;
                }
            }
            else    rb.velocity = new Vector2(inputX * moveSpeed, rb.velocity.y);


            // grappleLine.SetPosition(0, rb.position);   
            // if(Input.GetMouseButtonUp(0))   state = SwingState.Swinging;
            break;

            case SwingState.Grappling:
                if(Input.GetMouseButtonUp(0)){
                    
                    DeattachHook();
                    state = SwingState.InAir;
                }
            //     grappleLine.SetPosition(0, rb.position);
            //     if(Input.GetMouseButtonDown(0)) retracting = true;

            //     if(Input.GetMouseButtonUp(0)){
            //         state = SwingState.Flying;
            //         retracting = false;
            //         DeattachHook();
            //     }
            break;
            
            default:
            break;
        }

    }

    void OnTriggerEnter2D(Collider2D other){
        print("hit");
        if(other.CompareTag("Floor")){
            if(state == SwingState.Grappling) DeattachHook();
            state = SwingState.Walking;
        }
    }


    private bool HookPosFromMousePos(Vector2 mousepos){
        //max dist in 16:9 is 178:100, max diagonal dist 204
        //player will almost always be in centre of the screen, max dist from 89:100 is 134
        //I want it to get the closest point to the mousepos along mouse Dir Vector that is on a building collider
        Vector2 player2mouse = mousepos - rb.position;

        List<RaycastHit2D> castResults = new List<RaycastHit2D>();
        // castResults.Add(Physics2D.Raycast(mousepos, mouse2player, mouse2player.magnitude, LayerMask.GetMask("Brick")));// distance limited to not go behind the player
        // castResults.Add(Physics2D.Linecast(mousepos, rb.position, LayerMask.GetMask("Brick")));// distance limited to not go behind the player

        castResults.Add(Physics2D.Linecast(mousepos, rb.position));// distance limited to not go behind the player
        // castResults.Add(Physics2D.Raycast(mousepos, player2mouse, 130f - player2mouse.magnitude , LayerMask.GetMask("Brick")));
        
        var possibleCastResults =  castResults.Where(cast => cast.collider != null);
        if(possibleCastResults.Count() > 0){
            hookPos = possibleCastResults.OrderBy(cast => cast.distance).First().point;
            return true;
        }
        return false;
    }

    // private bool TempHook(Vector2 mousepos){
    //     hookPos = mousepos;
    //     return true;
    // }

    private void AttachHook(Vector2 globalPos){
        joint.connectedAnchor = globalPos;
        joint.enabled = true;

        grappleLine.SetPosition(0, rb.position);
        grappleLine.SetPosition(1, globalPos);
        grappleLine.enabled = true;
    }

    public void DeattachHook(){
        joint.enabled = false;
        grappleLine.enabled = false;
    }

    private Vector2 getMousePos()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.nearClipPlane;
        Vector2 worldPosition = Camera.main.ScreenToWorldPoint(mousePos);

        return worldPosition;
    }

}



public enum SwingState {
    Walking,
    InAir,
    Grappling
}