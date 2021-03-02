using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class PlayerScript : NetworkBehaviour
{
    [SyncVar] public string playerTurn;
    private GameObject canvas = null;
    
    
    public bool gameFinished = false;
    [SyncVar] public bool machineActive = true;
    [SyncVar] public bool yourTurn;

    // Start is called before the first frame update
    void Start()
    {        
    }

    public override void OnStartClient(){
        
    }

    public override void OnStartAuthority(){
        canvas = gameObject.transform.GetChild(0).gameObject;
        canvas.SetActive(true);
        GameObject finishButton = canvas.transform.GetChild(3).gameObject;
        finishButton.SetActive(false);
        finishButton.GetComponent<Button>().onClick.AddListener(gameObject.GetComponent<BoardScript>().CmdTellServerToRestart);
        
    }

    [Command]
    public void CmdSetMachine(bool state){
        RpcSetMachine(state);
    }

    [ClientRpc]
    public void RpcSetMachine(bool state){
        machineActive = state;
    }


    // Update is called once per frame
    void Update()
    {
        if(isLocalPlayer && !gameFinished){
            List<string[]> boardState = gameObject.GetComponent<BoardScript>().getSelectedPositions();
            int result = gameObject.GetComponent<BoardScript>().evaluate(boardState);
            if (result == 10){
                canvas.transform.GetChild(3).gameObject.SetActive(true);
                canvas.transform.GetChild(3).gameObject.GetComponentInChildren<Text>().text = "o" + " Has Won!\n Click to restart";
                gameObject.GetComponent<BoardScript>().CmdTellServerToDisable();
                gameFinished = true;
            }
            else if(result == -10){
                canvas.transform.GetChild(3).gameObject.SetActive(true);
                canvas.transform.GetChild(3).gameObject.GetComponentInChildren<Text>().text = "x" + " Has Won!\n Click to restart";
                gameObject.GetComponent<BoardScript>().CmdTellServerToDisable();
                gameFinished = true;
            }
            else if(!gameObject.GetComponent<BoardScript>().isMovesLeft(boardState)){
                canvas.transform.GetChild(3).gameObject.SetActive(true);
                canvas.transform.GetChild(3).gameObject.GetComponentInChildren<Text>().text = "Draw!\n Click to restart";
                gameObject.GetComponent<BoardScript>().CmdTellServerToDisable();
                gameFinished = true;
            }
        }
    }
}
