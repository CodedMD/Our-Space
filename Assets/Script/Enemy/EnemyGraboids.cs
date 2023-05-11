using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGraboids : MonoBehaviour
{
    [SerializeField] private float _speed = 2f;
    private Player _player;

 

    private Animator _deathAnim;
    [SerializeField] private AudioClip _enemyExplosionAudio;
    [SerializeField] private AudioSource _audioSource;


    [SerializeField] private Transform[] _wayPoint;
    [SerializeField] private int _wayPointIndex = 0;

    private float _canFire;
    [SerializeField] private float _fireRate = 0.5f;
    [SerializeField] private GameObject _enemyBeam;



    // Start is called before the first frame update
    void Start()
    {
        
        transform.position = _wayPoint[_wayPointIndex].transform.position;
        _speed = 2f;
        _audioSource = GetComponent<AudioSource>();
        // this creates a defualt access to the player script
        _player = GameObject.Find("Player").GetComponent<Player>();
        _deathAnim = gameObject.GetComponent<Animator>();
        //null check
        if (this.gameObject == null)
        {
            Debug.LogError("Whyyyy!!");
        }


        if (_player == null)
        {
            Debug.LogError("_player is null");
        }

    

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
    }

    // Update is called once per frame
    void Update()
    {
        if (_player != null)
        {
            GraboidMode();
            if (Time.time > _canFire)
            {
                _fireRate = Random.Range(1f, 3f);
                _canFire = Time.time + _fireRate;
                EnableLaser();

            }
        }


    }
    
    public void EnableLaser()
    {

         Instantiate(_enemyBeam, transform.position + new Vector3(0,-1.5f,0), Quaternion.identity);
      
    }



    public void GraboidMode()
    {

        
       if(_wayPointIndex <= _wayPoint.Length - 1)
        {
            transform.position = Vector3.MoveTowards(transform.position, _wayPoint[_wayPointIndex].transform.position, _speed * Time.deltaTime);
        }
       if(transform.position == _wayPoint[_wayPointIndex].transform.position)
        {
            
            _wayPointIndex += 1;
            if (_wayPointIndex == _wayPoint.Length)
            {
                _wayPointIndex = 0;
                
            }

        }
       
        if (transform.position.x >= 9.0f)
        {

            transform.position = new Vector3(Random.Range(-9.0f, -9.90f), 1, 0);
            Vector3.MoveTowards(transform.position, _wayPoint[_wayPointIndex].transform.position, _speed * Time.deltaTime);
        }
    }

    IEnumerator HoldFire()
    {
        _speed = 0;
        yield return new WaitForSeconds(3);
        _speed = 15f;
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
            Destroy(this.gameObject, 1f);
        }
        //if other is laser destroy laser and then us 
        if (other.tag == "Player_Laser")
        {
            Destroy(other.gameObject);
            _deathAnim.SetTrigger("OnEnemyDeath");
            Destroy(gameObject,1f);

            // add 10 to the score
            if (_player != null)
            {
                _player.AddScore(10);
            }
            //StartCoroutine(AnimatedEnemyDeath());

            _speed = 0;
            Destroy(GetComponent<LaserSeeker>());
            _audioSource.Play();
            Destroy(GetComponent<Collider2D>());


        }
        if (other.tag == "Enemy_Laser")
        {


        }
        if (other.tag == "Wipe_Out_Laser")
        {
            Destroy(this.gameObject, 1.8f);
            Destroy(GetComponent<LaserSeeker>());
            // add 10 to the score
            if (_player != null)
            {
                _player.AddScore(10);
            }
            //StartCoroutine(AnimatedEnemyDeath());
            _deathAnim.SetTrigger("OnEnemyDeath");
            _speed = 0;
            Destroy(GetComponent<LaserSeeker>());
            _audioSource.Play();
            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject);

        }

    }
}
