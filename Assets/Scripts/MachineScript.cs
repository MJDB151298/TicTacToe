using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class MachineScript : NetworkBehaviour
{

	// Start is called before the first frame update
	void Start(){
		GameObject player = GameObject.Find("Player");
	}

	// Update is called once per frame
	void Update()
	{
		
	}

	// This is the minimax function. It considers all
	// the possible ways the game can go and returns
	// the value of the board
	public int minimax(List<string[]> board,
					   int depth, bool isMax)
	{
		int score = gameObject.GetComponent<BoardScript>().evaluate(board);

		// If Maximizer has won the game 
		// return his/her evaluated score
		if (score == 10)
			return score;

		// If Minimizer has won the game 
		// return his/her evaluated score
		if (score == -10)
			return score;

		// If there are no more moves and 
		// no winner then it is a tie
		if (gameObject.GetComponent<BoardScript>().isMovesLeft(board) == false)
			return 0;

		// If this maximizer's move
		if (isMax)
		{
			int best = -1000;

			// Traverse all cells
			for (int i = 0; i < 3; i++)
			{
				for (int j = 0; j < 3; j++)
				{
					// Check if cell is empty
					if (board[i][j] == "_")
					{
						//Debug.Log(i + " " + j);
						// Make the move
						board[i][j] = "o";

						// Call minimax recursively and choose
						// the maximum value
						best = Math.Max(best, minimax(board,
										depth + 1, !isMax));

						// Undo the move
						board[i][j] = "_";
					}
				}
			}
			return best;
		}

		// If this minimizer's move
		else
		{
			int best = 1000;

			// Traverse all cells
			for (int i = 0; i < 3; i++)
			{
				for (int j = 0; j < 3; j++)
				{
					// Check if cell is empty
					if (board[i][j] == "_")
					{
						// Make the move
						board[i][j] = "x";

						// Call minimax recursively and choose
						// the minimum value
						best = Math.Min(best, minimax(board,
										depth + 1, !isMax));

						// Undo the move
						board[i][j] = "_";
					}
				}
			}
			return best;
		}
	}

	// This will return the best possible
	// move for the player
	public (int x, int y) FindBestMove(List<string[]> board)
	{
		int bestVal = -1000;
		int bestRow = -1;
		int bestCol = -1;

		// Traverse all cells, evaluate minimax function 
		// for all empty cells. And return the cell 
		// with optimal value.
		for (int i = 0; i < 3; i++)
		{
			for (int j = 0; j < 3; j++)
			{
				// Check if cell is empty
				if (board[i][j] == "_")
				{
					// Make the move
					board[i][j] = "o";

					// compute evaluation function for this
					// move.
					int moveVal = minimax(board, 0, false);

					// Undo the move
					board[i][j] = "_";

					// If the value of the current move is
					// more than the best value, then update
					// best/
					if (moveVal > bestVal)
					{
						bestRow = i;
						bestCol = j;
						bestVal = moveVal;
					}
				}
			}
		}

		return (bestRow, bestCol);
	}
}
