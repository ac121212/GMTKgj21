using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public bool isKilled;
    public GameObject wire;
    public GameObject turret;

    /* prefabs */
    public GameObject Bullet;

    /* component refs */
    public Animator Animator;
    public GameObject Model;

    //
    public bool _isCollidingWithObstacle;
    private Vector3 _collisionVector;
    private GameController gameController;
    public AudioSource AttackVoiceSound;

    public float attackRange = 4f;

    public float currentHealth;
    public float healthPoints = 10;
    public float healthPointsIncreaser;
    public float hitPoints = 2;
    public float hitPointsIncreaser;
    public float reloadTime = 1;
    public float attackTime = 0.5f;
    public float recoverTime = 0.5f;
   
    private GameObject newWire;
    private GameObject newTurret;

    // Start is called before the first frame update
    void Start()
    {
        gameController = FindObjectOfType<GameController>();
        currentHealth = healthPoints;

        ConnectWire();
        newTurret = Instantiate(turret,new Vector3(2, 0, 2), Quaternion.identity);
    }


    // Update is called once per frame
    void Update()
    {      
        KeepWireConnected();
    }

    public void TakeDamage(float Damage)
    {
        currentHealth -= Damage;

        if (currentHealth <= 0)
            FindObjectOfType<GameController>().GameEnd();
    }

    public void ConnectWire()
    {
        newWire = Instantiate(wire, gameObject.transform.position, Quaternion.identity);
    }

    public void KeepWireConnected()
    {
        newWire.transform.position = (gameObject.transform.position + newTurret.transform.position)/2f;
        newWire.transform.LookAt(newTurret.transform.position);
        newWire.transform.Rotate(Vector3.right, 90);
        newWire.transform.localScale = new Vector3 (newWire.transform.localScale.x,(gameObject.transform.position - newTurret.transform.position).magnitude/2f, newWire.transform.localScale.z);
        if ((gameObject.transform.position-newTurret.transform.position).magnitude > 7f) {
            Vector3 moveVector = gameObject.GetComponent<Movement>().publicMoveVector;
            newTurret.GetComponent<Rigidbody>().velocity = moveVector/moveVector.magnitude * gameObject.GetComponent<Movement>().speed*2;
        }
        if ((gameObject.transform.position - newTurret.transform.position).magnitude <= 7f)
        {
            newTurret.GetComponent<Rigidbody>().velocity = .995f * newTurret.GetComponent<Rigidbody>().velocity;
        }
        
    }
    
}
