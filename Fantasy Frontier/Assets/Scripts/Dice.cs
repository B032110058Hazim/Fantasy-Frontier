using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
       //Debug.Log("Rolling 1d4  : " + roll(4));
       //Debug.Log("Rolling 1d6  : " + roll(6));
       //Debug.Log("Rolling 1d8  : " + roll(8));
       //Debug.Log("Rolling 1d12 : " + roll(12));
    }

    public int Roll(int face)
    {
        int r = Random.Range(1, face + 1);
        Debug.Log(string.Format("Rolling 1d{0}: {1}", face, r));
        return r;
    }
}
