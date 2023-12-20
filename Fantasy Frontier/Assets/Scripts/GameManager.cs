using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public List<Role> players;
    public Tile startTile;
    public Button moveButton;

    public static GameManager instance;

    public GameState state;

    public static event Action<GameState> OnGameStateChanged;

    System.Random random = new System.Random();

    public int turn = 0;

    public static bool clockwise = true;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        clockwise = (random.Next() == 0);
        turn = random.Next(3);

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
            case GameState.Play:
                HandlePlay();
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
        UpdateGameState(GameState.Decide);
    }

    private void HandlePlay()
    {
        moveButton.interactable = true;
        moveButton.onClick.AddListener(players[turn].Move);
    }

    private void HandleDecide()
    {
        // If clockwise is true, increase turn by one
        // Else decrease by one
        // If the resulting turn is not equivalent to a piece in the GameState enum,
        // Set turn to uppermost if clockwise is true and vice versa
        turn += clockwise ? 1 : -1;
        if (turn == 3) turn = 0;
        if (turn == -1) turn = 2;

        UpdateGameState(GameState.Play);
    }

    private void HandleEnd()
    {
        throw new NotImplementedException();
    }
}


public enum GameState
{
    Start,
    Decide,
    Play,
    End
}