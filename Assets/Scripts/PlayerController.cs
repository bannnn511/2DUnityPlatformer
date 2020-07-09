using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerController : MonoBehaviour
{
	public float moveSpeed = 6f;
	public float jumpForce = 3f;
	public Transform feetPos;
	public float checkRadius;
	public LayerMask whatIsGround;
	bool isGrounded;
	Rigidbody2D rigidbody;
	Vector2 movement;
	float jumpTimeCounter;
	public float jumpTime = 0.5f;
	bool isJumping;
	float moveInput;
	Animator playerAnimator;

	// Start is called before the first frame update
	void Start()
	{
		rigidbody = GetComponent<Rigidbody2D>();
		playerAnimator = GetComponent<Animator>();
	}

	// Update is called once per frame
	void Update()
	{
		// movement.x = Input.GetAxisRaw("Horizontal");
		// movement.y = Input.GetAxisRaw("Vertical");
		moveInput = Input.GetAxisRaw("Horizontal");

		playerAnimator.SetFloat("Speed", Math.Abs(moveInput * moveSpeed));


		// The longer space is hold the higher you jump
		if (moveInput > 0)
		{
			transform.eulerAngles = new Vector3(0, 0, 0);
		}
		else if (moveInput < 0)
		{
			transform.eulerAngles = new Vector3(0, 180, 0);
		}

		isGrounded = Physics2D.OverlapCircle(feetPos.position, checkRadius, whatIsGround);
		if (isGrounded == true && Input.GetKeyDown(KeyCode.Space))
		{
			Debug.Log("jumping");
			isJumping = true;
			playerAnimator.SetBool("IsJumping", isJumping);
			jumpTimeCounter = jumpTime;
			// rigidbody.velocity = Vector2.up * jumpForce;
			rigidbody.velocity = new Vector2(rigidbody.velocity.x, jumpForce);
		}

		if (Input.GetKey(KeyCode.Space) && isJumping == true)
		{
			if (jumpTimeCounter > 0)
			{
				// rigidbody.velocity = Vector2.up * jumpForce;
				rigidbody.velocity = new Vector2(rigidbody.velocity.x, jumpForce);
				jumpTimeCounter -= Time.deltaTime;
			}
			else
			{
				isJumping = false;
				playerAnimator.SetBool("IsJumping", isJumping);
			}
		}


		if (Input.GetKeyUp(KeyCode.Space))
		{
			isJumping = false;
			playerAnimator.SetBool("IsJumping", isJumping);
		}
	}
	private void FixedUpdate()
	{
		// rigidbody.MovePosition(rigidbody.position + movement * moveSpeed * Time.fixedDeltaTime);
		rigidbody.velocity = new Vector2(moveInput * moveSpeed, rigidbody.velocity.y);
	}
}

