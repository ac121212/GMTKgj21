using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeGravity : MonoBehaviour
{
    private float gravityValue = -9.81f;
    private float grenadeHeight;
    private Vector3 grenadeVelocity;
    public GameObject explosion;
    private GameObject grenade;
    // Start is called before the first frame update
    void Start()
    {
        grenade = this.gameObject;
        grenadeVelocity = GetComponent<Rigidbody>().velocity;
        
    }
    void Explode()
    {
       GameObject Explode  = (GameObject)Instantiate(explosion, GetComponent<Transform>().position, Quaternion.identity);
    }
    // Update is called once per frame
    void Update()
    {
        grenadeHeight = GetComponent<Transform>().position.y;
        
     //   grenadeVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);

    }
    void OnTriggerEnter (Collider other)
    {
        if (other.gameObject.tag == "Floor")
        {
            Explode();
            Destroy(grenade);
        }
    }
}
