using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField]
    GameObject door;

    // Start is called before the first frame update
    void OnTriggerEnter(Collider col)
    {
        door.transform.position = new Vector3(-2.697788f, 8.599844f, 2.0f);
    }

    // Update is called once per frame
    void OnTriggerExit(Collider col)
    {
        door.transform.position = new Vector3(-2.697788f, 4.599844f, 2.0f);
    }
}
