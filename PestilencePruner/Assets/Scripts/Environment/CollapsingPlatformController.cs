using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollapsingPlatformController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Collider2D collider;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        //TODO: Only trigger when the character is above the platform
       // if (transform.position.y + collider.bounds.extents.y < other.gameObject.position.y - other.bounds.extents.y)
        //{
            if (other.gameObject.name.Equals("Character"))
            {
                //Invoke("DropPlatform", 0.5f);
                Destroy(gameObject, 2f);
                PlatformManager.Instance.StartCoroutine("SpawnPlatform", new Vector2(transform.position.x, transform.position.y));
            }
       // }
    }


    void DropPlatform()
    {
        rb.isKinematic = false;
    }
}
