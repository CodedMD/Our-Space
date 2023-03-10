using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class Laser : MonoBehaviour
{
    //Speed
    [SerializeField] private float _speed = 8.0f;
    private bool _isEnemyLaser = false;
    private bool _isWipeOutLaser = false;
    




    // Start is called before the first frame update
    void Start()
    {
     
    }




    // Update is called once per frame
    void Update()
    {
        if (_isEnemyLaser == false)
        {
            MoveUp();
        }
        else if (_isWipeOutLaser == true)
        {
            
            MoveUp();
        }
        else
        {
            MoveDown();
        }

    }



    void MoveUp()
    {
        transform.Translate(Vector3.up * _speed * Time.deltaTime);

        //if laser position is greater than 8 on the y distroy the object
        if (transform.position.y > 8.0f)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            Destroy(this.gameObject);
        }

    }




    void MoveDown()
    {
        transform.Translate(Vector3.down*_speed * Time.deltaTime);

        //if laser position is greater than 8 on the y distroy the object
        if (transform.position.y < -5.0f)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            Destroy(this.gameObject);
        }

    }




    public void AssignEnemyLaser()
    {
        _isEnemyLaser = true;
    }
    public void WipeOutTime()
    {
        _isWipeOutLaser = true;
    }




    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && _isEnemyLaser == true)
        {
            //_impulseSource = GetComponent<CinemachineImpulseSource>();
            Player player = other.GetComponent<Player>();

            if (player != null)
            {
                
                player.Damage();
            }
        }


    }



}
