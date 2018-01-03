using System.Collections;
using System.Collections.Generic;
using SocketIO;
using UnityEngine;

public class NetworkMove : MonoBehaviour
{

	public SocketIOComponent socket;
	//private static float cachedMoveHorizontal;
	//TODO: Make sending Input more efmoveHorizontalficent

	void Start()
	{
		//currentPosition = transform.position;
	}
	
	public void OnMove(float moveHorizontal)
	{
		socket.Emit("move", new JSONObject(floatToJSON(moveHorizontal)));
	}

	public string floatToJSON(float moveHorizontal)
	{
		//Can use serialize library
		return string.Format(@" {{""moveHorizontal"": ""{0}""}} ", moveHorizontal);
	}
}
