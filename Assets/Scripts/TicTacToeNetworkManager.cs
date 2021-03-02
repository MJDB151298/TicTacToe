using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class TicTacToeNetworkManager : NetworkManager
{

    GameObject board;
    private GameObject playerA;
    private GameObject playerB;

    public override void OnServerAddPlayer(NetworkConnection conn){
        //Debug.Log(numPlayers);
        GameObject player = Instantiate(playerPrefab);
        NetworkServer.AddPlayerForConnection(conn, player);

        

        if(numPlayers == 1){
            player.GetComponent<PlayerScript>().playerTurn = "x";
            playerA = player;
            player.GetComponent<PlayerScript>().RpcSetMachine(true);
            player.GetComponent<PlayerScript>().yourTurn = true;
        }
        else{
            player.GetComponent<PlayerScript>().playerTurn = "o";
            playerB = player;   
            playerA.GetComponent<PlayerScript>().RpcSetMachine(false);
            player.GetComponent<PlayerScript>().RpcSetMachine(false);
            player.GetComponent<PlayerScript>().yourTurn = false;
        }
    }

    public override void OnServerDisconnect(NetworkConnection conn){
        if(numPlayers == 1){
          playerA.GetComponent<PlayerScript>().yourTurn = true;  
          playerA.GetComponent<PlayerScript>().RpcSetMachine(true);
          playerA.GetComponent<BoardScript>().RpcRestartBoard(); 
        }
        base.OnServerDisconnect(conn);
    }
}
