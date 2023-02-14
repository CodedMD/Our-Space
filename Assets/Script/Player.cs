using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //This describe Variables
    //public(can be accessed in the inspector and by other object) or private reference(can not be access by outside elements)
    //[SerializedField] gives limited access to other editor helping on a project for private Variables
    //data type (int, float, bool, string)
    //every variable has a name 
    //optional value assigned
    [SerializeField]
    private float speed = 3.5f;
    /// <summary>
    /// global variable are not needed for input variable
    /// </summary>(public float rightleftInput;)
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private float _fireRate = 0.5f;

    private float _canfire = 1.0f;
    [SerializeField]
    private int Lives = 4;
    private SpawnManager _spawnManager;
    // Start is called before the first frame update
    void Start()
    {
        
        //take the current position = new position (0,0,0)
         transform.position = new Vector3(0, 0, 0);
        //
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        //
        if (_spawnManager == null)
        {
            //
            Debug.LogError("The Spawn Manager did not Spawn");
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
            
            lasertime(); 
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
        float rightleftInput = Input.GetAxis("Horizontal");
        float updownInput = Input.GetAxis("Vertical");

        ///a new local variable that incorparate the to other local variables
        //Vector3 direction = new Vector3(rightleftInput, updownInput, 0);


        ///Translate Opens the access to direct an object
        ///Vector3.right = new Vector3(1, 0, 0) move to the right
        /// * Time.deltatime converts unity time to real life time or secs
        ///if youu add a number like 5 between verctor3.direction and Time.deltaTime instead of 1 meter per sec it will move 5 meters per sec
        transform.Translate(Vector3.right * rightleftInput * speed * Time.deltaTime);
        //transform.Translate(Vector3.right * speed * Time.deltaTime);

        /// When input is add to this line it causes the object the scrpit is on to wait for Input
        /// using the updownInput calls the "Vertical" Axis
        /// using the Vecvtor3.up as a start set the movement to a nuetral point for vertical as does right for horizontal
       transform.Translate(Vector3.up * updownInput * speed * Time.deltaTime);
      //  transform.Translate(Vector3.up * speed * Time.deltaTime);

        ///this is a consolidate way to write the two transform lines above by using the Vector3 local Variable 
        ///to include both horitonal and vertical
        //transform.Translate(direction * speed * Time.deltaTime);

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

    void lasertime()
    {
        //if I hit the space key spawn Object?

        //player can fire after _canfire = 1.0f / measured by frame rate /plus _firerate = 0.5f 
         _canfire = Time.time + _fireRate;

        //Instantiate: spawns the object you call on/ _laserPrefab: the Object your calling on/ transform.position: player's position/
        //Quaternion.identity: is the rotation of the object, default rotation
        //new Vector3(0,0.8f,0): tell the laser to spawn just outside of the player
        Instantiate(_laserPrefab, transform.position + new Vector3(0, 0.8f, 0), Quaternion.identity);
        // Instantiate(_laserPrefab, transform.position, Quaternion.identity);

    }
    public void Damage()
    {
        Lives--;

        if (Lives < 1)
        {
            _spawnManager.OnPlayerDeath();
            Destroy(gameObject);
        }


    }






}
