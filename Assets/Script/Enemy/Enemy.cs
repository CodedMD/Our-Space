using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4.0f;
    private Player _player;

    private Animator _deathAnim;
    [SerializeField] private AudioClip _enemyExplosionAudio;
    [SerializeField] private AudioSource _audioSource;
   
    private float _canFire;
    [SerializeField] private float _fireRate = 0.5f;
    [SerializeField] private GameObject _laserPrefab;

    
    // Start is called before the first frame update
    void Start()
    {


      
        _audioSource = GetComponent<AudioSource>();
        _player = GameObject.Find("Player").GetComponent<Player>();





        //null check
        if (_player == null)
        {
            Debug.LogError("_player is null");
        }
        _deathAnim = gameObject.GetComponent<Animator>();

        if (_deathAnim == null)
        {
            Debug.LogError("_deathAnim is Null");
        }
        if (_audioSource == null)
        {
            Debug.LogError("Audio Source on Enemy null");
        }
        else
        {
            _audioSource.clip = _enemyExplosionAudio;
        }
        
        //assign to animator
    }

    // Update is called once per frame
    void Update()
    {
        //move down 4 meters per seconds

        CalculateMovement();
        if(Time.time > _canFire)
        {
            _fireRate = Random.Range(3f, 6f);
            _canFire = Time.time + _fireRate;
            GameObject enemyLaser = Instantiate(_laserPrefab, transform.position, Quaternion.identity);
            Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();
           
            for (int i = 0; i < lasers.Length; i++)
            {
                lasers[i].AssignEnemyLaser();
            }
        }


    }
    void CalculateMovement()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        // if bottom screen 
        if (transform.position.y <= -6.0f)
        {
            //respawn at top with a new random x position
            //Instantiate(transform.position, new Vector3(0,7,0), Quaternion.identity);
            transform.position = new Vector3(Random.Range(-8.0f, 8.0f), 7, 0);
        }
    }






    public void DodgeLaser()
    {
        int _randomMove = Random.Range(2, 10);

        if (_randomMove == 0)
        {
            StartCoroutine(DodgeLeft());
        }
        else
        {
            StartCoroutine(DodgeRight());
        }
        
    }



    IEnumerator DodgeLeft()
    {
       
        Vector3 _currentPos = transform.position;
        Vector3 _destination = new Vector3(transform.position.x - 2, transform.position.y);
        float _dodge = 0f;
        
        while(_dodge < 1)
        {
            _dodge += Time.deltaTime * 3f;
            transform.position = Vector3.Lerp(_currentPos, _destination, _dodge);
            yield return null;
        }

    }

    IEnumerator DodgeRight()
    {
        Vector3 _currentPos = transform.position;
        Vector3 _destination = new Vector3(transform.position.x + 2, transform.position.y);
        float _dodge = 0f;

        while (_dodge < 1)
        {
            _dodge += Time.deltaTime * 3f;
            transform.position = Vector3.Lerp(_currentPos, _destination, _dodge);
            yield return null;
        }
    }



    void OnTriggerEnter2D(Collider2D other)
    {

        //if other is player damage player then destroy us 
        if (other.tag == "Player")
        {
            other.transform.GetComponent<Player>().Damage();

            _speed = 0;
            Destroy(gameObject, 1.1f);
            _deathAnim.SetTrigger("OnEnemyDeath");
            _audioSource.Play();
            Destroy(GetComponent<Collider2D>());
            
        }
        //if other is laser destroy laser and then us 
        if (other.tag == "Player_Laser")
        {
            Destroy(other.gameObject);
            // add 10 to the score
            if (_player != null)
            {
                _player.AddScore(10);
            }
            //StartCoroutine(AnimatedEnemyDeath());
            _speed = 0;
            Destroy(gameObject, 1f);
            _deathAnim.SetTrigger("OnEnemyDeath");
            Destroy(GetComponent<Laser>());
            Destroy(GetComponent<Collider2D>());
            _audioSource.Play();
 

        }
        if (other.tag == "Enemy_Laser")
        {
            

        }
        if (other.tag == "Wipe_Out_Laser")
        {
            
            // add 10 to the score
            if (_player != null)
            {
                _player.AddScore(10);
            }
            _speed = 0;
            Destroy(gameObject, 1.8f);
            _deathAnim.SetTrigger("OnEnemyDeath");
            Destroy(GetComponent<Laser>());
            _audioSource.Play();
            Destroy(GetComponent<Collider2D>());
         

        }

    }
    
  


}
