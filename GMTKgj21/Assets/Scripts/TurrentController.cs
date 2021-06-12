using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurrentController : MonoBehaviour
{

    /* component refs */
    public Animator Animator;
    public GameObject Model;
    public int Damage;

    public float attackRange = 4f;
    public Transform Laser;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("Aim", 1.0f, 0.5f);
        Laser.localScale = new Vector3(0, 0, attackRange);

    }


    public Enemy GetNearestEnemy()
    {
        Enemy nearestEnemy = null;
        float nearestDistance = attackRange;
        foreach (Enemy enemy in FindObjectsOfType<Enemy>())
        {           
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
    }



    private Vector3 _aimDirectionVector = new Vector3(0, 0, 1);
    public void Aim()
    {
        Enemy enemyToShoot = GetNearestEnemy();

        _aimDirectionVector = (enemyToShoot.transform.position + transform.position).normalized;

        float distanceToObstacle = attackRange;
        RaycastHit hit;
        Vector3 unitCenterPos = transform.position + new Vector3(0, 0.5f, 0);

        Ray ray = new Ray(unitCenterPos, _aimDirectionVector);
        if (Physics.Raycast(ray, out hit) && hit.transform.CompareTag("Wall"))
        {
            float distance = Vector3.Distance(hit.point, unitCenterPos);
            if (distance < distanceToObstacle )
                distanceToObstacle = distance;
        }

        transform.rotation = Quaternion.LookRotation(_aimDirectionVector);


        enemyToShoot.TakeDamage(Damage);
    }


    public void Shoot()
    {
        Shoot();

        //ShootBullet();
    }

    public void Attack()
    {


        //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //
        //if (Physics.Raycast(ray, out RaycastHit hitInfo))
        //{
        //    Vector3 aimPoint = GetGunHeightAimPoint(ray, hitInfo);
        //    gameObject.transform.LookAt(aimPoint);
        //}


        //ShootBullet();
    }
}
