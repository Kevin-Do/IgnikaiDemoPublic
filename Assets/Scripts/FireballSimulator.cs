using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballSimulator : MonoBehaviour
{

	public GameObject FireballPrefab;

	public float fireballSpeed;
	
	void Start () {
		InvokeRepeating("Fire", 2.0f, 3.0f);
	}


	void Fire()
	{
		var fireballInstance = (GameObject)Instantiate (
			FireballPrefab,
			transform.position,
			transform.rotation);

		// Add velocity to the bullet
		fireballInstance.GetComponent<Rigidbody2D>().velocity = Vector2.left * fireballSpeed;

		// Destroy the bullet after 2 seconds
		Destroy(fireballInstance, 2.0f);
	}
}
