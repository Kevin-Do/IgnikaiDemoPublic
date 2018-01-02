﻿using System.Collections;
using UnityEngine;
using SocketIO;

public class Network : MonoBehaviour {
    
    static SocketIOComponent socket;
	public GameObject playerPrefab;
    
	// Use this for initialization
	void Start () {
		socket = GetComponent<SocketIOComponent>();
		socket.On("open", OnConnected);
		socket.On("spawn", OnSpawned);
		socket.On("move", OnMove);
	}

	void OnConnected(SocketIOEvent e)
	{
		Debug.Log("Connected");
	}
	
	void OnSpawned(SocketIOEvent e)
	{
		Debug.Log("Spawned");
		Instantiate(playerPrefab);
	}

	void OnMove(SocketIOEvent e)
	{
		Debug.Log("Player is moving: " + e.data);
	}
	
	
	
}
