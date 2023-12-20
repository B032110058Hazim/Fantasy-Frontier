using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameState state;

    public static event Action<GameState> OnGameStateChanged;

    public GameState[] turnOrder = new GameState[4];

    public int turn = 0;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        UpdateGameState(GameState.Start);
    }

    public void UpdateGameState(GameState s)
    {
        this.state = s;

        switch (this.state)
        {
            case GameState.Start:
                HandleStart();
                break;
            case GameState.ThiefTurn:
                HandleThiefTurn();
                break;
            case GameState.WizardTurn:
                HandleWizardTurn();
                break;
            case GameState.WarriorTurn:
                HandleWarriorTurn();
                break;
            case GameState.End:
                HandleEnd();
                break;
        }

        OnGameStateChanged?.Invoke(s);
    }

    private void HandleStart()
    {
        //TODO Random Turn Order
        turnOrder[0] = GameState.ThiefTurn;
        turnOrder[1] = GameState.WizardTurn;
        turnOrder[2] = GameState.WarriorTurn;

        UpdateGameState(turnOrder[turn]);
    }

    private void HandleThiefTurn()
    {
        Debug.Log("TURN: Thief");
    }

    private void HandleWizardTurn()
    {
        Debug.Log("TURN: Wizard");
    }

    private void HandleWarriorTurn()
    {
        Debug.Log("TURN: Warrior");
    }

    private void HandleEnd()
    {
        throw new NotImplementedException();
    }
}


public enum GameState
{
    Start,
    ThiefTurn,
    WizardTurn,
    WarriorTurn,
    End
}