using System.Collections;
using System.Collections.Generic;
//using NUnit.Framework.Internal;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

public class PlayerController : MonoBehaviour
{
	//Player Components
	private Rigidbody2D rb;
	public Transform fireSpawnPoint;
	public GameObject ReflectionCollider;

	//Player Factors
	public float playerSpeed;
	public float playerJumpForce;
	public float fallMultiplier;
	public float lowJumpMultiplier;
	private bool canJump;
	public float fireballSpeed;

	//Projectile
	public GameObject fireballPrefab;

	//Network Components
	private NetworkMove netMove;

	//TODO:
	public int jumpCount = 0;
    public int jumpLimit = 1;

	/* private bool firstJump;
	*  private bool secondJump;
	*/
	void Awake ()
	{
		rb = GetComponent<Rigidbody2D>();
		netMove = GetComponent<NetworkMove>();
	}

	void Update()
	{
		//Handle Jumping (Double Jump)
		if (Input.GetButtonDown("Jump") && jumpCount < jumpLimit && canJump)
        {
            jumpCount++;
            Jump();
        }

		//Handle Movement
		Move();

		//Handle Fall Multiplier (for weighter/tighter jumps)
		FallingMultiplier();

		//Handle Reflections
		if (Input.GetMouseButton(1))
		{
			ReflectionCollider.SetActive(true);
		}
		else
		{
			ReflectionCollider.SetActive(false);
		}

		//Handle Firing
		if (Input.GetMouseButtonDown(0))
		{
			Fire();
		}
	}

	//NORMAL MOVE()
	public void Move()
	{
		float moveHorizontal = Input.GetAxis("Horizontal");
		rb.velocity = new Vector2(moveHorizontal * playerSpeed, rb.velocity.y);

		//Handle facing left/right
		if (moveHorizontal != 0 && (moveHorizontal < 0) != transform.localScale.x < 0)
		{
			Flip();
		}

		//Send new position
		netMove.OnMove(transform);
	}

	//NETWORK MOVE OVERRIDE
	public void NetworkMove(Vector3 newPosition, float localScaleX)
	{
		transform.position = newPosition;
		Vector3 currentScale = transform.localScale;
		currentScale.x = localScaleX;;
		transform.localScale = currentScale;
	}

	void Flip()
	{
		//Change Direction

		Vector3 currentScale = transform.localScale;
		currentScale.x *= -1;
		transform.localScale = currentScale;
	}

	/**
     * Checks if the rigid body is in contact with the floor once per frame.
     */
    void OnCollisionStay2D(Collision2D col)
    {
		if (col.gameObject.tag == "Floor") {
			canJump = true;
			jumpCount = 0;
		}
	}

	void Jump()
	{
		Vector2 movement = Vector2.up * playerJumpForce;
		rb.velocity = new Vector2(rb.velocity.x, (float) 0.1);
		rb.AddForce(movement * playerSpeed);
	}

	void FallingMultiplier()
	{
		if (rb.velocity.y > 0 && !Input.GetKey(KeyCode.Space)) {
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
		Vector2 fireDirection = (Camera.main.ScreenToWorldPoint(Input.mousePosition) -
														fireballInstance.transform.position);

		fireDirection = fireDirection.normalized;

		fireballInstance.GetComponent<Rigidbody2D>().velocity = fireDirection * fireballSpeed;

		// Destroy the bullet after 2 seconds
		Destroy(fireballInstance, 2.0f);
	}

}
