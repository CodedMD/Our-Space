using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyTanker : MonoBehaviour
{
    
    [SerializeField] private float _speed = 2.5f;
    [SerializeField] private int _lives;
    [SerializeField] private GameObject _tankerShield;
    [SerializeField] private GameObject _tankerHurtShield;
    [SerializeField] private Transform[] _wayPoint;
    [SerializeField] private int _wayPointIndex = 0;

    private Player _player;
    private float _canFire;
    [SerializeField] private float _fireRate = 0.5f;
   // private bool _isShieldPowerupActive = true;
    

    //private bool _isHurtShieldActive = false;



    private Animator _deathAnim;
    [SerializeField] private AudioClip _enemyExplosionAudio;
    [SerializeField] private AudioClip _enemyLaserbeamAudio;
    [SerializeField] private AudioSource _audioSourceExplosion;
    [SerializeField] private AudioSource _audioSourceLaser;




    //private LineRenderer _lineRenderer;


    // Start is called before the first frame update
    void Start()
    {
        _lives = 2;
        _tankerShield.gameObject.SetActive(true);
        _tankerHurtShield.gameObject.SetActive(false);
        transform.position = _wayPoint[_wayPointIndex].transform.position;
        _player = GameObject.Find("Player").GetComponent<Player>();

        _audioSourceExplosion = GetComponent<AudioSource>();
        _audioSourceLaser = GetComponent<AudioSource>();
        _deathAnim = gameObject.GetComponent<Animator>();

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
        if (_audioSourceExplosion == null)
        {
            Debug.LogError("Audio Source on Enemy null");
        }
        else
        {
            _audioSourceExplosion.clip = _enemyExplosionAudio;
            _audioSourceLaser.clip = _enemyLaserbeamAudio;
        }
    }

    // Update is called once per frame
    void Update()
    {
        

        if(_player != null)
        {
            TankerMode();

            if (Time.time > _canFire)
            {
                
                _fireRate = Random.Range(1f, 2f);
                _canFire = Time.time + _fireRate;
                FireLaser();

            }

        }

        if (_lives >= 2)
        {
            _lives = 2;
        }

    }

    public void FireLaser()
    {
        GetComponent<LineRenderer>().enabled = true;
        _audioSourceLaser.Play();

        StartCoroutine(DisableLaser());
    }
    IEnumerator DisableLaser()
    {
        yield return new WaitForSeconds(3f);
        GetComponent<LineRenderer>().enabled = false;
 
    }



    public void Damage()
    {


        _lives--;


         if (_lives == 1)
        {
            StartCoroutine(ShieldPowerDownRoutine());

        }


        
        if (_lives <= 0)
        {

            _lives = 0;
           // _isShieldPowerupActive = false;
            _deathAnim.SetTrigger("OnTankerDeath");
            _audioSourceExplosion.Play();
            Destroy(gameObject, 1f);


        }

    }
    IEnumerator ShieldPowerDownRoutine()
    {

        _tankerHurtShield.SetActive(true);
        _tankerShield.SetActive(false);
        yield return new WaitForSeconds(0.30f);
        _tankerHurtShield.SetActive(false);
        _tankerShield.SetActive(true);
        //_isShieldPowerupActive = true;



    }


    public void TankerMode()
    {


        if (_wayPointIndex <= _wayPoint.Length - 1)
        {
            transform.position = Vector3.MoveTowards(transform.position, _wayPoint[_wayPointIndex].transform.position, _speed * Time.deltaTime);
        }
        if (transform.position == _wayPoint[_wayPointIndex].transform.position)
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
    
    void OnTriggerEnter2D(Collider2D other)
    {

        //if other is player damage player then destroy us 
        if (other.tag == "Player")
        {
            other.transform.GetComponent<Player>().Damage();
            StartCoroutine(ShieldPowerDownRoutine());
        }
        //if other is laser destroy laser and then us 
        if (other.tag == "Player_Laser")
        {
 

            Destroy(other.gameObject);

            // add 10 to the score
            if (_player != null)
            {
                _player.AddScore(25);
            }
          
            

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
            _deathAnim.SetTrigger("OnTankerDeath");
            _speed = 0;
            Destroy(GetComponent<LaserSeeker>());
            _audioSourceExplosion.Play();
            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject);

        }

    }
}
