using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public bool isKilled;

    private float IntervalLoadAttack = 0.1f;
    private float StartIntervalLoadAttack = 0.1f;
    private Common.State _currentState;

    /* prefabs */
    public GameObject Bullet;
    public GameObject BombPrefab;

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

    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private float playerSpeed = 2.0f;
    private float jumpHeight = 1.0f;
    private float gravityValue = -9.81f;
    public GameObject grenade;
    private GameObject reticle;
    public GameObject bullet;
    public GameObject grenadeReticle;
    private Vector3 position;
    private Vector3 playerRotation;
    private float cooldown;
    private float timestamp;
    private float grenadeTimeStamp = 0f;
    private float grenadeCooldown = 1f;
    private bool grenadeTriggered;
    private Vector3 finalPoint;

    // Start is called before the first frame update
    void Start()
    {
        gameController = FindObjectOfType<GameController>();
        currentHealth = healthPoints;


        grenadeTriggered = false;
        reticle = (GameObject)Instantiate(grenadeReticle, new Vector3(Input.mousePosition.x, Input.mousePosition.y, .1f), Quaternion.identity);
        reticle.GetComponent<MeshRenderer>().enabled = false;
        cooldown = .5f;
        timestamp = 0f;

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

    public void Attack()
    {
        ShootBullet();


    }

    // Update is called once per frame
    void Update()
    {
        Vector3 forward = transform.forward;
        float headingAngle = Quaternion.LookRotation(forward).eulerAngles.y;
        //
        
        //
        //if (groundedPlayer && playerVelocity.y < 0)
        //{
        //    playerVelocity.y = 0f;
        //}
        //
        //
        //// Changes the height position of the player..
        //if (Input.GetKeyDown("space"))
        //{
        //    playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        //}
        //
        //
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Attack();
        }
        //
        //if (Input.GetKeyDown("q"))
        //{
        //    if (grenadeTriggered)
        //    {
        //        grenadeTriggered = false;
        //        reticle.GetComponent<MeshRenderer>().enabled = false;
        //    }
        //    else
        //    {
        //        if (grenadeTimeStamp <= Time.time)
        //        {
        //            reticle.GetComponent<MeshRenderer>().enabled = true;
        //            grenadeTriggered = true;
        //        }
        //    }
        //
        //    //   AimGrenade(position, forward, headingAngle, playerVelocity);
        //}
        //
        //FaceMouse();

    }

    public void ShootBullet()
    {
        GameObject currentBullet = Instantiate(Bullet);
        currentBullet.GetComponent<Projectile>().SetData( attackRange, hitPoints);
        //bullet.transform.rotation = Quaternion.LookRotation(/*_aimDirectionVector*//*+ (new Vector3(0, Angle,0)).normalized*/);

    }

    public void ThrowGrenade(Vector3 playerPosition)
    {
        Vector3 spawnPos = playerPosition;
        GameObject projectile = (GameObject)Instantiate(grenade, spawnPos, Quaternion.identity);
        float time = 2f;
        float yVelocity = (-playerPosition.y + .5f * 9.81f * time * time) / time;
        projectile.GetComponent<Rigidbody>().velocity = new Vector3((finalPoint.x - playerPosition.x) / time, yVelocity, (finalPoint.z - playerPosition.z) / time);
        reticle.GetComponent<MeshRenderer>().enabled = false;
        grenadeTriggered = false;
        grenadeTimeStamp = Time.time + grenadeCooldown;
    }

    public void FaceMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hitInfo))
        {
            Vector3 aimPoint = GetGunHeightAimPoint(ray, hitInfo);
            gameObject.transform.LookAt(aimPoint);
            reticle.GetComponent<Transform>().position = aimPoint;
            finalPoint = aimPoint;
        }


    }
    private Vector3 GetGunHeightAimPoint(Ray mouseAim, RaycastHit hitInfo)
    {
        // if the raycast hit something and the Y is above the gun height
        if (hitInfo.collider != null && hitInfo.point.y > .51)
        {
            Vector3 heightAdjusted = hitInfo.point;
            heightAdjusted.y = .5f;
            return heightAdjusted;
        }

        Plane aimPlane = new Plane(Vector3.up, Vector3.up * .5f);
        if (aimPlane.Raycast(mouseAim, out float distance))
        {
            return mouseAim.GetPoint(distance);
        }
        else
        {
            return Vector3.zero; // you missed the ground
        }
    }
}
