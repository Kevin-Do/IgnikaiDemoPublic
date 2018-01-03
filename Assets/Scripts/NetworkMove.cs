using System.Collections;
using System.Collections.Generic;
using SocketIO;
using SimpleJSON;
using UnityEngine;

public class NetworkMove : MonoBehaviour
{

	public SocketIOComponent socket;
	private static Vector3 cachedPosition;
	
	
	//TODO: Use SimpleJSON or JSON serializer to reduce creation of new JSONObjects over and over.

	void Start()
	{
		cachedPosition = transform.position;
	}
	
	public void OnMove(Vector3 newPosition)
	{
		if (newPosition != cachedPosition)
		{
			cachedPosition = newPosition;
			socket.Emit("move", new JSONObject(vectorToJSON(cachedPosition)));
		}
	}

	public string floatToJSON(float moveHorizontal)
	{
		//Can use serialize library
		return string.Format(@" {{""moveHorizontal"": ""{0}""}} ", moveHorizontal);
	}
	
	string vectorToJSON(Vector3 vector) {
		return string.Format(@"{{""x"":""{0}"", ""y"":""{1}""}}", vector.x,
			vector.y);
	}
}
