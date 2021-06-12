using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretController : MonoBehaviour
{
    Animator laserTurretAnimator;
    List<Collider> enemies;
    // Start is called before the first frame update
    void Start()
    {
        laserTurretAnimator = gameObject.GetComponentInChildren<Animator>();
        enemies = new List<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnTriggerEnter(Collider other)
    {
        if (!enemies.Contains(other))
        {
            //add the object to the list
            enemies.Add(other);
        }
    }
    public void OnTriggerStay (Collider other)
    {
        if(other.gameObject.tag == "Enemy")
        {
            laserTurretAnimator.SetTrigger("Fire");
            GameObject closest = ClosestEnemy(enemies);
            gameObject.transform.LookAt(closest.transform.position);

        }

    }
    public GameObject ClosestEnemy(List<Collider> colliders)
    {
        float bestDist = 100000f;
        Collider closest = new Collider();
        foreach(Collider enemy in colliders)
        {
            float dist = (gameObject.transform.position - enemy.gameObject.transform.position).magnitude;
            if (dist < bestDist)
            {
                bestDist = dist;
                closest = enemy;
            }
        }
        return closest.gameObject;
    }
    public void OnTriggerExit(Collider other)
    {
        //if the object is in the list
        if (enemies.Contains(other))
        {
            //remove it from the list
            enemies.Remove(other);
        }
    }
    public void Shoot()
    {

    }
}
