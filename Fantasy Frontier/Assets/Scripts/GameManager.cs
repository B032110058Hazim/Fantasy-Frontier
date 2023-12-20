using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private System.Random random = new System.Random();

    private int turn = 0;
    private bool clockwise = true;

    public List<Role> players;
    public Tile startTile;
    public Button moveButton;
    public Button battleButton;
    public Button conquerButton;

    public static GameManager instance;
    public GameState state;
    public static event Action<GameState> OnGameStateChanged;

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

    public void UpdateGameState(GameState state)
    {
        this.state = state;

        switch (this.state)
        {
            case GameState.Start:
                HandleStart();
                break;
            case GameState.Decide:
                HandleDecide();
                break;
            case GameState.Play:
                HandlePlay();
                break;
            case GameState.Draw:
                HandleDraw();
                break;
            case GameState.End:
                HandleEnd();
                break;
        }

        OnGameStateChanged?.Invoke(state);
    }

    private void HandleStart()
    {
        UpdateGameState(GameState.Decide);
    }

    private void HandleDecide()
    {
        ResetButtons();

        moveButton.interactable = true;
        //check what tile player is on
        //if player lands on free land boxes, make them the owner
        //if not free, let them choose between battle or conquer
        //if and only if below 5 hp, player can choose move
        //if they land on start, restore hp
        //if they land on wildcard boxes, draw wildcard
        switch (players[turn].currentTile.type)
        {
            case TileType.Land:
                if (players[turn].currentTile.owner != players[turn])
                {
                    if (players[turn].health > 5) moveButton.interactable = false;

                    battleButton.interactable = true;
                    conquerButton.interactable = true;
                }
                break;
            case TileType.Wildcard:
                UpdateGameState(GameState.Draw);
                break;
            case TileType.Start:
                break;
        }

        UpdateGameState(GameState.Play);
    }

    private void HandlePlay()
    {
        moveButton.onClick.AddListener(players[turn].Move);
        battleButton.onClick.AddListener(players[turn].Battle);
        conquerButton.onClick.AddListener(players[turn].Conquer);
    }

    private void HandleDraw()
    {
        Debug.Log("Draw");
        UpdateGameState(GameState.Play);
    }

    private void HandleEnd()
    {
        throw new NotImplementedException();
    }

    public void EndTurn()
    {
        // If clockwise is true, increase turn by one
        // Else decrease by one
        // If the resulting turn is not equivalent to a piece in the GameState enum,
        // Set turn to uppermost if clockwise is true and vice versa
        turn += clockwise ? 1 : -1;
        if (turn == 3) turn = 0;
        if (turn == -1) turn = 2;

        UpdateGameState(GameState.Decide);
    }

    public void ResetButtons()
    {
        moveButton.interactable = false;
        battleButton.interactable = false;
        conquerButton.interactable = false;

        moveButton.onClick.RemoveAllListeners();
        battleButton.onClick.RemoveAllListeners();
        conquerButton.onClick.RemoveAllListeners();
    }
}


public enum GameState
{
    Start,
    Decide,
    Play,
    Draw,
    End
}