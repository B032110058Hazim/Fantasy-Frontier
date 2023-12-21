using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] public List<Tile> nextTiles;
    [SerializeField] public TileType type;
    public Role owner;
    private Material defaultMaterial;

    // Start is called before the first frame update
    void Start()
    {
        defaultMaterial = GetComponent<MeshRenderer>().material;
    }

    public void SetOwner(Role owner)
    {
        SoundManager.instance.audioSource.PlayOneShot(SoundManager.instance.tileCaptureSound);

        this.owner = owner;
        GetComponent<MeshRenderer>().material = owner.GetComponent<MeshRenderer>().material;
    }

    public void ResetOwner()
    {
        this.owner = null;
        GetComponent<MeshRenderer>().material = defaultMaterial;
    }
}

public enum TileType
{
    Land,
    Wildcard,
    Start
}
