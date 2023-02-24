using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Audio;
public class Player : MonoBehaviour
{
    //This describe Variables
    //public(can be accessed in the inspector and by other object) or private reference(can not be access by outside elements)
    //[SerializedField] gives limited access to other editor helping on a project for private Variables
    //data type (int, float, bool, string)
    //every variable has a name 
    //optional value assigned
    [SerializeField] private float _speed = 3.5f;
    [SerializeField] private float _speedupIsActive = 4.0f;
    /// <summary>
    /// global variable are not needed for input variable
    /// </summary>(public float rightleftInput;)
    [SerializeField] private GameObject _laserPrefab;
    [SerializeField] private GameObject _tripleShotPrefab;
    [SerializeField] private GameObject _speedupPrefab;
    [SerializeField] private GameObject _playerShield;
    [SerializeField] private GameObject _rightEngine;
    [SerializeField] private GameObject _leftengine;
    [SerializeField] private float _fireRate = 0.5f;
    [SerializeField] private int _lives = 4;
    //the variable for you sound
    [SerializeField] private AudioClip _laserAudio,_playerDeathAudio;
    //the reference to yor audio source attached to the player
    [SerializeField] private AudioSource _audioSource;
    private SpawnManager _spawnManager;
    private float _canfire = 1.0f;
    
    //isTripleShotActive
    private bool _isTripleShotActive = false;
    private bool _isSpeedPowerupActive = false;
    private bool _isShieldPowerupActive = false;
    private bool _isLifePowerupActive = false;
  
    [SerializeField] private int _score;
    //[SerializeField] private TMP_Text _scoreText;
    private UIManager _uiManager;








    // Start is called before the first frame update
    void Start()
    {
        
        _audioSource = GetComponent<AudioSource>();
        //take the current position = new position (0,0,0)
        transform.position = new Vector3(0, 0, 0);
        //
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        //// this creates a defualt access to the player script
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        //
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










    //Debug.Log("Space Key Presed");
    // Update is called once per frame
    void Update()
    {
        //this calls on the player movement void
       CalculateMovement();


        //if I hit the space key spawn Object/ measured in frate rate/after _canfire = 1.0f
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canfire)
         {
            
            LaserTime(); 
        }
        


    }









    // This is the player movement void 
    //CalculateMovement is a place holder and can be named anything as long as you know what is means like local variables
    void CalculateMovement()
    {
        ///</movement input Summary>
        ///local Variables inside of the update function
        ///allow you to create variables like inputs that need to be updated on the fly via the push of a button
        ///float starts the creation of the local variable 
        ///updownInput or rightleftInput are references to the Input map and can be named anything
        ///Input.GetAxis refers to the Input Axis option in Unity 
        ///"Horizontal" and "Vertical" are strings/name of the Axis that we want to access will calling the input up
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        ///a new local variable that incorparate the to other local variables
        //Vector3 direction = new Vector3(rightleftInput, updownInput, 0);
        

        ///Translate Opens the access to direct an object
        ///Vector3.right = new Vector3(1, 0, 0) move to the right
        /// * Time.deltatime converts unity time to real life time or secs
        ///if youu add a number like 5 between verctor3.direction and Time.deltaTime instead of 1 meter per sec it will move 5 meters per sec
        //transform.Translate(Vector3.right * rightleftInput * _speed * Time.deltaTime);*
        //transform.Translate(Vector3.right * speed * Time.deltaTime);

        /// When input is add to this line it causes the object the scrpit is on to wait for Input
        /// using the updownInput calls the "Vertical" Axis
        /// using the Vecvtor3.up as a start set the movement to a nuetral point for vertical as does right for horizontal
       //transform.Translate(Vector3.up * updownInput * _speed * Time.deltaTime);*
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
        if (transform.position.y >= 0)
        {
            transform.position = new Vector3(transform.position.x, 0, 0);
        }
        else if (transform.position.y <= -3.5f)
        {
            transform.position = new Vector3(transform.position.x, -3.5f);
        }
        //if player pos on the x > 11.3f 
        //x pos = -11.3f
        //else if play on the x is less than -11.3f
        //x pos =11.3f
        if (transform.position.x >= 11.3f)
        {
            transform.position = new Vector3(-11.3f, transform.position.y, 0);
        }
        else if (transform.position.x <= -11.3f)
        {
            transform.position = new Vector3(11.3f, transform.position.y, 0);
        }



    }







    void LaserTime()
    {
        //if I hit the space key spawn Object?

        //player can fire after _canfire = 1.0f / measured by frame rate /plus _firerate = 0.5f 
         _canfire = Time.time + _fireRate;
        

        //Instantiate: spawns the object you call on/ _laserPrefab: the Object your calling on/ transform.position: player's position/
        //Quaternion.identity: is the rotation of the object, default rotation
        //new Vector3(0,0.8f,0): tell the laser to spawn just outside of the player
        //Instantiate(_laserPrefab, transform.position + new Vector3(0, 0.8f, 0), Quaternion.identity);
        if (_isTripleShotActive == true)
        {
            
            Instantiate(_tripleShotPrefab, transform.position + new Vector3(-.6f, 1.05f, 0), Quaternion.identity);
        }
       else 
        {
            Instantiate(_laserPrefab, transform.position + new Vector3(0, 1.05f, 0), Quaternion.identity);
        }
        _audioSource.clip = _laserAudio;
        _audioSource.Play();

    }






    public void Damage()
    {
        if (_isShieldPowerupActive == true)
        {
            _isShieldPowerupActive = false;
            _playerShield.SetActive(false);
            return;
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
           
            //_uiManager.GameOver();
        }


    }







    public void TripleShotActive()
    {
        _isTripleShotActive = true;
        Instantiate(_tripleShotPrefab, transform.position + new Vector3(-.6f, 1.05f, 0), Quaternion.identity);
        StartCoroutine(TripleShotPowerDownRoutine());
    }

    IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isTripleShotActive = false;
    }

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
    public void ShieldPowerupActive()
    {
        _isShieldPowerupActive = true;
        //easy way to activate and deactivate a game object in the hierarchy
        _playerShield.SetActive(true);
       
    }
  
    public void LifePowerupActive()
    {
        //set the life power to active
        _isLifePowerupActive = true;
        //add one life
        _lives++;
        //update lives and sprites
        _uiManager.UpdateLives(_lives);
    }
    public void LifePowerupDisabled()
    {
        //set the life power to active
        _isLifePowerupActive = false;
       
    }


    //method to add 10 to the score
    public void AddScore(int points)
    {

        //Communicate with the UI to update the score!
        _score += points;
        //
        _uiManager.UpdateScore(_score);
    }
   







}
