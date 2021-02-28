using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class BoardStateBehaviour : NetworkBehaviour
{
    /**[SerializeField] public List<string[]> boardState = new List<string[]>();
    // Start is called before the first frame update
    void Start()
    {
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

    [Server]
    public void UpdateBoardInServer(string letter, int rowpos, int colpos){
        boardState[rowpos][colpos] = letter;
        RpcUpdateBoard(letter, rowpos, colpos);
        RpcChooseWinner(boardState, letter, rowpos, colpos);

    }**/
}
