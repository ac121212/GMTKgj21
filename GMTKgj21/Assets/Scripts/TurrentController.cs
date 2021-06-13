using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurrentController : MonoBehaviour
{
    public float attackRange = 4f;
    public Transform Laser;
    private Vector3 _aimDirectionVector = new Vector3(0, 0, 1);
    public Enemy CurrentTarget;
    public int Damage;
    public Animator turretAnimation;
    public bool CanShoot;
    [Header("The lower the faster")]
    public float ShootingInterval = 0.3f;
    public float ShootingTime = 0.3f;
    private GameObject closest;
    List<Collider> enemies = new List<Collider>(); 
    // Start is called before the first frame update
    void Start()
    {
        Laser.localScale = new Vector3(0, 0, attackRange);
    }

   /* public Enemy GetNearestEnemy()
    {
        Enemy nearestEnemy = null;
        float nearestDistance = attackRange;
        foreach (Enemy enemy in FindObjectsOfType<Enemy>())
        {
            print("here");
            RaycastHit hit;
            Vector3 unitCenterPos = transform.position + new Vector3(0, 0.5f, 0);
            Ray ray = new Ray(unitCenterPos, _aimDirectionVector);
            {
                if (Physics.Raycast(ray, out hit))
                    if (hit.transform.CompareTag("Wall"))
                    {
                        continue;
                    }
            }
            float dist = Vector3.Distance(enemy.transform.position, this.transform.position);
            if (dist < nearestDistance)
                nearestEnemy = enemy;
        }

        return nearestEnemy;
    }*/

    public void Aim()
    {
        //CurrentTarget. = closest;
        //print("here");
        _aimDirectionVector = (closest.transform.position + transform.position).normalized;
        float distanceToObstacle = attackRange;
        RaycastHit hit;
        Vector3 unitCenterPos = transform.position + new Vector3(0, 0.5f, 0);
        Ray ray = new Ray(unitCenterPos, _aimDirectionVector);
        if (Physics.Raycast(ray, out hit) && hit.transform.CompareTag("Wall"))
        {
            float distance = Vector3.Distance(hit.point, unitCenterPos);
            if (distance < distanceToObstacle)
                distanceToObstacle = distance;

        }
      //   transform.rotation = Quaternion.LookRotation(_aimDirectionVector);
        

         transform.LookAt(closest.transform.position);
      
        turretAnimation.SetBool("Firing", true);
        CanShoot = true;
    }
    public void OnTriggerEnter(Collider other)
    {
        if (!enemies.Contains(other) && other.gameObject.tag == "Enemy")
        {
            //add the object to the list
            enemies.Add(other);
        }
    }
    public void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            //laserTurretAnimator.SetTrigger("Fire");
            closest = ClosestEnemy(enemies);
            //gameObject.transform.LookAt(closest.transform.position);
           // print(closest.transform.position);

        }

    }
    public GameObject ClosestEnemy(List<Collider> colliders)
    {
        float bestDist = 100000f;
        Collider closest = new Collider();
        foreach (Collider enemy in colliders)
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
        
        float dist = Vector3.Distance(closest.transform.position, this.transform.position);
        if (dist < GetComponent<SphereCollider>().radius)
        {
            
         //   CurrentTarget.TakeDamage(Damage);
        }
            
        else
        {
            turretAnimation.SetBool("Firing", false);
            CanShoot = false;
        }
            
    }

    private void Update()
    {
       // if (ShootingTime > 0)
     //   {
            ShootingTime -= Time.deltaTime;
            Aim();
      //  }
        if (CanShoot && ShootingTime <= 0)
        {
            ShootingTime = ShootingInterval;
            Shoot();
        }
    }
}