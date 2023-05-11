using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingFireLaser : MonoBehaviour
{
    //Speed
    [SerializeField] private float _speed = 8.0f;
    private float _rotationSpeed = 350;
    private GameObject _closeEnemy;
    private Rigidbody2D _rb;









    // Start is called before the first frame update
    void Start()
    {
       
        _rb = GetComponent<Rigidbody2D>();
 
        if (_rb == null)
        {
            Debug.LogError("_rb is null");
        }
       
    }




    // Update is called once per frame
    void Update()
    {
        if (_closeEnemy == null)
        {
           _closeEnemy = WheresTheEnemy();
        }
        if (_closeEnemy != null)
        {
            MoveTowardsEnemy();
        }
        else
        {
            transform.Translate(Vector3.up * (_speed/3) * Time.deltaTime);
        }

        //if laser position is greater than 8 on the y distroy the object
        if (transform.position.y > 5.0f || transform.position.y < -5.0f || transform.position.x < -9.0f || transform.position.x > 9.0f)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            Destroy(this.gameObject);
        }




    }


    private GameObject WheresTheEnemy()
    {
        try
        {
            GameObject[] enemies;
            enemies = GameObject.FindGameObjectsWithTag("Enemy");
            GameObject close = null;
            float distance = Mathf.Infinity;
            Vector3 position = transform.position;

            foreach(GameObject enemy in enemies)
            {
                Vector3 other = enemy.transform.position - position;
                float currentDistance = other.sqrMagnitude;
                if(currentDistance < distance)
                {
                    close = enemy;
                    distance = currentDistance;
                }
            }
            return close;
        }
        catch
        {
            return null;
        }
    }

    private void MoveTowardsEnemy()
    {
        Vector3 direction = _rb.position - (Vector2)_closeEnemy.transform.position ;
        direction.Normalize();
        float rototionAmount = Vector3.Cross(direction, transform.up).z;
        _rb.angularVelocity = rototionAmount * _rotationSpeed;
        _rb.velocity = transform.up * _speed;
    }



    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            
            Destroy(other.gameObject);
            EnemyTanker enemyTanker = other.GetComponent<EnemyTanker>();
           
            if (enemyTanker != null)
            {
                enemyTanker.Damage();
            }
        }
        
    }


}
