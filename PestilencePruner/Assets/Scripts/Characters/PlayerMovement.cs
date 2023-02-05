using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5;
    public float jumpForce = 5;
    public int HP = 3;
    public int currentHP = 3;
    public Slider healthBar;
    private Rigidbody2D rb;
    private Collider2D collider;
    private Vector2 movement;
    public float inputX;

    public static PlayerMovement instance;
    // public bool onGround = true;
    [SerializeField] GameObject pauseMenu;

    public SwingState state; 


    public SpringJoint2D joint;
    private Vector2 hookPos;
    private Transform hookTransf;

    public LineRenderer grappleLine;

    private void Awake()
    {
        instance = this;
    }


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
        currentHP = HP;
        healthBar.maxValue = HP;
        healthBar.value = HP;
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

            if(Input.GetKeyDown("escape"))
                {
                    if (pauseMenu.activeSelf)
                    {
                        pauseMenu.SetActive(false);
                        Time.timeScale = 1f;
                    } else
                    {
                        pauseMenu.SetActive(true);
                        Time.timeScale = 0f;
                    }
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
                if(HookObjFromMousePos(getMousePos())){

                    if(hookTransf.CompareTag("Platform")){
                        if(transform.position.y < hookTransf.transform.position.y)//if ceiling is above you TODO: suspected bug here, when player and ceiling are close together
                        AttachHook(hookPos);
                    }

                    if(hookTransf.CompareTag("Enemy")){
                        float angle = Vector2.Angle(Vector2.up, transform.position - hookTransf.transform.position);
                        print(angle);
                        if(transform.position.y > hookTransf.transform.position.y && angle < 45)
                        Whip(hookTransf);   
                    }



                    // if(hookPos.y > transform.position.y)//swing
                    // else //uproot
                    // AttachHook(hookPos);
                    
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

    public void HitGround()
    {
        print("hitground");
        if (state == SwingState.Grappling) DeattachHook();
        state = SwingState.Walking;
    }

    void OnTriggerEnter2D(Collider2D other){
        print("hit");
        if(other.CompareTag("Floor")){
            if(state == SwingState.Grappling) DeattachHook();
            state = SwingState.Walking;
        }
        if (other.CompareTag("Enemy"))
        {
            Transform Enemy = other.transform;

            if (Enemy.position.y + other.bounds.extents.y < transform.position.y - transform.GetComponent<Collider2D>().bounds.extents.y)
            {
                if (other.GetComponent<AI_2>() != null)
                {
                    rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                    state = SwingState.InAir;
                } else
                {
                    print("jump reset");
                    HitGround();
                }
            }
            else { 
            takeDamage();
        }
        }
    }


    private void takeDamage()
    {
        if (currentHP <= 0)
        {
            //GameOver();
            SceneManager.LoadScene(0);

        } else
        {
            //StartCoroutine(Invulnerability());
            currentHP = currentHP -= 1;
            healthBar.value = currentHP;
           // healthBar.SetHealth(currentHealth);
        }

    }

    private void SetMaxHealth(int health)
    {
        healthBar.maxValue = health;
        healthBar.value = health;
    }

    private void SetHealth(int health)
    {
        healthBar.value = health;
    }

    /*
    IEnumerator Invulnerability()
    {
        yield return new WaitForSeconds(1);
    }
    */


    private bool HookObjFromMousePos(Vector2 mousepos){
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
            RaycastHit2D hit = possibleCastResults.OrderBy(cast => cast.distance).First();
            hookPos = hit.point;
            hookTransf = hit.collider.transform;
            return true;
        }
        return false;
    }

    // private bool HookPosFromMousePos(Vector2 mousepos){
    //     //max dist in 16:9 is 178:100, max diagonal dist 204
    //     //player will almost always be in centre of the screen, max dist from 89:100 is 134
    //     //I want it to get the closest point to the mousepos along mouse Dir Vector that is on a building collider
    //     Vector2 player2mouse = mousepos - rb.position;

    //     List<RaycastHit2D> castResults = new List<RaycastHit2D>();
    //     // castResults.Add(Physics2D.Raycast(mousepos, mouse2player, mouse2player.magnitude, LayerMask.GetMask("Brick")));// distance limited to not go behind the player
    //     // castResults.Add(Physics2D.Linecast(mousepos, rb.position, LayerMask.GetMask("Brick")));// distance limited to not go behind the player

    //     castResults.Add(Physics2D.Linecast(mousepos, rb.position));// distance limited to not go behind the player
    //     // castResults.Add(Physics2D.Raycast(mousepos, player2mouse, 130f - player2mouse.magnitude , LayerMask.GetMask("Brick")));
        
    //     var possibleCastResults =  castResults.Where(cast => cast.collider != null);
    //     if(possibleCastResults.Count() > 0){
    //         hookPos = possibleCastResults.OrderBy(cast => cast.distance).First().point;
    //         return true;
    //     }
    //     return false;
    // }

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

        state = SwingState.Grappling;
    }

    public void DeattachHook(){
        joint.enabled = false;
        grappleLine.enabled = false;
    }

    public void Whip(Transform tr){
        // if(tr.GetComponent<EnemyAI>())
        print("whip");
    }

    private Vector2 getMousePos()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.nearClipPlane;
        Vector2 worldPosition = Camera.main.ScreenToWorldPoint(mousePos);

        return worldPosition;
    }

    public IEnumerator Knockback(float duration, float power, Transform obj)
    {
        float KBtimer = 0;
        while (duration > KBtimer)
        {
            KBtimer += Time.deltaTime;
            Vector2 direction = (obj.transform.position - this.transform.position).normalized;
            rb.AddForce(-direction * power);
        }
        yield return 0;
    }

}



public enum SwingState {
    Walking,
    InAir,
    Grappling
}