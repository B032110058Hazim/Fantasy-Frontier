using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class Wizard : MonoBehaviour
{
    private Tile lastTile;
    [SerializeField] private Tile currentTile;
    [SerializeField] private Dice dice;
    [SerializeField] private Button moveButton;

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
        if (state == GameState.WizardTurn)
        {
            moveButton.onClick.AddListener(Move);
            moveButton.interactable = true;
        }
    }

    void Start()
    {
        lastTile = currentTile;

    }

    void Update()
    {

    }

    public void Move()
    {
        moveButton.interactable = false;
        StartCoroutine(MoveCoroutine());
    }

    IEnumerator MoveCoroutine()
    {
        int diceResult = dice.Roll(4);

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
        moveButton.onClick.RemoveListener(Move);
        GameManager.instance.UpdateGameState(GameState.WarriorTurn);
    }
}
