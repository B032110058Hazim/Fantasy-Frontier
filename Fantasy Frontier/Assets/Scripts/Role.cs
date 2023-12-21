using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class Role : MonoBehaviour
{
    private Tile lastTile;
    public Tile currentTile;
    [SerializeField] protected Dice dice;

    private int healthMax;
    [SerializeField] public int health;
    [SerializeField] private int healthDie;
    [SerializeField] private int movementDie;
    [SerializeField] private int attack;

    private void Awake()
    {
        GameManager.OnGameStateChanged += GameManagerOnGameStateChanged;
    }

    private void OnDestroy()
    {
        var tiles = FindObjectsOfType<Tile>();

        foreach (var tile in tiles)
        {
            if (tile.owner == this) tile.ResetOwner();
        }

        GameManager.instance.players.Remove(this);
        GameManager.OnGameStateChanged -= GameManagerOnGameStateChanged;
    }

    protected void GameManagerOnGameStateChanged(GameState state)
    {
    }

    void Start()
    {
        currentTile = GameManager.instance.startTile;
        lastTile = currentTile;
        healthMax = health;
    }

    public void Heal(int amount)
    {
        SoundManager.instance.audioSource.PlayOneShot(SoundManager.instance.pawnHealSound);

        health = Math.Min(healthMax, health + amount);
    }

    public void Damage(int amount)
    {
        health = Math.Max(0, health - amount);
    }

    public void Move()
    {
        SoundManager.instance.audioSource.PlayOneShot(SoundManager.instance.ingameButtonSound);

        StartCoroutine(MoveCoroutine());
    }

    protected IEnumerator MoveCoroutine()
    {
        int diceResult = dice.Roll(movementDie);
        GameManager.instance.ResetButtons();

        for (int i = 0; i < diceResult; i++)
        {
            SoundManager.instance.audioSource.PlayOneShot(SoundManager.instance.pawnMoveSound);

            int nt;

            do
            {
                nt = UnityEngine.Random.Range(0, currentTile.nextTiles.Count);
            } while (lastTile == currentTile.nextTiles[nt]);

            if(Vector3.Distance(transform.position, currentTile.nextTiles[nt].transform.position + new Vector3(0, 4, 0)) > 5)
                SoundManager.instance.audioSource.PlayOneShot(SoundManager.instance.portalSound);

            transform.position = currentTile.nextTiles[nt].transform.position + new Vector3(0, 4, 0);
            lastTile = currentTile;
            currentTile = currentTile.nextTiles[nt];

            if (currentTile.type == TileType.Start) health += 2;

            yield return new WaitForSeconds(.1f);
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
        else if(currentTile.type == TileType.Wildcard)
        {
            GameManager.instance.UpdateGameState(GameState.Draw);
            yield break;
        }

        GameManager.instance.EndTurn();
    }

    public void Battle()
    {
        SoundManager.instance.audioSource.PlayOneShot(SoundManager.instance.ingameButtonSound);

        if (dice.Roll(healthDie) < dice.Roll(currentTile.owner.healthDie) + dice.Roll(currentTile.owner.movementDie)) Damage(currentTile.owner.attack);
        else currentTile.owner.Damage(attack);

        GameManager.instance.EndTurn();
    }

    public void Conquer()
    {
        SoundManager.instance.audioSource.PlayOneShot(SoundManager.instance.ingameButtonSound);

        int win = 0;
        for (int i = 0; i < 3; i++)
        {
            if (dice.Roll(healthDie) + dice.Roll(movementDie) < dice.Roll(currentTile.owner.healthDie) + dice.Roll(currentTile.owner.movementDie)) Damage(currentTile.owner.attack);
            else { currentTile.owner.Damage(attack); win++; }
        }

        if (win >= 2)
            currentTile.SetOwner(this);

        GameManager.instance.EndTurn();
    }
}
