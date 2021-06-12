using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public bool isKilled;

    private float IntervalLoadAttack = 0.1f;
    private float StartIntervalLoadAttack = 0.1f;
    private Common.State _currentState;

    public Movement movement;
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
    private Rigidbody rigidbody;
    public bool _isMoving;


    // Start is called before the first frame update
    void Start()
    {
        movement = GetComponent<Movement>();
        gameController = FindObjectOfType<GameController>();
        rigidbody = GetComponent<Rigidbody>();
        StartAI();
    }

    public void Init()
    {
        currentHealth = healthPoints;
    }

    // Update is called once per frame
    void Update()
    {
        AIUpdate();
        //Attack();
        //groundedPlayer = controller.isGrounded;
        //if (groundedPlayer && playerVelocity.y < 0)
        //{
        //    playerVelocity.y = 0f;
        //}
        //
        //if (!groundedPlayer)
        //{
        //    playerVelocity.y -= Mathf.Sqrt(-3.0f * gravityValue);
        //}



        //controller.Move(playerVelocity * Time.deltaTime * playerVelocity.magnitude);
        //playerVelocity = .995f * playerVelocity;

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


    public void TakeDamage(int Damage)
    {
        currentHealth -= Damage;

        if (currentHealth <= 0)
            Die();
    }

    private void Die()
    {
        gameController.Enemys.Remove(this.gameObject);
        Destroy(this.gameObject);
    }


    private void Attack()
    {
        //Vector3 mag = player.transform.position - gameObject.transform.position;
        //float dist = mag.magnitude;
        //float scale = .001f;
        //float range = player.gameObject.GetComponent<BoxCollider>().size.x + GetComponent<CapsuleCollider>().radius;
        //
        //playerVelocity += (mag / dist) * scale;

        // gameObject.GetComponent<Rigidbody>().velocity +=.2f*(player.transform.position - gameObject.transform.position);
    }

    
    #region AI
    /* private vars */

    public PlayerController _targetUnit;

    private Vector3 _targetPosition;
    private bool _targetFound;
    private int AttackCounter;

    public AIGoal myCurrentGoal;
    public AIType myAIType;
    public NavMeshAgent navMeshAgent;
    public int minResetAIGoalTime;
    public int maxResetAIGoalTime;

    public enum AIType
    {
       Beginner, Normal, Strong
    }

    public enum AIGoal
    {
        GoForTarget
    }

    void StartAI()
    {
        this._targetFound = false;
        gameController = FindObjectOfType<GameController>();
        navMeshAgent = GetComponent<NavMeshAgent>();


        if(GetComponent<Movement>() != null)
            navMeshAgent.speed = this.GetComponent<Movement>().speed;

        AITypeSetup();

    }

    public void AITypeSetup()
    {

       
    }

    void AIUpdate()
    {
        if (!_targetFound || this._targetUnit == null)
        {
            this.FindTarget();
        }
        else
        {
            if (Vector3.Distance(this._targetPosition, this.transform.position) < this.attackRange)
            {
                Attack();
                AttackCounter++;
                if (myAIType.Equals(AIType.Beginner) && AttackCounter == 2)
                    this._targetUnit = null;
                else if (myAIType.Equals(AIType.Normal) && AttackCounter == 3)
                    this._targetUnit = null;
                else if (myAIType.Equals(AIType.Strong) && AttackCounter == 4)
                    this._targetUnit = null;
            }
            else
            {
                AttackCounter = 0;

                if (this._targetUnit != null)
                    this._targetPosition = this._targetUnit.transform.position;

                MoveWithNavMesh();

            }
        }
    }


    public void MoveWithNavMesh()
    {
        navMeshAgent.SetDestination(this._targetPosition);
        rigidbody.velocity = new Vector3(0, 0, 0);

        SetState(Common.State.RUN);
        _isMoving = true;

    }

    public void SetAIGoal()
    {
        switch (myAIType)
        {
            case AIType.Beginner:
                {
                   

                }
                break;

            case AIType.Normal:
                {


                }
                break;

            case AIType.Strong:
                {


                }
                break;
        }

    }

  



    public void FindTarget()
    {

        this._targetUnit = FindObjectOfType<PlayerController>();
        this._targetPosition = this._targetUnit.transform.position;

        this._targetFound = true;

        this.StartCoroutine(_RepeatFindTargetAfterAWhile());
    }

    
    private IEnumerator _RepeatFindTargetAfterAWhile()
    {
        yield return new WaitForSeconds(Random.Range(minResetAIGoalTime, maxResetAIGoalTime));
        this._targetFound = false;
        FindTarget();
    }



    #endregion AI

}
