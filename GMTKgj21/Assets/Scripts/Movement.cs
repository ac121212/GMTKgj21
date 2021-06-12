using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private PlayerController playerController;

    public float speed;
    public bool _isCollidingWithObstacle;
    private Vector3 _collisionVector;

    [HideInInspector]
    public Rigidbody rigidbody;

    /* private vars */
    public bool _isMoving;
    private Vector3 _TempMoveVector;
    public GameObject laser;
    public Vector3 publicMoveVector;

    public void Start()
    {
        if (GetComponent<PlayerController>() != null)
            playerController = GetComponent<PlayerController>();
        
        if (GetComponent<Rigidbody>() != null)
            rigidbody = GetComponent<Rigidbody>();

        laser = (GameObject)Instantiate(laser, new Vector3(GetComponent<Transform>().position.x, GetComponent<Transform>().position.y, GetComponent<Transform>().position.z), Quaternion.identity);

    }


    private void LookAt(Vector3 direction)
    {
        laser.transform.rotation = Quaternion.LookRotation(direction); 
        laser.transform.position = GetComponent<Transform>().position;
        transform.rotation = Quaternion.LookRotation(direction);
    }

    public void Move(Vector3 moveVector)
    {
        if (playerController.isKilled) return;
        playerController.Animator.SetFloat("Forward", 1);

        //this is for slip unit when it collide with obstacle
        if (this._isCollidingWithObstacle)
        {
            float angleBetweenCollisionAndMoveVector = Vector3.Angle(this._collisionVector, moveVector);
            if (angleBetweenCollisionAndMoveVector > 120)
            {
                if (this._collisionVector.x == 0)
                {
                    moveVector.z = 0;
                    moveVector.x = Mathf.Sign(moveVector.x) * 1f;
                }
                else
                {
                    moveVector.z = Mathf.Sign(moveVector.z) * 1f;
                    moveVector.x = 0;
                }
            }
        }

        rigidbody.velocity = new Vector3(0, 0, 0);
        transform.rotation = Quaternion.LookRotation(moveVector);
        this.transform.position = new Vector3(transform.position.x, 0.5f, transform.position.z);

        transform.Translate(moveVector/moveVector.magnitude * speed * Time.deltaTime, Space.World); //Direct
        publicMoveVector = moveVector;
        
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Floor")
            return;

        WalkAgainstWall = 0;
        this._isCollidingWithObstacle = false;
    }

    private int WalkAgainstWall;
    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Floor")
            return;

        this._isCollidingWithObstacle = true;
        this._collisionVector = collision.contacts[0].normal;

        if (WalkAgainstWall > 3)
        {
            Vector3 MoveTo = new Vector3(_collisionVector.x, _collisionVector.y, _collisionVector.z);

            if (Mathf.Abs(_collisionVector.x) > Mathf.Abs(_collisionVector.z))
                MoveTo = new Vector3(0, _collisionVector.y, _collisionVector.z);
            else
                MoveTo = new Vector3(_collisionVector.x, _collisionVector.y, 0);

            Move(MoveTo);
        }
        else
            WalkAgainstWall++;
    }

    void Update()
    {
        playerController.Animator.SetFloat("Forward", 0);

        _TempMoveVector = Vector3.zero;

        this._isMoving = false;

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) // left
        {
            _TempMoveVector = new Vector3(-1, 0, _TempMoveVector.z);
            playerController.Animator.SetFloat("LeftRight", -1);

            this._isMoving = true;

        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) //right
        {
            _TempMoveVector = new Vector3(1, 0, _TempMoveVector.z);
            playerController.Animator.SetFloat("LeftRight", 1);

            this._isMoving = true;

        }

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) //up
        {
            _TempMoveVector = new Vector3(_TempMoveVector.x, 0, 1);
            this._isMoving = true;

        }
        else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))// down
        {
            _TempMoveVector = new Vector3(_TempMoveVector.x, 0, -1);

            this._isMoving = true;

        }

        Move(_TempMoveVector);

    }
}