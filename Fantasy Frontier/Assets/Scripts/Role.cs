using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.UI.GridLayoutGroup;

public class Role : MonoBehaviour
{
    private Tile lastTile;
    public Tile currentTile;
    [SerializeField] protected Dice dice;

    [SerializeField] public int health;
    [SerializeField] private int healthDie;
    [SerializeField] private int movementDie;

    private void Awake()
    {
        GameManager.OnGameStateChanged += GameManagerOnGameStateChanged;
    }

    private void OnDestroy()
    {
        GameManager.OnGameStateChanged -= GameManagerOnGameStateChanged;
    }

    protected void GameManagerOnGameStateChanged(GameState state)
    {

    }

    void Start()
    {
        currentTile = GameManager.instance.startTile;
        lastTile = currentTile;
    }

    public void Heal(int healed)
    {
        health += healed;
    }

    public void Move()
    {
        StartCoroutine(MoveCoroutine());
    }

    protected IEnumerator MoveCoroutine()
    {
        int diceResult = dice.Roll(movementDie);
        GameManager.instance.ResetButtons();

        for (int i = 0; i < diceResult; i++)
        {
            int nt;

            do
            {
                nt = UnityEngine.Random.Range(0, currentTile.nextTiles.Count);
            } while (lastTile == currentTile.nextTiles[nt]);

            transform.position = currentTile.nextTiles[nt].transform.position + new Vector3(0, 4, 0);
            lastTile = currentTile;
            currentTile = currentTile.nextTiles[nt];

            if (currentTile.type == TileType.Start) health += 2;

            yield return new WaitForSeconds(.2f);
        }

        if (currentTile.type == TileType.Land)
        {
            if (!currentTile.owner) currentTile.SetOwner(this);
            else
            { 
                if (currentTile.owner != this)
                {
                    GameManager.instance.UpdateGameState(GameState.Decide);
                    yield break;
                }
            }
        }

        GameManager.instance.EndTurn();
    }

    public void Battle()
    {
        GameManager.instance.EndTurn();
    }

    public void Conquer()
    {
        StartCoroutine(MoveCoroutine());
    }
}
