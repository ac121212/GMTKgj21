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
    public GameObject muzzlePrefab;
    public GameObject hitPrefab;
    public AudioClip shotSFX;
    public AudioClip hitSFX;
    public bool NoMove;
    public bool DontDestroyOnWalls;
    public bool ThrowAttack;
    public float LifeTime = 0.3f;
    private bool _isStarted;
    public float bulletSpeed = 10f;
    public float velocity;

    public void Start()
    {
        

    }

    /// <summary>
    /// Called via an animator event when the charge starts
    /// </summary>
    public void PlayDestroyFeedBack()
    {
    }
    
    public void SetData(float maxDistance, int hitPoints)
    {
        this._moveDirection = Vector3.forward;
        this._maxDistance = maxDistance;
        this._hitPoints = hitPoints;
        this._isStarted = true;
        
        if (NoMove || DontDestroyOnWalls)
            StartCoroutine(DestroyAfterTime(LifeTime));

        if (ThrowAttack)
            StartCoroutine(ActivateColliderAfterTime(0.5f));

    }

    public void LookAt(Vector3 direction)
    {
        transform.rotation = Quaternion.LookRotation(direction);
    }

    private float _BulletTraveledDistance = 0;
    private void Update()
    {
        if (!this._isStarted)
            return;
        if (NoMove)
            return;

        if (this._maxDistance > this._velocity * this._BulletTraveledDistance)
        {
            _BulletTraveledDistance = _BulletTraveledDistance + Time.deltaTime;
        }

        Vector3 newDirection = new Vector3(LeftRightDirection, 0, 1);
        //this.transform.Translate(this._moveDirection * this._velocity * Time.deltaTime);
        this.transform.Translate(newDirection * this._velocity * Time.deltaTime);

       
    }

    private void OnTriggerEnter(Collider other)
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
            PlayDestroyFeedBack();

            if (!DontDestroyOnWalls)
                Destroy(this.gameObject);
        }
    }

    

    public IEnumerator DestroyAfterTime(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        PlayDestroyFeedBack();
        Destroy(this.gameObject);
    }

    public IEnumerator ActivateColliderAfterTime(float waitTime)
    {
        GetComponent<Collider>().enabled = false;
        yield return new WaitForSeconds(waitTime);
        GetComponent<Collider>().enabled = true;
    }

   
}

