using System.Collections;
using System.Collections.Generic;
using NUnit.Framework.Internal;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	//Player Components
	private Rigidbody2D rb;
	public Transform fireSpawnPoint;
	
	//Player Factors
	[Range(1,20)]
	public float playerSpeed;
	[Range(100,500)]
	public float playerJumpForce;
	private bool isFacingRight;
	private bool canJump;
	public float fireballSpeed;
	
	//Projectile
	public GameObject fireballPrefab;
	
	//TODO:
	private bool firstJump;
	private bool secondJump;

	void Start ()
	{
		rb = GetComponent<Rigidbody2D>();
		isFacingRight = true;
	}

	void Update()
	{
		//Handle Jump
		if (Input.GetKeyDown(KeyCode.Space) && canJump)
		{
			Jump();
		}
		
		//Handle Movement
		Move();
		
		//Handle Firing
		if (Input.GetKeyDown(KeyCode.F))
		{
			Fire();
		}

		if (Input.GetKeyDown(KeyCode.R))
		{
			Reflect();
		}
	}

	void Move()
	{
		float moveHorizontal = Input.GetAxis("Horizontal");
		rb.velocity = new Vector2(moveHorizontal * playerSpeed, rb.velocity.y);
		
		//Handle facing left/right
		// If Moving Right and Not Oriented towards right and vice versa
		if (moveHorizontal > 0 && !isFacingRight || moveHorizontal < 0 && isFacingRight)
		{
			Flip();
		}
	}

	void Flip()
	{
		//Change Direction
		isFacingRight = !isFacingRight;
		Vector3 currentScale = transform.localScale;
		currentScale.x *= -1;
		transform.localScale = currentScale;
	}
	
	
	void OnCollisionEnter2D (Collision2D other) 
	{
		if (other.gameObject.tag == "Floor")
		{
			canJump = true;
		}
	}
	
	
	void Jump()
	{
		canJump = false;
		Vector2 movement = Vector2.up * playerJumpForce;
		rb.AddForce(movement * playerSpeed);
	}

	void Fire()
	{
		var fireballInstance = (GameObject)Instantiate (
			fireballPrefab,
			fireSpawnPoint.position,
			fireSpawnPoint.rotation);

		// Add velocity to the bullet
		fireballInstance.GetComponent<Rigidbody2D>().velocity = Vector2.right * fireballSpeed;

		// Destroy the bullet after 2 seconds
		Destroy(fireballInstance, 2.0f);
	}

	void Reflect()
	{
		
	}
}
