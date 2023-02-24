using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        // this creates a defualt access to the player script
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

    void OnTriggerEnter2D(Collider2D other)
    {

        //if other is player damage player then destroy us 
        if (other.tag == "Player")
        {
            other.transform.GetComponent<Player>().Damage();
            // StartCoroutine(AnimatedEnemyDeath());
            _deathAnim.SetTrigger("OnEnemyDeath");
            _speed = 0;
            _audioSource.Play();
            Destroy(GetComponent<Collider2D>());
            Destroy(gameObject, 2.5f);
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
            _deathAnim.SetTrigger("OnEnemyDeath");
            _speed = 0;
            _audioSource.Play();
            Destroy(GetComponent<Collider2D>());
            Destroy(gameObject, 2.5f);

        }
        if (other.tag == "Enemy_Laser")
        {
            

        }

    }
    //IEnumerator AnimatedEnemyDeath()
    //{
        
       
      //  yield return new WaitForSeconds(2.50f);
        
      //  Destroy(gameObject);
   // }
  


}
