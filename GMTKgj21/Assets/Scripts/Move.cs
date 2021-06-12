using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    private CharacterController controller;
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
    public GameObject laser;

    private void Start()
    {
        grenadeTriggered = false;
        reticle = (GameObject)Instantiate(grenadeReticle, new Vector3(Input.mousePosition.x, Input.mousePosition.y, .1f), Quaternion.identity);
        reticle.GetComponent<MeshRenderer>().enabled = false;
        controller = gameObject.AddComponent<CharacterController>();
        cooldown = .5f;
        timestamp = 0f;
        laser = (GameObject)Instantiate(laser, new Vector3(GetComponent<Transform>().position.x, GetComponent<Transform>().position.y, GetComponent<Transform>().position.z), Quaternion.identity);

    }

    public void ShootBullet(Vector3 playerPosition, Vector3 playerDirection, float angle, Vector3 playerVelocity)
    {
        if (angle > 180f) angle -= 360f;
        playerDirection.y = 0;
        Vector3 spawnPos = playerPosition;
        if (timestamp <= Time.time)
        {
            GameObject projectile = (GameObject)Instantiate(bullet, spawnPos, Quaternion.identity);
            projectile.GetComponent<Rigidbody>().velocity = playerDirection;
            if (projectile != null)
            {
                Destroy(projectile, 5f);
            }
            timestamp = Time.time + cooldown;
        }

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
    void Update()
    {
        
        Vector3 forward = transform.forward;
        float headingAngle = Quaternion.LookRotation(forward).eulerAngles.y;
        groundedPlayer = controller.isGrounded;
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        controller.Move(move * Time.deltaTime * playerSpeed);
        laser.GetComponent<Transform>().rotation = GetComponent<Transform>().rotation;
        laser.GetComponent<Transform>().position = GetComponent<Transform>().position;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        if (move != Vector3.zero)
        {
            gameObject.transform.forward = move;
        }

        // Changes the height position of the player..
        if (Input.GetKeyDown("space"))
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }
        if (Input.GetMouseButtonDown(0))
        {
            position = gameObject.transform.position;
            if (!grenadeTriggered)
            {
                ShootBullet(position, forward, headingAngle, playerVelocity);
            }
            if (grenadeTriggered && grenadeTimeStamp <= Time.time)
            {
                ThrowGrenade(position);
            }
        }
        if (Input.GetKeyDown("q"))
        {
            if (grenadeTriggered)
            {
                grenadeTriggered = false;
                reticle.GetComponent<MeshRenderer>().enabled = false;
            }
            else
            {
                if (grenadeTimeStamp <= Time.time)
                {
                    reticle.GetComponent<MeshRenderer>().enabled = true;
                    grenadeTriggered = true;
                }
            }

            //   AimGrenade(position, forward, headingAngle, playerVelocity);
        }
        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
        FaceMouse();
    }
}