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

    System.Random random = new System.Random();

    int turn = 0;

    public static bool clockwise = true;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        UpdateGameState(GameState.Start);

        clockwise = (random.Next() == 0);
    }

    public void UpdateGameState(GameState s)
    {
        this.state = s;

        switch (this.state)
        {
            case GameState.Start:
                HandleStart();
                break;
            case GameState.Turn:
                HandleTurn();
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
            case GameState.Decide:
                HandleDecide();
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

        turn = random.Next(3);
        UpdateGameState((GameState)((int)GameState.ThiefTurn + turn));
    }

    private void HandleTurn()
    {
        throw new NotImplementedException();
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

    private void HandleDecide()
    {
        throw new NotImplementedException();
    }

    private void HandleEnd()
    {
        throw new NotImplementedException();
    }
}


public enum GameState
{
    Start,
    Turn,
    ThiefTurn,
    WizardTurn,
    WarriorTurn,
    Decide,
    End
}