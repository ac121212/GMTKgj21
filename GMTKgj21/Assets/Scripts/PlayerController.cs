using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public bool isKilled;
    public GameObject wire;
    public GameObject turret;
    private float IntervalLoadAttack = 0.1f;
    private float StartIntervalLoadAttack = 0.1f;
    private Common.State _currentState;

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
    public AudioListener audioListener;

    public float attackRange = 4f;

    public float currentHealth;
    public float healthPoints = 10;
    public float healthPointsIncreaser;
    public int hitPoints = 2;
    public float hitPointsIncreaser;
    public float reloadTime = 1;
    public float attackTime = 0.5f;
    public float recoverTime = 0.5f;
    private bool slowed;
   
    private Vector3 finalPoint;
    private GameObject newWire;
    private GameObject newTurret;
    private float slowTimeStamp;
    // Start is called before the first frame update
    void Start()
    {
        gameController = FindObjectOfType<GameController>();
        currentHealth = healthPoints;

        
        ConnectWire();
        newTurret = Instantiate(turret,new Vector3(2, 1, 2), Quaternion.identity);
        slowed = false;
    }

    public void SetState(Common.State state)
    {
        if (this.Animator == null)
            return;

        if (state == _currentState)
            return;

        if (state == Common.State.IDLE)
            this.Animator.SetTrigger("idle");
        else if (state == Common.State.RUN)
            this.Animator.SetTrigger("run");
        else if (state == Common.State.ATTACK)
            this.Animator.SetTrigger("attack");
        else if (state == Common.State.TAKEDAMAGE)
            this.Animator.SetTrigger("takedamage");
      
        this._currentState = state;
    }

    

    // Update is called once per frame
    void Update()
    {
        
        KeepWireConnected();
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
            gameObject.GetComponent<Movement>().speed = 2f;
            Vector3 moveVector = gameObject.GetComponent<Movement>().publicMoveVector;
            newTurret.GetComponent<Rigidbody>().velocity = moveVector/moveVector.magnitude * gameObject.GetComponent<Movement>().speed*2;
            slowed = true;
            slowTimeStamp = Time.time+2f;
        }
        if ((gameObject.transform.position - newTurret.transform.position).magnitude <= 7f)
        {
            if (slowed && Time.time >= slowTimeStamp)
            {
                gameObject.GetComponent<Movement>().speed = 5;
            }
            
            newTurret.GetComponent<Rigidbody>().velocity = .995f * newTurret.GetComponent<Rigidbody>().velocity;
        }
        
    }
    
}
