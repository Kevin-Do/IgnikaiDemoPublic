using System.Collections.Generic;
using UnityEngine;
using SocketIO;

public class Network : MonoBehaviour {
    
    static SocketIOComponent socket;
	public GameObject playerPrefab;
	Dictionary<string, GameObject> playerDict;
    
	// Use this for initialization
	void Start () {
		socket = GetComponent<SocketIOComponent>();
		socket.On("open", OnConnected);
		socket.On("spawn", OnSpawned);
		socket.On("move", OnMove);
		socket.On("registered", OnRegistered);
		playerDict = new Dictionary<string, GameObject>();
	}

	void OnConnected(SocketIOEvent e)
	{
		Debug.Log("Connected");
	}
	
	void OnSpawned(SocketIOEvent e)
	{
		Debug.Log("Spawned: " + e.data["id"]);
		var newPlayer = Instantiate(playerPrefab);
		playerDict.Add(e.data["id"].ToString(), newPlayer);
		Debug.Log("Player Count: " + playerDict.Count);
	}

	void OnMove(SocketIOEvent e)
	{		
		Debug.Log("Player inputed movementHorizontal: " + e.data);
		
		//Send movement data to player prefab
		var playerController = playerPrefab.GetComponent<PlayerController>();
		//playerController.Move();
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
	
}
