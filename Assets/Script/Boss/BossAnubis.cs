using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class BossAnubis : MonoBehaviour
{
    // private bool _crosshands = false;
 
    [SerializeField]private float _speed = 3f;
   
    [SerializeField] private float _bossLives= 100;

    public Sprite _openEyes,_openEyesRoar, _closedEyes, _closedEyesCharge;

    private HealthBar _healthBar;
    private Player _player;
    private int _bossMovement = 1;
    private int _bossAttack;

    private float _canFire;
    [SerializeField] private float _fireRate = 0.5f;
    private bool _sideToSide = false;
    private bool _bossLoading = false;
    private bool _anubisJudgement = false;
     private bool _isBossAttacking = false;
     private bool _bossAi = false;
    private bool _isAnubisRushing = false;
    private bool _isAnubisRoaring = false;

    [SerializeField] private AudioClip _roarAudio;
     private AudioSource _audioSource;


    [SerializeField] private Hand_Swap _leftBossHand,_rightBossHand;
    [SerializeField] private GameObject _rainJudgementPrefab, _anubisLaser, _anubisWave, _anubisSlash, _particlesCharge;
    [SerializeField] private GameObject _bossOrbsPrefab;
    [SerializeField] private Animator _leftSwipe, _rightSwipe, _leftSlash, _rightSlash,_cameraShake;

    // Start is called before the first frame update
    void Start()
    {
        _bossLives = 100;
        transform.position = new Vector3(0, 8.5f, 0);
        _particlesCharge.SetActive(false);
        _bossLoading = true;
        _rightBossHand.ClosedHand();
        _leftBossHand.ClosedHand();
        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null)
        {
            Debug.LogError("Audio Source Null");
        }
        _cameraShake = GameObject.FindGameObjectWithTag("Virtural_Camera").GetComponent<Animator>();
        if (_cameraShake == null)
        {
            Debug.LogError("V-Camera Null");
        }
        _leftBossHand.GetComponentInChildren<SpriteRenderer>();
        if (_leftBossHand == null)
        {
            Debug.LogError("left hand Null");
        }
        _rightBossHand.GetComponentInChildren<SpriteRenderer>();
        if (_rightBossHand == null)
        {
            Debug.LogError("right hand Null");
        }
        _healthBar = GameObject.Find("Health Bar").GetComponent<HealthBar>();
        if (_healthBar == null)
        {
            Debug.LogError("Boss health bar is Null");
        }
        _player = GameObject.Find("Player").GetComponent<Player>();
        if (_player == null)
        {
            Debug.LogError("Player null");
        }
       
    }

    // Update is called once per frame
    void Update()
    {
       
        if (_bossLoading == true && transform.position.y >= 2.3f && _isAnubisRushing == false)
        {
            
            BossLoadingMovement();
        }
        if(_sideToSide == true)
        {
           SidewaysMovements();
            

        }
        if(_isBossAttacking == true)
        {
            BossAttack();
        }
       
        

 


    }



   


    /// <summary>
    /// Boss Movement Section
    /// </summary>
    /// 
    void BossLoadingMovement()
    {
        if(_bossLoading == true)
        {
            transform.Translate(Vector3.down * _speed * Time.deltaTime);

            if (transform.position.y <= 2.3f && _isBossAttacking == false && _isAnubisRushing == false)
            {
                _bossLoading = false;
                _sideToSide = true;
                _isAnubisRoaring = true;
                if (_isAnubisRoaring == true && _isBossAttacking == false)
                {
                    AnubisRoar();
                }

            }
        }
       
    }
   


  

    


    /// <summary>
    /// Anubis Judgement
    /// </summary>
    public void AnubisJudgement()
    {
        _isAnubisRoaring = true;
        _anubisJudgement = true;
        StartCoroutine(JudgementTrigger());
   
    }
    IEnumerator JudgementTrigger()
    {
        _sideToSide = false;
        if (_anubisJudgement == true)
        {
            
           _leftBossHand.openhandGlow();
            _rightBossHand.openhandGlow();
      
            Instantiate(_rainJudgementPrefab, transform.position + new Vector3(0, -2f, 0), Quaternion.identity);
            GetComponent<SpriteRenderer>().sprite = _openEyesRoar;
            yield return new WaitForSeconds(3f);
            GetComponent<SpriteRenderer>().sprite = _closedEyes;

             _leftBossHand.ClosedHand();
             _rightBossHand.ClosedHand();

            _sideToSide = true;
            _anubisJudgement = false;
            _isBossAttacking = false;
            _bossAi = false;

        }
    }




    /// <summary>
    /// Anubis Tri-Beam Attack
    /// </summary>
    public void AnubisLaser()
    {
        _isAnubisRoaring = true;
        StartCoroutine(EyeOpener());

    }

    IEnumerator EyeOpener()
    {
        GetComponent<SpriteRenderer>().sprite = _openEyes;
        _anubisLaser.SetActive(true);
        yield return new WaitForSeconds(2f);
        _anubisLaser.SetActive(false);
        GetComponent<SpriteRenderer>().sprite = _closedEyes;
        _isBossAttacking = false;
    }



    /// <summary>
    /// Anubis Slash Attack
    /// </summary>
    public void SlashAttack()
    {
        _isAnubisRoaring = true;
        StartCoroutine(AnubisSlash());

    }

    IEnumerator AnubisSlash()
    {
        _rightBossHand.OpenHand();
        _leftBossHand.OpenHand();
        GetComponent<SpriteRenderer>().sprite = _openEyesRoar;
        _leftSlash.SetTrigger("Left Hand Slash"); _rightSlash.SetTrigger("Right Hand Slash");
        yield return new WaitForSeconds(.7f);
        GameObject enemyLaser = Instantiate(_anubisSlash, transform.position + new Vector3(0, 0-2, 0), Quaternion.identity);
        Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();

        for (int i = 0; i < lasers.Length; i++)
        {
            lasers[i].AssignEnemyLaser();
        }
        yield return new WaitForSeconds(1);
        _rightBossHand.ClosedHand();
        _leftBossHand.ClosedHand();
        yield return new WaitForSeconds(1);
        GetComponent<SpriteRenderer>().sprite = _closedEyes;
        
        _isBossAttacking = false;
    }


    

    /// <summary>
    /// Lives Counter
    /// </summary>
    /// <param name="lives"></param>
    public void Damage(int lives)
    {
        _bossLives -= lives;

        if (_bossLives <= 0)
        {
            _bossLives = 0;
            Destroy(gameObject);
        }

        if (_bossLives <= 75)
        {
          
            AnubisRoar();

            StartCoroutine(NextStage());

        }
        if (_bossLives <= 50)
        {
            AnubisRoar();
            StartCoroutine(NextStage());
        }
        if (_bossLives <= 25)
        {
            AnubisRoar();
            StartCoroutine(NextStage());
        }


        _healthBar.UpdateBossLives(_bossLives);
    }


    public void SidewaysMovements()
    {

        if (transform.position.x <= -5.3f)
        {
            _bossMovement = 1;
        }
        else if (transform.position.x >= 5.3f)
        {
            _bossMovement = -1;
        }
        transform.Translate(Vector3.right * _speed * _bossMovement * Time.deltaTime);
        StartCoroutine(BattleWait());
    }

    IEnumerator BattleWait()
    {

        yield return new WaitForSeconds(2);
        _isBossAttacking = true;
        _bossAi = true;
    }

    IEnumerator NextStage()
    {
        StopCoroutine(ChargeUpWarning()); StopCoroutine(ChargingLights());
        yield return new WaitForSeconds(5);
        _bossAttack = 0;
        if(_bossAttack== 0 && _bossLives <= 75)
        {
            _bossLives = 1;
        }
        if(_bossAttack == 1 && _bossLives <= 50)
        {
            _bossAttack = 2;
        }
        if(_bossAttack  == 2 && _bossLives <= 25)
        {
            _bossAttack = 3;
        }

    }

    /// <summary>
    /// Attack Timer
    /// </summary>
    public void BossAttack()
    {

       

        if (Time.time > _canFire && _isBossAttacking == true)
        {
            _fireRate = 8f;
            _canFire = Time.time + _fireRate;
            if (_sideToSide == true && _isBossAttacking == true)
            {

                StartCoroutine(ChargeUpWarning());
                StartCoroutine(ChargingLights());
            }
           


        }
       




    }


    /// <summary>
    /// Anubis Punch attack
    /// </summary>

    public void AnubisPunch()
    {
        _isAnubisRoaring = true;
        _rightSwipe.SetTrigger("Right Hand Swipe"); _leftSlash.SetTrigger("Left Hand Swipe");
        StartCoroutine(AnubisWave());
    }

    IEnumerator AnubisWave()
    {
        yield return new WaitForSeconds(.5f);
        GameObject enemyLaser = Instantiate(_anubisWave, transform.position + new Vector3(0, -3, 0), Quaternion.identity);
        Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();

        for (int i = 0; i < lasers.Length; i++)
        {
            lasers[i].AssignEnemyLaser();
        }
        _isBossAttacking = false;
    }


    /// <summary>
    /// Roar Warning
    /// </summary>
    public void AnubisRoar()
    {
        StopCoroutine(ChargeUpWarning()); StopCoroutine(ChargingLights());
        _isBossAttacking = false;
        _sideToSide = false;
        StartCoroutine(RoarTimer());

    }
    IEnumerator RoarTimer()
    {
        if (_isAnubisRoaring == true)
        {
            _speed = 0;
            _audioSource.clip = _roarAudio;
            _audioSource.Play();
            GetComponent<SpriteRenderer>().sprite = _openEyesRoar;
            _cameraShake.SetTrigger("Camera Animation Shake");
            yield return new WaitForSeconds(2);
            _cameraShake.ResetTrigger("Camera Animation Shake");
            GetComponent<SpriteRenderer>().sprite = _closedEyes;
            yield return new WaitForSeconds(1);

        }
        _sideToSide = true;
        _isAnubisRoaring = false;
        _isBossAttacking = true;
        _speed = 3;
    }




 
  
    /// <summary>
    /// Acttack Warning and boss Ai
    /// </summary>
    /// <returns></returns>
    IEnumerator ChargeUpWarning()
    {
        
        _particlesCharge.SetActive(true);
        GetComponent<SpriteRenderer>().sprite = _closedEyesCharge;
        yield return new WaitForSeconds(3f);
         _particlesCharge.SetActive(false);
        
        if (_bossAi == true)
                {

            
             
            switch (_bossAttack)
                    {
                        case 0:
                    AnubisPunch();
                    

                    break;
                        case 1:
                            SlashAttack();
           
                    break;
                        case 2:
                            AnubisLaser();
            
                    break;
                        case 3:
                    AnubisJudgement();
               
                    break;
                        default:
                            Debug.Log("Default Value");
                            break;
                    }
                   
                }

    }


    IEnumerator ChargingLights()
    {
        
            GetComponent<SpriteRenderer>().sprite = _closedEyesCharge;
            yield return new WaitForSeconds(0.5f);
            GetComponent<SpriteRenderer>().sprite = _closedEyes;
            yield return new WaitForSeconds(0.5f);
            GetComponent<SpriteRenderer>().sprite = _closedEyesCharge;
            yield return new WaitForSeconds(0.5f);
            GetComponent<SpriteRenderer>().sprite = _closedEyes;
            yield return new WaitForSeconds(0.5f);
            GetComponent<SpriteRenderer>().sprite = _closedEyesCharge;
            yield return new WaitForSeconds(0.5f);
            GetComponent<SpriteRenderer>().sprite = _closedEyes;
            yield return new WaitForSeconds(0.5f);
        


    }




    /// <summary>
    /// Collider Triggers
    /// </summary>
    /// <param name="other"></param>

    void OnTriggerEnter2D(Collider2D other)
    {

        //if other is player damage player then destroy us 
        if(other.tag == "Player")
        {
            other.transform.GetComponent<Player>().Damage();

            _speed = 0;
            Destroy(gameObject, 1.1f);
            Destroy(GetComponent<Collider2D>());

        }

        //if other is laser destroy laser and then us 
        if(other.tag == "Player_Laser")
        {

            Destroy(other.gameObject);
            
            // add 10 to the score
            if (_player != null)
            {
                Damage(25);
                
            }



        }

        if(other.tag == "Enemy_Laser")
        {
        

        }

        if(other.tag == "Wipe_Out_Laser")
        {
            Destroy(other.gameObject);
            Damage(15);
            // add 10 to the score
            if (_player != null)
            {
                _player.AddScore(10);
            }
            


        }

    }



}
