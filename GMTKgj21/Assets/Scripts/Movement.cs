using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private PlayerController playerController;

    public float speed;
    private float turnScale;
    private float forwardScale;
    public bool _isCollidingWithObstacle;
    private Vector3 _collisionVector;
    private bool up;
    private bool down;
    private bool left;
    private bool right;
    private Vector3 lookingDirection = new Vector3 (0f,0f,0f);

    [HideInInspector]
    public Rigidbody rigidbody;

    /* private vars */
    public bool _isMoving;
    private Vector3 _TempMoveVector;
    public Vector3 publicMoveVector;

    public void Start()
    {
        if (GetComponent<PlayerController>() != null)
            playerController = GetComponent<PlayerController>();
        
        if (GetComponent<Rigidbody>() != null)
            rigidbody = GetComponent<Rigidbody>();

        turnScale = 0f;
        forwardScale = 0f;
        up = false;
        left = false;
        right = false;
        down = false;

    }


    private void LookAt(Vector3 direction)
    {
        transform.rotation = Quaternion.LookRotation(direction);
    }

    public void Move(Vector3 moveVector)
    {
        if (playerController.isKilled) return;
        //playerController.Animator.SetFloat("Forward", 1);

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
        
        float angleDif = (transform.rotation.eulerAngles.y - Quaternion.LookRotation(lookingDirection).eulerAngles.y);
        if (angleDif < 0)
        {
            angleDif += 360f;
        }
        if (angleDif >0f && angleDif<180f)
        {
            if (turnScale > -1f)
            {
                turnScale -= .1f;
            }
            
            playerController.Animator.SetFloat("LeftRight", turnScale);
        } else if (angleDif >=180f && angleDif <= 360)
        {
            if (turnScale < 1)
            {
                turnScale += .1f;
            }
            playerController.Animator.SetFloat("LeftRight", turnScale);
        }
        if (angleDif == 0)
        {
            if (forwardScale <1f)
            {
                forwardScale += .1f;
            }

            turnScale *= .8f;
            if (turnScale >=-.02f && turnScale <= .02f) 
            {
                turnScale = 0f;
            }

            playerController.Animator.SetFloat("Forward", forwardScale);
            playerController.Animator.SetFloat("LeftRight", turnScale);
        }
        
        
        //  transform.rotation = Quaternion.LookRotation(moveVector);
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

        if (collision.gameObject.tag == "Wall")
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
     //   print(WalkAgainstWall);
    }

    void Update()
    {
        playerController.Animator.SetFloat("Forward", 0);
        if (!((Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))||(Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))))
        {
            _TempMoveVector = new Vector3(0f, 0f, _TempMoveVector.z);
        }
        if (!((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) || (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))))
        {
            _TempMoveVector = new Vector3(_TempMoveVector.x, 0f, 0f);
        }
        if (_TempMoveVector != Vector3.zero)
        {
            lookingDirection = _TempMoveVector;
        } else
        {
            lookingDirection = transform.forward;
        }


        this._isMoving = false;
        up = false;
        down = false;
        right = false;
        left = false;
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) // left
        {
            left = true;
        
            this._isMoving = true;
        
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) //right
        {
            right = true;
            this._isMoving = true;
        
        }
        
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) //up
        {
            up = true;
            this._isMoving = true;
        
        }
        else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))// down
        {
            down = true;
            this._isMoving = true;
        
        } 
        if ((up || down) && !(left || right))
        {
            if (up)
            {
                _TempMoveVector = new Vector3(0f, 0f, 1f);
            }
            if (down)
            {
                _TempMoveVector = new Vector3(0f, 0f, -1f);
            }  
        }
        if (!(up || down) && (left || right))
        {
            if (left)
            {
                _TempMoveVector = new Vector3(-1f, 0f, 0f);
            }
            if (right)
            {
                _TempMoveVector = new Vector3(1f, 0f, 0f);
            }
        }
        if ((up || down) && (left || right))
        {
            if (left && up)
            {
                _TempMoveVector = new Vector3(-1f, 0f, 1f);
            }
            if (left && down)
            {
                _TempMoveVector = new Vector3(-1f, 0f, -1f);
            }
            if (right && up)
            {
                _TempMoveVector = new Vector3(1f, 0f, 1f);
            }
            if (right && down)
            {
                _TempMoveVector = new Vector3(1f, 0f, -1f);
            }
        }
        if (this._isMoving == false && forwardScale > 0)
        {
            forwardScale -= .1f;
        }
        
        Move(_TempMoveVector);
        gameObject.transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(lookingDirection), Time.deltaTime * 100f);
    }
}