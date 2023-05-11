using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackFireLaser : MonoBehaviour
{
    
    //Speed
    [SerializeField] private float _speed = 8.0f;
    //private bool _isEnemyLaser = false;
    //private bool _isWipeOutLaser = false;

    private Player _player;
    private float _distance;
    [SerializeField] private float _distanceBetween;



    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        if (_player == null)
        {

            Debug.LogError("_player is null");
        }
    }




    // Update is called once per frame
    void Update()
    {
        if (_player != null)
        {
            MoveUp();
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

        _distance = Vector3.Distance(transform.position, _player.transform.position);

        Vector3 direction = _player.transform.position - transform.position;
        // transform.Translate(Vector3.down * _speed * Time.deltaTime);
        if (_distance >= _distanceBetween)
        {
            transform.Translate(Vector3.down * _speed * Time.deltaTime);
        }
        else if (_distance < _distanceBetween)
        {

            _speed = 15f;
            transform.position = Vector3.MoveTowards(this.transform.position, _player.transform.position, _speed * Time.deltaTime);


        }

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




    //public void AssignEnemyLaser()
    // {
    //   _isEnemyLaser = true;
    //}

    //public void WipeOutTime()
    // {
    //    _isWipeOutLaser = true;
    //}




    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {

            Player player = other.GetComponent<Player>();

            if (player != null)
            {

                player.Damage();
                Destroy(gameObject);
            }
        }




    }
}
