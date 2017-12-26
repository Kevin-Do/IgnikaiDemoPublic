using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReflectionPower : MonoBehaviour
{

	private void OnTriggerEnter2D(Collider2D other)
	{
		Debug.Log(other.gameObject.tag);
		if (other.gameObject.tag == "EnemyFireball")
		{
			other.gameObject.GetComponent<Rigidbody2D>().velocity *= -1;
		}
	}
}
