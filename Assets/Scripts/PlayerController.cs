using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	//Player Components
	private Rigidbody2D rb;
	private Collider2D coll;
	public Transform fireSpawnPoint;
	// Should we make spawn point a GetComponent because all we need is the child transform?
	
	//Player Factors
	public float playerSpeed;
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
		coll = GetComponent<Collider2D>();
		canJump = false;
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
		float moveHorizontal = Input.GetAxis("Horizontal");
		isFacingRight = moveHorizontal > 0 ? true : false;
		rb.velocity = new Vector2(moveHorizontal * playerSpeed, rb.velocity.y);
		
		//Handle Firing
		if (Input.GetKeyDown(KeyCode.F))
		{
			Fire();
		}
	}
	
	
	void OnCollisionEnter2D (Collision2D other) 
	{
		if (other.gameObject.tag == "Floor")
		{
			canJump = true;
			Debug.Log("Touching Ground");
		}
	}
	
	
	void Jump()
	{
		Debug.Log("Left Ground");
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
		fireballInstance.GetComponent<Rigidbody2D>().velocity = fireballInstance.transform.forward * fireballSpeed;

		// Destroy the bullet after 2 seconds
		Destroy(fireballInstance, 2.0f);
	}
}
