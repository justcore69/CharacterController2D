using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController2D : MonoBehaviour
{
    [HideInInspector] public static GameObject _this;

    [Header("Horizontal Movement")]
    public float horizontalVelocity;

    [Header("Jump and Gravity")]
    public float jumpVelocity;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;
    public float groundCheckCircleRadius = 0.2f;
    public LayerMask whatIsGround;

    [Header("Input")]
    public KeyCode keyJump = KeyCode.W;

    [Header("GameObjects and something")]
    public Transform feetPos;

    private float moveInput;

    //Components
    private Rigidbody2D rb;

    private void Awake()
    {
        _this = gameObject;
        rb = GetComponent<Rigidbody2D>();

        if (!Interface.CheckRequireComponents()) this.enabled = false;
    }

    private void Update()
    {
        CalculateMovement();
        CalculateJump();
    }

    private void CalculateMovement()
    {
        moveInput = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(moveInput * horizontalVelocity, rb.velocity.y);
    }

    private void CalculateJump()
    {
        if (Input.GetKeyDown(keyJump))
        {
            if (CheckGround())
            {
                Jump();
            }
        }

        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (rb.velocity.y > 0 && !Input.GetKey(keyJump))
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpVelocity);
    }

    private bool CheckGround() => Physics2D.OverlapCircle(feetPos.position, groundCheckCircleRadius, whatIsGround);
}



public static class Interface
{
    public static bool CheckRequireComponents()
    {
        if (CharacterController2D._this.GetComponent<Collider2D>() == null) { Debug.LogError("CharacterController2D: Collider2D is required!"); return false; }
        if (CharacterController2D._this.GetComponent<Rigidbody2D>() == null) { Debug.LogError("CharacterController2D: Rigidbody2D is required!"); return false; }

        CharacterController2D._this.GetComponent<Rigidbody2D>().freezeRotation = true;

        return true;
    }
}
