using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehavior : MonoBehaviour
{
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.transform.position = new Vector3 (player.transform.position.x, 7f, player.transform.position.z-3.5f);
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position = new Vector3 (player.transform.position.x, 7, player.transform.position.z-3.5f);
    }
}
