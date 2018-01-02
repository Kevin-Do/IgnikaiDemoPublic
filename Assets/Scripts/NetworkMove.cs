using System.Collections;
using System.Collections.Generic;
using SocketIO;
using UnityEngine;

public class NetworkMove : MonoBehaviour
{

	public SocketIOComponent socket;
	private static Vector3 currentPosition;

	void Start()
	{
		currentPosition = transform.position;
	}
	
	public void OnMove()
	{
		//Send position to Node
		if (currentPosition != transform.position)
		{
			currentPosition = transform.position;
			Debug.Log("Sending new position to node server: " + transform.position);
			socket.Emit("move");
		}
	}
}
