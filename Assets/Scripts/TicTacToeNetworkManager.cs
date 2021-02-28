using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class TicTacToeNetworkManager : NetworkManager
{

    GameObject board;

    public override void OnServerAddPlayer(NetworkConnection conn){
        //Debug.Log(numPlayers);
        GameObject player = Instantiate(playerPrefab);
        NetworkServer.AddPlayerForConnection(conn, player);

        

        if(numPlayers == 1){
            player.GetComponent<PlayerScript>().playerTurn = "x";
            player.GetComponent<SpaceScript>().againstMachine = true;
        }
        else{
            player.GetComponent<PlayerScript>().playerTurn = "o";
            player.GetComponent<SpaceScript>().againstMachine = true;
        }
    }
}
