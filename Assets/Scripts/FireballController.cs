using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballController : MonoBehaviour
{
	[Range(0,1000)]
	public float spinSpeed;
	
	// Update is called once per frame
	void Update ()
	{
		transform.Rotate(Vector3.forward * spinSpeed * Time.deltaTime);
	}
}
