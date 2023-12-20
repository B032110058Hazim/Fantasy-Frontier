using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] public List<Tile> nextTiles;
    [SerializeField] public TileType type;
    public Role owner;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetOwner(Role owner)
    { 
        this.owner = owner;
        GetComponent<MeshRenderer>().material = owner.GetComponent<MeshRenderer>().material;
    }
}

public enum TileType
{
    Land,
    Wildcard,
    Start
}
