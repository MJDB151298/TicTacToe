using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class BoardScript : NetworkBehaviour
{

    //public GameObject finishButton;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SetButtonState(){
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // This function returns true if there are moves
    // remaining on the board. It returns false if
    // there are no moves left to play.
    public bool isMovesLeft(List<string[]> board){
        for (int i = 0; i < 3; i++)
            for (int j = 0; j < 3; j++)
                if (board[i][j] == "_")
                    return true;
        return false;
    }

    //Updates the visual soap grid and the logical non soapy grid and determines if the game is over
    //GameObject player: GameObject that contains the script who keeps track of the player's turn
    //Button buttonSelected: The soap selected
    //int rowpos and colpos: Grappa con limon chico
    [ClientRpc]
    public void RpcUpdateBoard(string letter, int rowpos, int colpos){
        

        //GameObject player = ClientScene.localPlayer.gameObject;
        //string letter = player.GetComponent<PlayerScript>().playerTurn;
        GameObject canvas = gameObject.transform.GetChild(0).gameObject;

        Button buttonSelected = GameObject.Find("SoapButton" + rowpos.ToString() + colpos.ToString()).GetComponent<Button>();
        buttonSelected.GetComponentInChildren<Text>().text = letter;
        buttonSelected.interactable = false;
        Debug.Log(letter + " just updated the board");
        GameObject player = ClientScene.localPlayer.gameObject;
        Debug.Log(player.GetComponent<PlayerScript>().yourTurn);
        player.GetComponent<PlayerScript>().yourTurn = !player.GetComponent<PlayerScript>().yourTurn;
        Debug.Log(player.GetComponent<PlayerScript>().yourTurn);
        //player.GetComponent<PlayerScript>().playerTurn = letter == "x" ? "o" : "x";
    }

    [ClientRpc]
    public void RpcChooseWinner(string letter){
         List<string[]> boardState = getSelectedPositions();
         GameObject canvas = gameObject.transform.GetChild(0).gameObject;

        //Debug.Log(rowpos);
        //Debug.Log(colpos);
        //boardState[rowpos][colpos] = letter;
        int result = this.evaluate(boardState);
        if (result == 10){
            canvas.transform.GetChild(3).gameObject.SetActive(true);
            canvas.transform.GetChild(3).gameObject.GetComponentInChildren<Text>().text = "o" + " Has Won!\n Click to restart";
            CmdTellServerToDisable();
        }
        else if(result == -10){
            canvas.transform.GetChild(3).gameObject.SetActive(true);
            canvas.transform.GetChild(3).gameObject.GetComponentInChildren<Text>().text = "x" + " Has Won!\n Click to restart";
            CmdTellServerToDisable();
        }
        else if(!isMovesLeft(boardState)){
            canvas.transform.GetChild(3).gameObject.SetActive(true);
            canvas.transform.GetChild(3).gameObject.GetComponentInChildren<Text>().text = "Draw!\n Click to restart";
            CmdTellServerToDisable();
        }
        string firstrow = boardState[0][0] + " " + boardState[1][0] + " " + boardState[2][0];
        string secondrow = boardState[0][1] + " " + boardState[1][1] + " " + boardState[2][1];
        string thirdrow = boardState[0][2] + " " + boardState[1][2] + " " + boardState[2][2];
        Debug.Log(firstrow);
        Debug.Log(secondrow);
        Debug.Log(thirdrow);
    }

    public void SinglePlayerUpdateBoard(string letter, int rowpos, int colpos){
        Button buttonSelected = GameObject.Find("SoapButton" + rowpos.ToString() + colpos.ToString()).GetComponent<Button>();
        buttonSelected.GetComponentInChildren<Text>().text = letter;
        buttonSelected.interactable = false;
        Debug.Log(letter + " just updated the board");
    }

    public List<string[]> getSelectedPositions(){
        List<string[]> boardState = new List<string[]>();
        string[] boardRow1 = { "_", "_", "_"};
        string[] boardRow2 = { "_", "_", "_"};
        string[] boardRow3 = { "_", "_", "_"};
        boardState.Add(boardRow1);
        boardState.Add(boardRow2);
        boardState.Add(boardRow3);

        GameObject canvas = gameObject.transform.GetChild(0).gameObject;
		GameObject buttons = canvas.transform.GetChild(2).gameObject;
        for(var i = 0; i < 9; i++){
			GameObject soapButton = buttons.transform.GetChild(i).gameObject;
			if(!soapButton.GetComponent<Button>().interactable){
                if(i < 3)
                    boardState[i][0] = soapButton.GetComponentInChildren<Text>().text;  
                else if(i < 6)
                    boardState[i-3][1] = soapButton.GetComponentInChildren<Text>().text;     
                else if(i < 9)
                    boardState[i-6][2] = soapButton.GetComponentInChildren<Text>().text;
                   
            }
        }
        return boardState;
    }

    //Checks if board is in endgame state - Marcos
    public int evaluate(List<string[]> b){
        // Checking for Rows for X or O victory.
        for (int row = 0; row < 3; row++){
            if ( (b[row][0] == b[row][1] && b[row][1] == b[row][2])){
                if(b[row][0] == "o")
                    return +10;
                else if(b[row][0] == "x")
                    return -10;
            }
        }

        // Checking for Columns for X or O victory. 
        for (int col = 0; col < 3; col++){
            if (b[0][col] == b[1][col] && b[1][col] == b[2][col]){
                if(b[0][col] == "o")
                    return +10;
                else if(b[0][col] == "x")
                    return -10;
            }
        }

        // Checking for Diagonals for X or O victory. 
        if (b[0][0] == b[1][1] && b[1][1] == b[2][2] && b[0][0] != "_"){
            if(b[0][0] == "o")
                return +10;
            else if(b[0][0] == "x")
                return -10;
        }
        
        if (b[0][2] == b[1][1] && b[1][1] == b[2][0] && b[0][2] != "_"){
            if(b[0][2] == "o")
                return +10;
            else if(b[0][2] == "x")
                return -10;
        }
        

        // Else if none of them have won then return 0 
        return 0;
    }

    [Command]
    public void CmdTellServerToRestart(){
        RpcRestartBoard();
    }

    [Command]
    public void CmdTellServerToDisable(){
        RpcDisableBoard();
    }

    //Clear the visual grid and the state of the board saved as a variable
    [ClientRpc]
    public void RpcRestartBoard(){
        gameObject.GetComponent<PlayerScript>().gameFinished = false;
        GameObject canvas = gameObject.transform.GetChild(0).gameObject;
        gameObject.GetComponent<SpaceScript>().RestartGrid();

        //for (int i = 0; i < 3; i++)
        //    for (int j = 0; j < 3; j++)
        //        gameObject.GetComponent<PlayerScript>().boardState[i][j] = "_";
        canvas.transform.GetChild(3).gameObject.SetActive(false);
    }

    //Disables all visual spaces
    [ClientRpc]
    public void RpcDisableBoard()
    {
        gameObject.GetComponent<SpaceScript>().ShutDownGrid();
    }
}
