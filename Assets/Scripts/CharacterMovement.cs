using UnityEngine;
using System.Collections;

public class CharacterMovement : MonoBehaviour
{
    private GameObject playerSprite;
    private Animator anim;
    private Rigidbody2D playerRigidBody2D;
    private bool facingRight;
    public float speed = 4.0f;



    // Use this for initialization
    void Awake()
    {
        playerSprite = transform.Find("PlayerSprite").gameObject;
        anim = (Animator)playerSprite.GetComponent(typeof(Animator));
        playerRigidBody2D = (Rigidbody2D)GetComponent(typeof(Rigidbody2D));
    }

    // Update is called once per frame
    void Update()
    {
        float movePlayerVector = Input.GetAxis("Horizontal");

        anim.SetFloat("speed", Mathf.Abs(movePlayerVector));

        playerRigidBody2D.velocity = new Vector2(movePlayerVector * speed, playerRigidBody2D.velocity.y);

        if (movePlayerVector > 0 && !facingRight)
        {
            Flip();
        }
        else if (movePlayerVector < 0 && facingRight)
        {
            Flip();
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector2 theScale = playerSprite.transform.localScale;
        theScale.x *= -1;
        playerSprite.transform.localScale = theScale;
    }
}
