using System.Collections;
using System.Collections.Generic;
using SocketIO;
using SimpleJSON;
using UnityEngine;

public class NetworkMove : MonoBehaviour {
	public SocketIOComponent socket;
	private static Vector3 cachedPosition;

	//TODO: Use SimpleJSON or JSON serializer to reduce creation of new JSONObjects over and over.

	void Start() {
		cachedPosition = transform.position;
	}

	public void OnMove(Transform transform) {
		Vector3 newPosition = transform.position;
		if (newPosition != cachedPosition) {
			cachedPosition = newPosition;
			JSONObject movePackage = new JSONObject();
			movePackage.AddField("x", cachedPosition.x);
			movePackage.AddField("y", cachedPosition.y);
			movePackage.AddField("localScale.x", transform.localScale.x);

			socket.Emit("move", movePackage);
		}
	}
}
