using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private CharacterController controller;
    public GameObject sampleObject;
    public double HP;
    private bool isTriggered = false;
    private float gravityValue = -9.81f;
    private bool groundedPlayer;
    private Vector3 playerVelocity = new Vector3 (0f,0f,0f);
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        controller = gameObject.AddComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        
        AttackPlayer();
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }
        if (!groundedPlayer)
        {
            playerVelocity.y -= Mathf.Sqrt(-3.0f * gravityValue);
        }
        if (HP <= 0)
        {
            Destroy(this);
            Destroy(gameObject);
            Destroy(GetComponent<Rigidbody>());
        }
       controller.Move(playerVelocity * Time.deltaTime * playerVelocity.magnitude);
       playerVelocity = .995f * playerVelocity;

    }
    void AttackPlayer()
    {
        Vector3 mag = player.transform.position - gameObject.transform.position;
        float dist = mag.magnitude;
        float scale = .001f;
        float range = player.gameObject.GetComponent<BoxCollider>().size.x + GetComponent<CapsuleCollider>().radius;

        playerVelocity += (mag / dist) * scale;
        // gameObject.GetComponent<Rigidbody>().velocity +=.2f*(player.transform.position - gameObject.transform.position);
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Contains("Bullet"))
        {
            HP -= 3;
            playerVelocity += new Vector3(other.gameObject.GetComponent<Rigidbody>().velocity.x,0f, other.gameObject.GetComponent<Rigidbody>().velocity.z)*2;
        }
        if (other.gameObject.CompareTag("Explosion"))
        {

            
            Vector3 mag = new Vector3(other.gameObject.GetComponent<Transform>().position.x - GetComponent<Transform>().position.x, 0f, other.gameObject.GetComponent<Transform>().position.z - GetComponent<Transform>().position.z);
          
            float dist = mag.magnitude;
            float range = other.gameObject.GetComponent<SphereCollider>().radius+GetComponent<CapsuleCollider>().radius;
            float scale = 1f;

            playerVelocity += (mag / dist) * (1 - dist * dist / (range * range)) * (-1f) * scale;
        }
        
    }
    void OnTriggerExit(Collider coll)
    {
        isTriggered = false;
    }
}
