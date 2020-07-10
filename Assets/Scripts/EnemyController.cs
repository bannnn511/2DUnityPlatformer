using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
	public int maxHealth = 120;
	Animator enemyAnimator;
	int currentHealth;
	Rigidbody2D enemyRigidBody;
	// Start is called before the first frame update
	void Start()
	{
		enemyAnimator = GetComponent<Animator>();
		currentHealth = maxHealth;
		enemyRigidBody = GetComponent<Rigidbody2D>();
	}

	// Update is called once per frame
	void Update()
	{

	}

	public void TakeDamage(int damage)
	{
		enemyAnimator.SetTrigger("IsHurt");
		currentHealth -= damage;
		if (currentHealth <= 0)
		{
			Die();
		}
	}

	void Die()
	{
		enemyAnimator.SetBool("IsDeath", true);
		GetComponent<Rigidbody2D>().simulated = false;
		// this.enabled = false;
	}
}
