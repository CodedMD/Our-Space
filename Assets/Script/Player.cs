using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Audio;
using Cinemachine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class Player : MonoBehaviour
{
    /// <summary>
    /// global variable are not needed for input variable
    /// </summary>(public float rightleftInput;)


    // Lives
    [SerializeField] private int _lives = 3;


    //Score
    [SerializeField] private int _score;
  

    //Speed
    [SerializeField] private float _speed = 3.5f;
    [SerializeField] private float _speedupIsActive = 4.0f;


    //Ammo
    [SerializeField] private float _fireRate = 0.5f;
    [SerializeField] private int _maxAmmo = 15;
    private bool _isAmmoActive = true;
   
    


    //Audio - the variable for you sound - the reference to yor audio source attached to the player
    private float _canFire = 1.0f;
    [SerializeField] private AudioClip _laserAudio, _playerDeathAudio,_thrusterBoostAudio;
    [SerializeField] private AudioSource _audioSource;




    //Basic Game Objects
    [SerializeField] private GameObject _homingFirePrefab;
    [SerializeField] private GameObject _laserPrefab;
    [SerializeField] private GameObject _rightEngine;
    [SerializeField] private GameObject _leftengine;

    [SerializeField] private Sprite _wipeOutShip, _baseShip;


    //powerups
    [SerializeField] private GameObject _wipeOutPrefab;
    private bool _isWipeOutPowerupActive = false;
    [SerializeField] private GameObject _tripleShotPrefab;
    private bool _isTripleShotActive = false;
    [SerializeField] private GameObject _speedupPrefab;
    private bool _isSpeedPowerupActive = false;
    [SerializeField] private GameObject _playerShield;
    private bool _isShieldPowerupActive = false;
    [SerializeField] private GameObject _playeHurtShield;
    private bool _isHurtShieldActive = false;
    [SerializeField] private int _shield = 0;


    //Other Powerups   
    private bool _isLifePowerupActive = false;
    private bool _isNegativePowerupActive = false;

    // Thrusters
    [SerializeField] private GameObject _thrusterBase;
    [SerializeField] private GameObject _thrusterBoost;
    [SerializeField] private float _thrusterBarPercentage;
    private bool _isThrusterBoostActive = false;
    private bool _thrusterRecover = false;
    private bool _isThrusterBaseActive = true;
    private bool _isHomingFireActive = false;

    //Other Scripts Hooks
    private UIManager _uiManager;
    private SpawnManager _spawnManager;
    private BossIntro _bossIntro;
    private float _distance;
    [SerializeField] private float _distanceBetween;

    private Enemy _enemy;



    // Start is called before the first frame update
    void Start()
    {
        
        _thrusterBarPercentage = 100;
        _maxAmmo = 15;
        //_thrusterBarPercentage = Mathf.Clamp(_thrusterBarPercentage, 0, 100);

        _audioSource = GetComponent<AudioSource>();
        transform.position = new Vector3(0, 0, 0);


        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        if(_isHomingFireActive == true)
        {
            Debug.LogError("homing Fire null");
        }
        if(_isNegativePowerupActive == true)
        {
            Debug.LogError("Negative Powerup is Active at start on player");
        }

        if(_isThrusterBoostActive == true)
        {
            Debug.LogError("Thruster Boost is Active on Player");
        }

        if (_isWipeOutPowerupActive == true)
        {
            Debug.LogError("WipeOut Power up is Active on player");
        }

        if (_isAmmoActive == false)
        {
            Debug.LogError("Ammo is not Active on the player");
        }

        if (_isHurtShieldActive == true)
        {
            Debug.LogError("_ishurtShield is Active From Start");
        }

        if (_isLifePowerupActive == true)
        {
            Debug.LogError("_isLifePowerupActive is Active from Start");
        }
        if (_spawnManager == null)
        {
            //
            Debug.LogError("The Spawn Manager did not Spawn");
        }
        if (_uiManager == null)
        {

            Debug.LogError("the UI manager is null");
        }
        if (_audioSource == null)
        {
            Debug.LogError("Audio is null on player");
        }
       
        
    }










  
    // Update is called once per frame
    void Update()
    {
        //this calls on the player movement void
       CalculateMovement();
       
        
        if (_maxAmmo <= 0)
        {
            _maxAmmo = 0;
            _isAmmoActive = false;
            return;
        }
       
       
        
        //if I hit the space key spawn Object/ measured in frate rate/after _canfire = 1.0f
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            LaserTime();
           
        }
        if (_lives >= 3)
        {
            _lives = 3;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (_thrusterRecover == false)
            {
                _audioSource.clip = _thrusterBoostAudio;
                _audioSource.Play();
            }
                
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (_thrusterRecover == false)
            {
                ThrusterBoostActive();
                _speed = 9.5f;
            }
             else if (_thrusterRecover == true)
            {
                ThrusterBaseActive();
                _speed = 3.5f;
            }
            if (_thrusterBarPercentage <= 0f)
            {
                _thrusterBarPercentage = 0;
                ThrusterBaseActive();
               
            }
            Debug.LogError("Thruster initiated");
            StopCoroutine(ThrusterRecoverRoutine());
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            _speed = 3.5f;
            ThrusterBaseActive();
          

        }
       else if (_thrusterBarPercentage <= 0)
        {
              
                StartCoroutine(ThrusterRecoverRoutine());
                ThrusterBaseActive();
            if(_thrusterBarPercentage >= 100)
            {
                _thrusterBarPercentage = 100;
                StopCoroutine(ThrusterRecoverRoutine());
            }
        }
       
       


    }









    // This is the player movement
    void CalculateMovement()
    {
        
        ///"Horizontal" and "Vertical" are strings/name of the Axis that we want to access will calling the input up
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
    
         Vector3 direction = new Vector3(horizontalInput, verticalInput, 0 );

        ///this is a consolidate way to write the two transform lines above by using the Vector3 local Variable 
        ///to include both horitonal and vertical
        transform.Translate(direction * _speed * Time.deltaTime);
        if (_isSpeedPowerupActive == false)
        {
            transform.Translate(direction * _speed * Time.deltaTime);
        }
        else
        {
            transform.Translate(direction * _speed *_speedupIsActive* Time.deltaTime);
        }

        //if the player position on the y is greater than 0 
        //y position = 0
        if (transform.position.y >= 1)
        {
            transform.position = new Vector3(transform.position.x, 1, 0);
        }
        else if (transform.position.y <= -3.5f)
        {
            transform.position = new Vector3(transform.position.x, -3.5f);
        }
        //if player pos on the x > 11.3f 
        //x pos = -11.3f
        //else if play on the x is less than -11.3f
        //x pos =11.3f
        if (transform.position.x >= 9.3f)
        {
            transform.position = new Vector3(-9.3f, transform.position.y, 0);
        }
        else if (transform.position.x <= -9.3f)
        {
            transform.position = new Vector3(9.3f, transform.position.y, 0);
        }
    }





    // Players Laser
    void LaserTime()
    {
       //player can fire after _canfire = 1.0f / measured by frame rate /plus _firerate = 0.5f 
         _canFire = Time.time + _fireRate;
        _maxAmmo--;
        
        
        _uiManager.UpadateAmmo(_maxAmmo);
        


        if (_isTripleShotActive == true)
        {
            
            Instantiate(_tripleShotPrefab, transform.position + new Vector3(0, 1.05f, 0), Quaternion.identity);
            _audioSource.clip = _laserAudio;
            _audioSource.Play();
        }
        else if (_isWipeOutPowerupActive == true)
        {
            
            Instantiate(_wipeOutPrefab, transform.position + new Vector3(0, 0, 0), Quaternion.identity);
            _audioSource.clip = _thrusterBoostAudio;
            _audioSource.Play();
        }
        else if (_isHomingFireActive)
        {
            Instantiate(_homingFirePrefab, transform.position + new Vector3(0, 0, 0), Quaternion.identity);
            _audioSource.clip = _laserAudio;
            _audioSource.Play();
        }
       else 
        {
            Instantiate(_laserPrefab, transform.position + new Vector3(0, 1.05f, 0), Quaternion.identity);
            _audioSource.clip = _laserAudio;
            _audioSource.Play();
        }
       

    }




    //Player Damage
    public void Damage()
    {
        

        if (_isShieldPowerupActive == true)
        {

            _shield--;
            _uiManager.UpdateShield(_shield);


            if (_shield == 3)
            {
               
               
                StartCoroutine(ShieldPowerDownRoutine());
               
                return;
            }
            if (_shield == 2)
            {
              
              
                StartCoroutine(ShieldPowerDownRoutine());
             
                return;
            }
            if (_shield == 1)
            {
                
              
                StartCoroutine(ShieldPowerDownRoutine());
                return;
            }
             if (_shield <= 0)
            {
                _shield = 0;
                _playerShield.SetActive(false);
                _isShieldPowerupActive = false;
                
                return;
            }


            
        }

       
        _lives--;

        if (_lives == 2)
        {
            
            _rightEngine.SetActive(true);
            
        }
        else if (_lives == 1)
        {

            _leftengine.SetActive(true);
        }

       
        _uiManager.UpdateLives(_lives);

        if (_lives < 1)
        {
            _spawnManager.OnPlayerDeath();
            Destroy(gameObject);
           
          
        }
    }


    /// <summary>
    /// PowerUp Section
    /// </summary>




    public void NegativePowerup()
    {
        _isNegativePowerupActive = true;
        if (_isNegativePowerupActive == true && _isShieldPowerupActive == true)
        {
            _shield--;
            _uiManager.UpdateShield(_shield);
            _maxAmmo = 0;
            _uiManager.UpadateAmmo(_maxAmmo);
           

        }
        else if (_isNegativePowerupActive == true)
        {
            _maxAmmo = 0;
            _uiManager.UpadateAmmo(_maxAmmo);
            _lives--;
            _uiManager.UpdateLives(_lives);
        }

        if (_lives == 2)
        {

            _rightEngine.SetActive(true);

        }
        else if (_lives == 1)
        {

            _leftengine.SetActive(true);
        }


       

        if (_lives < 1)
        {
            _spawnManager.OnPlayerDeath();
            Destroy(gameObject);

          
        }


    }



    //WipeOut Powerup Shot
    public void WipeOutPowerupActive()
    {
        _isWipeOutPowerupActive = true;
        if(_isWipeOutPowerupActive == true)
        {
           
            _maxAmmo = 6;
            _uiManager.UpadateAmmo(_maxAmmo);
            StartCoroutine(WipeOutPowerDownRoutine());
        }
       
    }
    IEnumerator WipeOutPowerDownRoutine()
    {
        GetComponent<SpriteRenderer>().sprite = _wipeOutShip;

        yield return new WaitForSeconds(5.0f);
        
        //Instantiate(_wipeOutPrefab, transform.position + new Vector3(0, 0, 0), Quaternion.identity);
        // yield return new WaitForSeconds(5.0f);
        _isWipeOutPowerupActive = false;
        GetComponent<SpriteRenderer>().sprite = _baseShip;
    }




    //Triple Shot 
    public void TripleShotActive()
    {
        _isTripleShotActive = true;
        if (_isTripleShotActive == true)
        {
            //Instantiate(_tripleShotPrefab, transform.position + new Vector3(0, 0, 0), Quaternion.identity);
            StartCoroutine(TripleShotPowerDownRoutine());
        }
      
    }

    IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isTripleShotActive = false;
    }


    // Homing Fire
    public void HomingFire()
    {
        _isHomingFireActive = true;
        if (_isHomingFireActive==true)
        {
            //Instantiate(_homingFirePrefab, transform.position + new Vector3(-0.88f, 2.11f, 0), Quaternion.identity);
            //_speed = 5;
            _maxAmmo = 15;
            _uiManager.UpadateAmmo(_maxAmmo);
            _audioSource.clip = _laserAudio;
            _audioSource.Play();
            StartCoroutine(HomingFirePowerDownRoutine());
        }
        

    }

    IEnumerator HomingFirePowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isHomingFireActive = false;
    }



    //Speedup 
    public void SpeedupPowerupActive()
    {
        _isSpeedPowerupActive = true;
        StartCoroutine(SpeedupPowerDownRoutine());
    }
    IEnumerator SpeedupPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isSpeedPowerupActive = false;
    }



    // Shield Power
    public void ShieldPowerupActive()
    {
        _isShieldPowerupActive = true;
        _shield = 3;
        _uiManager.UpdateShield(_shield);
       
        //easy way to activate and deactivate a game object in the hierarchy
        _playerShield.SetActive(true);
    }
    IEnumerator ShieldPowerDownRoutine()
    {
        _playeHurtShield.SetActive(true);
        yield return new WaitForSeconds(0.30f);
        _playeHurtShield.SetActive(false);
        _playerShield.SetActive(true);
        _isShieldPowerupActive = true;
    }



    //Ammo Power
    public void AmmoPowerupActive()
    {
        _isAmmoActive = true;
        _maxAmmo = 15;
        _uiManager.UpadateAmmo(_maxAmmo);
        Debug.LogError("Ammo Power Collected");
        
    }




    //Life PowerUp
    public void LifePowerupActive()
    {
        //set the life power to active
        _isLifePowerupActive = true;
        //add one life
        _lives++;
        //update lives and sprites
        _uiManager.UpdateLives(_lives);
        if (_lives == 3)
        {
            
            _leftengine.SetActive(false);
            _rightEngine.SetActive(false);
        }
        else if(_lives == 2)
        {
            _leftengine.SetActive(false);
        }
    }
    public void LifePowerupDisabled()
    {
        //set the life power to active
        _isLifePowerupActive = false;
       
    }

    /// <summary>
    /// Thruster Section
    /// </summary>


   
   void ThrusterBaseActive()
    {
        if (_isThrusterBaseActive == true)
        {
            _speed = 3.5f;
            _thrusterBase.gameObject.SetActive(true);
            _thrusterBoost.gameObject.SetActive(false);
        }
        
        
    }

    void ThrusterBoostActive()
    {
        
        if (_thrusterBarPercentage > 1 || _thrusterBarPercentage < 100)
        {
            _thrusterRecover = false;
           
            _isThrusterBoostActive = true;
           
            _thrusterBoost.gameObject.SetActive(true);
            
            _thrusterBarPercentage -= 10.0f * 5 * Time.deltaTime;
            Debug.LogError("Thruster Active");
            

        }
        _uiManager.UpdateThrusterBoost(_thrusterBarPercentage);
        
    }
    IEnumerator ThrusterRecoverRoutine()
    {
        while (_thrusterBarPercentage <= 100f)
        {
            
            yield return new WaitForSeconds(.8f);
            _thrusterRecover = true;
            _thrusterBarPercentage += 500 /100 * Time.deltaTime;
            _uiManager.UpdateThrusterBoost(_thrusterBarPercentage);
            yield return new WaitForSeconds(.05f);

            if (_thrusterBarPercentage >= 100)
            {
                _thrusterRecover = false;
                _thrusterBarPercentage = 100;
                _uiManager.UpdateThrusterBoost(_thrusterBarPercentage);
                Debug.LogError("Thruster Recovered");
                break;
            }
            
        }
        
    }



    /// <summary>
    /// Points Tracker Section
    /// </summary>
    
   
    public void AddScore(int points)
    {

        //Communicate with the UI to update the score!
        _score += points;
        //
        _uiManager.UpdateScore(_score);
        GameObject enemy = GameObject.Find("Enemy");

        if (_score >= 20 && this.gameObject != null)
        {
            Destroy(_enemy);
            _spawnManager.SpawnBossOrg();


        }
    }








}
