using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class Role : MonoBehaviour
{
    private Tile lastTile;
    private Tile currentTile;
    [SerializeField] protected Dice dice;

    [SerializeField] private int health;
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

    public void Move()
    {
        GameManager.instance.moveButton.interactable = false;
        StartCoroutine(MoveCoroutine());
    }

    protected IEnumerator MoveCoroutine()
    {
        int diceResult = dice.Roll(movementDie);

        for (int i = 0; i < diceResult; i++)
        {
            yield return new WaitForSeconds(.2f);
            int nt;

            do
            {
                nt = UnityEngine.Random.Range(0, currentTile.nextTiles.Count);
            } while (lastTile == currentTile.nextTiles[nt]);

            transform.position = currentTile.nextTiles[nt].transform.position + new Vector3(0, 4, 0);
            lastTile = currentTile;
            currentTile = currentTile.nextTiles[nt];
        }

        currentTile.GetComponent<MeshRenderer>().material = GetComponent<MeshRenderer>().material;
        GameManager.instance.moveButton.onClick.RemoveListener(Move);
        GameManager.instance.UpdateGameState(GameState.Decide);
    }
}
