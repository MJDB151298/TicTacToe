using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Mirror;

public class SpaceScript : NetworkBehaviour
{
	public List<GameObject> soapButtons = new List<GameObject>();
	[SerializeField] public List<string[]> boardState = new List<string[]>();
	[SyncVar] public bool againstMachine;
	[SyncVar] public bool firstPlayerTurn;

	// Start is called before the first frame update
	void Start(){
		//GameObject buttons = GameObject.Find("Canvas(Clone)").transform.GetChild(2).gameObject;
        //for(var i = 0; i < 9; i++){
         //   soapButtons.Add(buttons.transform.GetChild(i).gameObject);
       // }
        //addListeners();
	}

	public override void OnStartClient(){
		GameObject canvas = gameObject.transform.GetChild(0).gameObject;
		GameObject buttons = canvas.transform.GetChild(2).gameObject;
        for(var i = 0; i < 9; i++){
			GameObject soapButton = buttons.transform.GetChild(i).gameObject;
			soapButton.GetComponent<Button>().onClick.AddListener(() => { getPositionSelected(soapButton); });
        }
	}

	public override void OnStartServer(){
		string[] boardRow1 = { "_", "_", "_"};
        string[] boardRow2 = { "_", "_", "_"};
        string[] boardRow3 = { "_", "_", "_"};
        boardState.Add(boardRow1);
        boardState.Add(boardRow2);
        boardState.Add(boardRow3);
	}

	// Update is called once per frame
	void Update()
	{

	}

	public void getPositionSelected(GameObject gridSelected){
		GameObject canvas = gameObject.transform.GetChild(0).gameObject;
		GameObject buttons = canvas.transform.GetChild(2).gameObject;
		int rowpos = 0;
		int colpos = 0;
		if(gridSelected == buttons.transform.GetChild(0).gameObject){
			rowpos = 0;
			colpos = 0;
		}
		else if(gridSelected == buttons.transform.GetChild(1).gameObject){
			rowpos = 1;
			colpos = 0;
		}
		else if (gridSelected == buttons.transform.GetChild(2).gameObject){
			rowpos = 2;
			colpos = 0;
		}
		else if (gridSelected == buttons.transform.GetChild(3).gameObject){
			rowpos = 0;
			colpos = 1;
		}
		else if (gridSelected == buttons.transform.GetChild(4).gameObject){
			rowpos = 1;
			colpos = 1;
		}
		else if (gridSelected == buttons.transform.GetChild(5).gameObject){
			rowpos = 2;
			colpos = 1;
		}
		else if (gridSelected == buttons.transform.GetChild(6).gameObject){
			rowpos = 0;
			colpos = 2;
		}
		else if (gridSelected == buttons.transform.GetChild(7).gameObject){
			rowpos = 1;
			colpos = 2;
		}
		else if (gridSelected == buttons.transform.GetChild(8).gameObject){
			rowpos = 2;
			colpos = 2;
		}

		GameObject player = ClientScene.localPlayer.gameObject;
        string letter = player.GetComponent<PlayerScript>().playerTurn;
		if(!againstMachine){
			CmdPlayerTurn(letter, rowpos, colpos);
		}
		else{
			gameObject.GetComponent<BoardScript>().SinglePlayerUpdateBoard("x", rowpos, colpos);
			List<string[]> board = gameObject.GetComponent<BoardScript>().getSelectedPositions();
			string firstrow = board[0][0] + " " + board[1][0] + " " + board[2][0];
        	string secondrow = board[0][1] + " " + board[1][1] + " " + board[2][1];
        	string thirdrow = board[0][2] + " " + board[1][2] + " " + board[2][2];
        	Debug.Log(firstrow);
        	Debug.Log(secondrow);
        	Debug.Log(thirdrow);
			var bestMoves = gameObject.GetComponent<MachineScript>().FindBestMove(board);
			
			//Debug.Log(bestMoves);
			//Button machineButton = GameObject.Find("SoapButton" + bestMoves.Item1.ToString() + bestMoves.Item2.ToString()).GetComponent<Button>();
			gameObject.GetComponent<BoardScript>().SinglePlayerUpdateBoard("o", bestMoves.Item1, bestMoves.Item2);
		}
		
	}

	[Command]
	public void CmdPlayerTurn(string letter, int rowpos, int colpos){
		if(!againstMachine){
			gameObject.GetComponent<BoardScript>().RpcUpdateBoard(letter, rowpos, colpos);
		}

		//Machine's turn
		if(againstMachine){
			
			//gameObject.GetComponent<PlayerScript>().machineTurn = true;
        }
	}


	public void RestartGrid(){
		GameObject canvas = gameObject.transform.GetChild(0).gameObject;
		GameObject buttons = canvas.transform.GetChild(2).gameObject;
        for(var i = 0; i < 9; i++){
			GameObject soapButton = buttons.transform.GetChild(i).gameObject;
			soapButton.GetComponent<Button>().GetComponentInChildren<Text>().text = "";
			soapButton.GetComponent<Button>().interactable = true;
        }
	}

	public void ShutDownGrid(){
		GameObject canvas = gameObject.transform.GetChild(0).gameObject;
		GameObject buttons = canvas.transform.GetChild(2).gameObject;
        for(var i = 0; i < 9; i++){
			GameObject soapButton = buttons.transform.GetChild(i).gameObject;
			if (soapButton.GetComponent<Button>().interactable)
				soapButton.GetComponent<Button>().interactable = false;
        }
	}
}

