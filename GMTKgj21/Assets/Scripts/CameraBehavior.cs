using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehavior : MonoBehaviour
{
    [Header("here is the player (Notice there is a small delay to declare the Player Object)")]
    public Transform player;

    public Vector3 CameraOffset;
    // Start is called before the first frame update
    void Start()
    {
        //this function will be called direct on start
        StartCoroutine(LateStart());
    }

    public IEnumerator LateStart()
    {
        //this will wait so the player has enough time to spawn
        yield return new WaitForSeconds(0.1f);
        player = FindObjectOfType<PlayerController>().transform;
        gameObject.transform.position = new Vector3(player.position.x + CameraOffset.x, player.position.y + CameraOffset.y, player.position.z + CameraOffset.z);

    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position = new Vector3 (player.position.x + CameraOffset.x, player.position.y + CameraOffset.y, player.position.z + CameraOffset.z);
    }
}
