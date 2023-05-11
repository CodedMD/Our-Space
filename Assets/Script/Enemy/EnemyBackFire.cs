using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBackFire : MonoBehaviour
{
    [SerializeField]
    private float _speed;

    private Player _player;

     private Animator _deathAnim;



    [SerializeField] private GameObject _backFirePrefab;
    [SerializeField]private float minDistance;

    private bool _backFiredUp = true;

    // Start is called before the first frame update
    void Start()
    {
        
        _player = GameObject.Find("Player").GetComponent<Player>();
        GetComponent<BackFireLaser>();
       // _deathAnim = gameObject.GetComponent<Animator>();

        if (_player == null)
        {
            Debug.LogError("Player Null");
        }
        if (_speed == 0)
        {

        }


    }

    // Update is called once per frame
    void Update()
    {
       
       
       if (_player != null)
        {
            FireUpperCut();
        }
       
    }
    


public void FireUpperCut()
    {
        float distanceX = Mathf.Abs(_player.transform.position.x - transform.position.x);
        if (_player.transform.position.y > transform.position.y && distanceX < minDistance && _backFiredUp)
        {
            GameObject enemyLaser = Instantiate(_backFirePrefab, transform.position + new Vector3(0, 2.2f, 0), Quaternion.identity);
            BackFireLaser laser = enemyLaser.GetComponent<BackFireLaser>();

            _backFiredUp = false;
      
        }
    }


 







    void OnTriggerEnter2D(Collider2D other)
    {

        //if other is player damage player then destroy us 
        if (other.tag == "Player")
        {
            other.transform.GetComponent<Player>().Damage();

          
           _deathAnim.SetTrigger("OnTankerDeath");
            _speed = 0;

      
            Destroy(GetComponent<Laser>());
            Destroy(GetComponent<Collider2D>());
            Destroy(gameObject, 1f);
        }
        //if other is laser destroy laser and then us 
        if (other.tag == "Player_Laser")
        {
          //  _deathAnim.SetTrigger("OnTankerDeath");
            Destroy(other.gameObject);
            Destroy(gameObject, 1);
            // add 10 to the score
            if (_player != null)
            {
                _player.AddScore(10);
            }
        
            
            _speed = 0;
            Destroy(GetComponent<Laser>());
        
            Destroy(GetComponent<Collider2D>());
            

        }
        if (other.tag == "Enemy_Laser")
        {


        }
        if (other.tag == "Wipe_Out_Laser")
        {

            _deathAnim.SetTrigger("OnTankerDeath");
            Destroy(gameObject, 1f);
            // add 10 to the score
            if (_player != null)
            {
                _player.AddScore(10);
            }
        
            
            _speed = 0;
            Destroy(GetComponent<Laser>());
        
            Destroy(GetComponent<Collider2D>());
           

        }

    }


}
