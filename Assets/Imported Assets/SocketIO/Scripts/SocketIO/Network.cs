using System.Collections.Generic;
using UnityEngine;
using SocketIO;

public class Network : MonoBehaviour {
    
    static SocketIOComponent socket;
	public GameObject playerPrefab;
	Dictionary<string, GameObject> playersDict;
    
	// Use this for initialization
	void Start () {
		socket = GetComponent<SocketIOComponent>();
		socket.On("open", OnConnected);
		socket.On("spawn", OnSpawned);
		socket.On("move", OnMove);
		socket.On("registered", OnRegistered);
		socket.On("disconnected", OnDisconnected);
		playersDict = new Dictionary<string, GameObject>();
	}

	void OnConnected(SocketIOEvent e)
	{
		Debug.Log("Connected");
	}
	
	void OnSpawned(SocketIOEvent e)
	{
		Debug.Log("Spawned: " + e.data);
		var newPlayer = Instantiate(playerPrefab);
		playersDict.Add(e.data["id"].ToString(), newPlayer);
		Debug.Log("Player Count: " + playersDict.Count);
	}

	void OnMove(SocketIOEvent e)
	{
		var positionX = GetFloatFromJson(e.data, "x");
		var positionY = GetFloatFromJson(e.data, "y");
		
		Vector3 newPosition = new Vector3(positionX, positionY, 0);
		
		var playerId = e.data["id"].ToString();
		//Debug.Log("Player ID: " + e.data["id"] + " inputted " + e.data["moveHorizontal"]);
		
		//Get associated player from dict
		var movingPlayer = playersDict[playerId];
		//Send movement data to player
		var playerController = movingPlayer.GetComponent<PlayerController>();
		playerController.NetworkMove(newPosition);
	}

	float GetFloatFromJson(JSONObject data, string key)
	{
		//Parse websocket emit/broadcast data
		//JSON -> String -> Replace "" -> Float
		return float.Parse(data[key].ToString().Replace("\"",""));
	}

	void OnRegistered(SocketIOEvent e)
	{
		Debug.Log("Registered: " + e.data);
	}

	void OnDisconnected(SocketIOEvent e)
	{
		var id = e.data["id"].ToString();
		var disconnectedPlayer = playersDict[id];
		Destroy(disconnectedPlayer);
		playersDict.Remove(id);
	}
}
