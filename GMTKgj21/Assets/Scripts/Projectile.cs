using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float LeftRightDirection;
    public int _hitPoints;
    private float _velocity;
    private Vector3 _moveDirection;
    private float _maxDistance;
    public GameObject hitPrefab;
    public AudioClip shotSFX;
    public AudioClip hitSFX;
    public bool NoMove;
    public bool DontDestroyOnWalls;
    public bool ThrowAttack;
    public float LifeTime = 0.3f;
    public float bulletSpeed = 10f;
    public float velocity;

   
    public void SetData(float maxDistance, int hitPoints)
    {
        this._moveDirection = Vector3.forward;
        this._maxDistance = maxDistance;
        this._hitPoints = hitPoints;
        
        if (NoMove || DontDestroyOnWalls)
            StartCoroutine(DestroyAfterTime(LifeTime));

        if (ThrowAttack)
            StartCoroutine(ActivateColliderAfterTime(0.5f));

        GetComponent<Rigidbody>().velocity = transform.forward * bulletSpeed;
    }

    public void LookAt(Vector3 direction)
    {
        transform.rotation = Quaternion.LookRotation(direction);
    }

   

    private void OnTriggerEnter(Collider other)
    {
        //if(other.GetComponent<PlayerController>() != null)
        //{
        //}
        //else
        {
            Enemy unit = other.GetComponent<Enemy>();

            //Projectile hit Enemy
            if (unit != null && !unit.isKilled)
            {
                unit.TakeDamage(this._hitPoints);
                Destroy(this.gameObject);
            }
            //Projectile hit Wall or other
            else if (other != null && unit == null && other.GetComponentInParent<Enemy>() == null)
            {

                if (!DontDestroyOnWalls)
                    Destroy(this.gameObject);
            }
        }

        
    }

    

    public IEnumerator DestroyAfterTime(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        Destroy(this.gameObject);
    }

    public IEnumerator ActivateColliderAfterTime(float waitTime)
    {
        GetComponent<Collider>().enabled = false;
        yield return new WaitForSeconds(waitTime);
        GetComponent<Collider>().enabled = true;
    }

   
}




