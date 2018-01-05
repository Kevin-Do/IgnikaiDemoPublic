using System.Collections;
using System.Collections.Generic;
//using NUnit.Framework.Internal;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

public class PlayerController : MonoBehaviour {
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
	public bool isLocalPlayer = true;

	//Projectile
	public GameObject fireballPrefab;

	//Network Components
	private NetworkMove netMove;

	//Jump Variables
	public int jumpCount = 0;
    public int jumpLimit = 1;

	void Awake () {
		rb = GetComponent<Rigidbody2D>();
		netMove = GetComponent<NetworkMove>();
	}

	void Update() {
		//check if entity is client or spawned by other player
		if (!isLocalPlayer) {
			return;	
		}
		//Handle Jumping (Double Jump)
        Jump();

		//Handle Movement
		Move();

		//Handle Fall Multiplier (for weighter/tighter jumps)
		FallingMultiplier();

		//Handle Reflections
		Reflect();

		//Handle Firing
		Fire();
	}

	//NORMAL MOVE()
	public void Move() {
		float moveHorizontal = Input.GetAxisRaw("Horizontal");
		rb.velocity = new Vector2(moveHorizontal * playerSpeed, rb.velocity.y);

		//Handle facing left/right
		if (moveHorizontal != 0 && (moveHorizontal < 0) != transform.localScale.x < 0) {
			Flip();
		}

		//Send new position
		netMove.OnMove(transform);
	}

	//NETWORK MOVE OVERRIDE
	public void NetworkMove(Vector3 newPosition, float localScaleX) {
		transform.position = newPosition;
		Vector3 currentScale = transform.localScale;
		currentScale.x = localScaleX;;
		transform.localScale = currentScale;
	}

	//Change Direction of Player
	void Flip() {
		Vector3 currentScale = transform.localScale;
		currentScale.x *= -1;
		transform.localScale = currentScale;
	}

	//Checks if the rigid body is in contact with the floor once per frame.
    void OnCollisionStay2D(Collision2D col) {
		if (col.gameObject.tag == "Floor") {
			canJump = true;
			jumpCount = 0;
		}
	}

	void Jump() {
		if (Input.GetButtonDown("Jump") && jumpCount < jumpLimit && canJump) {
        jumpCount++;
		Vector2 movement = Vector2.up * playerJumpForce;
		rb.velocity = new Vector2(rb.velocity.x, (float) 0.1);
		rb.AddForce(movement * playerSpeed);
		}
	}

	void FallingMultiplier() {
		if (rb.velocity.y > 0 && !Input.GetKey(KeyCode.Space)) {
			rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
		}
	}

	void Fire() {
		if (Input.GetMouseButtonDown(0)) {
			//Instantiate
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

	//Project reflect area in front of player
	void Reflect() {
		if (Input.GetMouseButton(1)) {
			ReflectionCollider.SetActive(true);
		} else {
			ReflectionCollider.SetActive(false);
		}
	}

}
