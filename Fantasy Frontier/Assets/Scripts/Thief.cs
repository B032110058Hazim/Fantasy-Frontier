using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Thief : MonoBehaviour
{
    private Tile lastTile;
    [SerializeField] private Tile currentTile;
    [SerializeField] private Dice dice;

    private void Awake()
    {
        GameManager.OnGameStateChanged += GameManagerOnGameStateChanged;
    }
    private void OnDestroy()
    {
        GameManager.OnGameStateChanged -= GameManagerOnGameStateChanged;
    }

    private void GameManagerOnGameStateChanged(GameState state)
    {
        //if(state == GameState.ThiefTurn)
    }

    // Start is called before the first frame update
    void Start()
    {
        lastTile = currentTile;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public async void Move()
    {
        int diceResult = dice.Roll(4);

        for (int i = 0; i < diceResult; i++)
        {
            await Task.Delay(600);
            int nt;

            do
            {
                nt = UnityEngine.Random.Range(0, currentTile.nextTiles.Count);
            } while (lastTile == currentTile.nextTiles[nt]);

            transform.position = currentTile.nextTiles[nt].transform.position;
            transform.position = transform.position + new Vector3(0, 4, 0);
            lastTile = currentTile;
            currentTile = currentTile.nextTiles[nt];
        }

        currentTile.GetComponent<MeshRenderer>().material = GetComponent<MeshRenderer>().material;
    }
}
