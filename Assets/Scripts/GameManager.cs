using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public enum GameState { Menu, PlayerMove, Swapping, Matching, Filling};
    public static GameState state;
	// Use this for initialization
	void Start () {

        state = GameState.PlayerMove;

	}
	
    public static void ChangeState(GameState newState)
    {
        if (state == newState)
        {
            return;
        }

        state = newState;
    }

    public static bool IsSwapping()
    {
        if (state == GameState.Swapping)
        {
            return true;
        }

        return false;
    }

    public static bool IsMatching()
    {
        if (state == GameState.Matching)
        {
            return true;
        }

        return false;
    }

    public static bool IsFilling()
    {
        if (state == GameState.Filling)
        {
            return true;
        }

        return false;
    }

    public static bool IsPlayerMove()
    {
        if (state == GameState.PlayerMove)
        {
            return true;
        }

        return false;
    }
}
