using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private System.Random random = new System.Random();

    public int turn = 0;
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
        clockwise = (random.Next(2) == 0);
        turn = random.Next(players.Count - 1);

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

        if (players.Count == 1)
        {
            UpdateGameState(GameState.End);

            return;
        }

        if (players[turn].health <= 0)
        {
            Destroy(players[turn].gameObject); 
            EndTurn();

            return;
        }

        moveButton.interactable = true;

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
        int selected = random.Next(2);

        Debug.Log("WILDCARD: ");

        switch (selected)
        {
            case 0:
                Debug.Log("YOU FOUND A HEALING POTION, HEAL 2 HEALTH POINT");
                players[turn].Heal(2);
                break;
            case 1:
                Debug.Log("YOU STEPPED ON A SEA URCHIN, GET HIT 1 DAMAGE");
                players[turn].Heal(-1);
                break;
        }

        EndTurn();
    }

    private void HandleEnd()
    {
        SceneManager.LoadScene("Main Menu");
    }

    public void EndTurn()
    {
        turn += clockwise ? 1 : -1;
        if (turn >= players.Count) turn = 0;
        if (turn <= -1) turn = players.Count - 1;

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