using System.Collections;
using System.Collections.Generic;
using NUnit.Framework.Internal;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

public class PlayerController : MonoBehaviour
{
	//Player Components
	private Rigidbody2D rb;
	public Transform fireSpawnPoint;
	
	//Player Factors
	[Range(1,20)]
	public float playerSpeed;
	
	[Range(0,500)]
	public float playerJumpForce;
	
	[Range(1,20)]
	public float fallMultiplier;
	
	[Range(1,20)]
	public float lowJumpMultiplier;
	
	private bool isFacingRight;
	private bool canJump;
	
	[Range(10,50)]
	public float fireballSpeed;
	
	//Projectile
	public GameObject fireballPrefab;
	
	//TODO:
	private bool firstJump;
	private bool secondJump;

	void Awake ()
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
		if (Input.GetMouseButtonDown(0))
		{
			Fire();
		}

		if (Input.GetKeyDown(KeyCode.R))
		{
			Reflect();
		}
		
		//Handle Fall Multiplier (for weighter/tighter jumps)
		FallingMultiplier();
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

	void FallingMultiplier()
	{
		if (rb.velocity.y < 0)
		{
			rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
			// -1 accounts for normal unity gravity
		} else if (rb.velocity.y > 0 && ! Input.GetKey(KeyCode.Space))
		{
			rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
		}
	}

	void Fire()
	{
		//Instaniate
		var fireballInstance = (GameObject)Instantiate (
			fireballPrefab,
			fireSpawnPoint.position,
			fireSpawnPoint.rotation);
		
		//Mouse Aim	
		Vector3 fireDirection =
			Vector3.Normalize(Camera.main.ScreenToWorldPoint(Input.mousePosition) - fireballInstance.transform.position);
		
		Debug.Log(fireDirection);

		fireballInstance.GetComponent<Rigidbody2D>().velocity = fireDirection* fireballSpeed;

		// Destroy the bullet after 2 seconds
		Destroy(fireballInstance, 2.0f);
	}

	void Reflect()
	{
		
	}
}
