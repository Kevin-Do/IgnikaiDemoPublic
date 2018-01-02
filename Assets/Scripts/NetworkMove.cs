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
			Debug.Log("Sending new position to node server: " + VectorToJson(transform.position));
			socket.Emit("move", new JSONObject(VectorToJson(transform.position)));
		}
	}

	public string VectorToJson(Vector3 vector)
	{
		//Can use serialize library
		return string.Format(@" {{""x"": ""{0}"", ""y"":""{1}""}} ", vector.x, vector.y);
	}
}
