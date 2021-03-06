using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public bool isKilled;

    /* prefabs */
    public GameObject ExplodeParticle;

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
    public Animator enemyAnimator;
    public GameObject scrap1;
    public GameObject scrap2;
    public GameObject scrap3;
  
    void Start()
    {
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
        if (currentHealth <=0)
        {
            Die();
        }
    }

    public void TakeDamage(float Damage)
    {
        currentHealth -= Damage;

        if (currentHealth <= 0)
            Die();
    }

    private void Die()
    {
        if (Random.Range(0, 10) == 1)
            Instantiate(scrap1, transform.position, Quaternion.identity);
        Instantiate(ExplodeParticle, transform.position, Quaternion.identity);

        gameController.Enemys.Remove(this.gameObject);
        Destroy(this.gameObject);
        print("DIE");
    }
    
    private void Attack()
    {
        _targetUnit.TakeDamage(hitPoints);
        Die();

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

        _isMoving = true;

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
