using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerController : MonoBehaviour
{
	public float moveSpeed = 6f;
	float jumpForce = 3f;
	public float attackRange = 0.5f;
	public int attackDamage = 50;
	public float attackRate = 2f;
	float nextAttackTime = 0f;
	float jumpTime = 0.5f;
	public Transform feetPos;
	public float checkRadius;
	public LayerMask whatIsGround;
	public Transform attackPoint;
	public LayerMask enemyLayer;
	bool isGrounded;
	Rigidbody2D rigidbody;
	Vector2 movement;
	float jumpTimeCounter;
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

		// Attacking
		if (Time.time >= nextAttackTime)
		{
			if (Input.GetKeyUp(KeyCode.J))
			{
				attackRange = 0.4f;
				playerAnimator.SetFloat("IsAttackingMove", 1);
				Attack();
			}
			if (Input.GetKeyUp(KeyCode.K))
			{
				attackRange = 1f;
				playerAnimator.SetFloat("IsAttackingMove", 2);
				Attack();
			}
			if (Input.GetKeyUp(KeyCode.L))
			{
				attackRange = 0.9f;
				playerAnimator.SetFloat("IsAttackingMove", 3);
				Attack();
			}

		}
	}
	private void FixedUpdate()
	{
		// rigidbody.MovePosition(rigidbody.position + movement * moveSpeed * Time.fixedDeltaTime);
		rigidbody.velocity = new Vector2(moveInput * moveSpeed, rigidbody.velocity.y);
	}

	void Attack()
	{
		playerAnimator.SetTrigger("IsAttacking");
		Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);
		foreach (Collider2D enemy in hitEnemies)
		{
			enemy.GetComponent<EnemyController>().TakeDamage(attackDamage);
		}
		nextAttackTime = Time.time + 1f / attackRate;
	}

	void OnDrawGizmosSelected()
	{
		if (attackPoint == null)
			return;
		Gizmos.DrawWireSphere(attackPoint.position, attackRange);
	}
}