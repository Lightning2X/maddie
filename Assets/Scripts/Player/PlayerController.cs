using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float movementSpeed;
    [SerializeField]
    private float groundCheckRadius;
    [SerializeField]
    private float jumpForce;
    [SerializeField]
    private float jumpTime;
    [SerializeField]
    private float fallMultiplier;
    [SerializeField]
    private Transform groundCheck;
    [SerializeField]
    private Transform ceilingCheck;
    [SerializeField]
    private LayerMask whatIsGround;
    [SerializeField]
    private Animator playerAnimator;

    public float attackDelay;
    private float currentAttackDelay;
    private bool canAttack;

    public float xInput;
    private float jumpTimeCounter;
    public bool canDoubleJump;

    private int facingDirection = 1;

    public bool isGrounded;
    private bool isCealed;
    public bool isJumping;
    public bool canJump;
    public bool canControl = true;
    public bool isInAttack = false;


    private Vector2 newVelocity;
    private Vector2 newForce;

    public Rigidbody2D rb;
    public PhysicsMaterial2D noFriction;

    public PhysicsMaterial2D hiFriction;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        CheckInput();    

        currentAttackDelay -= Time.deltaTime;
        if(currentAttackDelay <= 0){
            canAttack = true;
        }
    }

    private void FixedUpdate()
    {
        ApplyMovement(); 
        
        CheckGround();
 
        ApplyAnimation();

        if(isInAttack && playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Attack")) { 
            isInAttack = false;
        }
    }

    private void CheckInput()
    {
        if(Input.GetKeyDown(KeyCode.T)) { 
            GameController.instance.GameOver();
        }
        xInput = Input.GetAxisRaw("Horizontal");
        // Switch physics material for higher friction and no sliding
        if(xInput == 0){ 
            rb.sharedMaterial = hiFriction;
        }
        else {
            rb.sharedMaterial = noFriction;
        }

        if (xInput > 0 && facingDirection == -1)
        {
            Flip();
        }
        else if (xInput < 0 && facingDirection == 1)
        {
            Flip();
        }

        if(canControl){
            if (Input.GetButtonDown("Jump"))
            {
                Jump();
            }
            
            if(Input.GetButton("Jump"))
            {
                highJump();
                
            }

            if (Input.GetButtonUp("Jump"))
            {
                isJumping = false;
            }

            if(Input.GetMouseButtonDown(0)){
                 if(isGrounded)
                    Attack();
            }
        }
        
    }

    private void CheckGround()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
        isCealed = Physics2D.OverlapCircle(ceilingCheck.position, groundCheckRadius, whatIsGround);

        if(isGrounded && !isJumping)
        {
            canJump = true;
            canDoubleJump = true;
        }

    }


    private void Jump()
    {
        if (canJump || canDoubleJump)
        {
            if(!isGrounded && canJump){
                canJump = false;
            }
            else { 
                canDoubleJump = false;
            }
            isJumping = true;
            jumpTimeCounter = jumpTime;

            rb.velocity = new Vector2(rb.velocity.x, jumpForce);   
            playerAnimator.SetTrigger("Jumped");
        }
    }   

    private void highJump(){
        if(isJumping){       

            if(jumpTimeCounter > 0)
            {           
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);

                jumpTimeCounter -= Time.deltaTime;

                if (isCealed)
                {
                    jumpTimeCounter = 0;
                }

            } else {
                isJumping = false;
            }
        } 
    }

    private void Attack(){
        if(canAttack){
            rb.velocity = new Vector2(0,0);
            playerAnimator.SetTrigger("Attack");   
            currentAttackDelay = attackDelay;  
            canAttack = false;
        } 
    }

    private void ApplyMovement()
    {
            rb.velocity = new Vector2(movementSpeed * xInput, rb.velocity.y);


            if (rb.velocity.y < 0)
            {
                rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
            }
    }

    private void ApplyAnimation(){
        playerAnimator.SetFloat("Speed", Mathf.Abs(rb.velocity.x));
        playerAnimator.SetFloat("YVel", rb.velocity.y);
        playerAnimator.SetBool("isGrounded", isGrounded);
    }


    private void Flip()
    {
        facingDirection *= -1;
        transform.Rotate(0.0f, 180.0f, 0.0f);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }

    private void OnCollisionEnter2D(Collision2D other) {

        if(other.gameObject.tag == "Enemy") { 
            GameController.instance.GameOver();
        }
        StaticObjectProps props = StaticObjectProps.getProps(other.gameObject);
        if(props.deadly) { 
            GameController.instance.GameOver();
        }
    }

}

