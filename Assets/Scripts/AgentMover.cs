using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class AgentMover : MonoBehaviour
{
    private Rigidbody2D rb2d;

    public float jumpForce = 1.2f;
    public float gMoon = 1.6f;

    private ConstantForce2D cForce;
    private Vector2 forceDirection;

    private AudioSource waterAudio;
    public AudioClip waterSound;


    public bool isGrounded;

    private float horizontal;
    private float speed = 2f;
    public Transform groundCheck;
    public LayerMask groundLayer;

    public Animator animator;

    [SerializeField]
    private float maxSpeed = 2, acceleration = 50, deacceleration = 100;
    [SerializeField]
    private float currentSpeed = 0;
    private Vector2 oldMovementInput;
    public Vector2 MovementInput { get; set; }

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();        
    }

    // Pelaajan ground-kosketuksen tarkistus. Mahdollistaa seiniin tarttumisen ja hyppimisen seini‰ pitkin.
    public bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed && IsGrounded())
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x, jumpForce);
        }

        if (context.canceled && rb2d.velocity.y > 0f)
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x, rb2d.velocity.y * 0.5f);
        }         
    }

    private void Update()
    {
        rb2d.velocity = new Vector2(horizontal * speed, rb2d.velocity.y);
    }

    public void Move(InputAction.CallbackContext context)
    {
        //animator.SetTrigger("Walk");
        horizontal = context.ReadValue<Vector2>().x;
    }

    public void Quit(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
        }
    }

    void FixedUpdate()
    {
        // Liikkumisen nopeuden s‰‰tˆ enemyille
        if(gameObject.CompareTag("Enemy") && isGrounded)
        {
            if (MovementInput.magnitude > 0 && currentSpeed >= 0)
            {
                animator.SetTrigger("Walk");
                rb2d.gravityScale = 1;
                oldMovementInput = MovementInput;
                currentSpeed += acceleration * maxSpeed * Time.deltaTime;
            }
            else
            {
                rb2d.gravityScale = 1;
                currentSpeed -= deacceleration * maxSpeed * Time.deltaTime;
            }
            currentSpeed = Mathf.Clamp(currentSpeed, 0, maxSpeed);
            rb2d.velocity = oldMovementInput * currentSpeed;
        }
        // Jos enemy k‰velee laidan yli, niin pudotetaan se
        if(gameObject.CompareTag("Enemy") && !isGrounded)
        {
            rb2d.constraints = RigidbodyConstraints2D.FreezeRotation;
            rb2d.constraints = ~RigidbodyConstraints2D.FreezePositionY;
        }
        if (gameObject.CompareTag("Enemy") && isGrounded)
        {
            rb2d.constraints = RigidbodyConstraints2D.FreezeRotation;
            rb2d.constraints = RigidbodyConstraints2D.FreezePositionY;
        }
    }

    // Tarkistaa onko enemy maassa ja asettaa sen mukaisesti boolean-arvon
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Jos on maassa
        if (collision.collider.tag == "Ground")
        {
            isGrounded = true;
        }        
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        // Ei ole maassa
        if (collision.collider.tag == "Ground")
        {
            isGrounded = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Jos menee veteen, painovoima hypp‰‰ volttia
        if (other.tag == "Water")
        {
            AudioSource.PlayClipAtPoint(waterSound, Camera.main.transform.position);
            
            cForce = GetComponent<ConstantForce2D>();
            forceDirection = new Vector2(0, 5);
            cForce.force = forceDirection;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if(other.tag == "Water")
        {
            forceDirection = forceDirection * 1;
            cForce.force = forceDirection;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        AudioSource.PlayClipAtPoint(waterSound, Camera.main.transform.position);
        // Jos l‰htee vedest‰, painovoima palautuu normaaliksi
        if (other.tag == "Water")
        {
            rb2d.AddForce(Physics.gravity * rb2d.mass);
        }

    }
}
