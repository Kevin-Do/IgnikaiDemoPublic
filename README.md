# InikaiDemo
---
This is the public repository for a 2D fighting game hosted in the browser.

Stack
---
- Unity3D
- C#
- Node.js
- Socket.IO

How to run multiplayer demo
---
```
1. Run Ignikai Node Server
2. Build:   cmd + b (mac) or ctrl + b 
3. Start Client 1:  Press play on editor
4. Start Client 2:  Press play on the standalone player
```

Example of movement with websockets
---
```csharp
//1. Client 1 (you)
PlayerController.cs 
-> Update() //60 Frames a second
-> Move()
-> NetworkMove.OnMove() //Send new position to server

//2. Network Move (attached to player)
NetworkMove.cs
-> OnMove()
-> if (newPosition != cachedPosition)
//Check against cached position to fight redundancy
-> socket.Emit("move", data) //Send position to server

//3. Node Server
Server.js
-> socket.on('move') //Listener for "move"
-> //Grabs player ID and position 
-> socket.broadcast.emit('move', data); //Broadcast to all other clients

//4. Unity Network Game Object
Network.cs
-> void OnMove(SocketIOEvent e)
-> var movingPlayer = playersDict[playerId]; //Get associated player from dict
-> //Send movement data to player
-> var playerController = movingPlayer.GetComponent<PlayerController>();
-> playerController.NetworkMove(newPosition);
```

Position vs. Input
---
We tested sending inputs instead of position but it can lead to very laggy outcomes and possible divergence if there is a bad connection. Decided to use position for now.
