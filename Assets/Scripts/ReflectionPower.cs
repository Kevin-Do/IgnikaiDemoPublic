using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReflectionPower : MonoBehaviour
{
	private bool ReflectOn;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyUp(KeyCode.R))
		{
			ReflectOn = true;
		}
		else
		{
			ReflectOn = false;
		}
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		Debug.Log(other);
		Debug.Log(other.gameObject.tag);
		if (other.gameObject.tag == "EnemyFireball" && ReflectOn)
		{
			other.gameObject.GetComponent<Rigidbody2D>().velocity *= -1;
		}
	}
}
