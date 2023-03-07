using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class UIManager : MonoBehaviour
{
    //Lives UI
    private Player _maxLives;

    //this is a variable slot that allows the stage of "lives display image" within the the canvas/UIManager folder in the Hierachy
    [SerializeField] private Image _livesImages;
    [SerializeField] private Image _shieldImages;
    [SerializeField] private Image _ammoImages;


    //Thruster Slider
    [SerializeField] private Slider _thrusterBar;
    [SerializeField] TMP_Text _thrusterBarPercentage;
    

    //GameOver GameObjects    
    [SerializeField] private GameObject _gameOverText;
    [SerializeField] private GameObject _restartLevelText;


    //this is a referance to a sprite/image [] = array or list
    [SerializeField] private Sprite[] _liveSprites = new Sprite[4];
    [SerializeField] private Sprite[] _shieldSprite = new Sprite[4];
    [SerializeField] private Sprite[] _ammoSprites = new Sprite[16];
    

    //Score Text
    [SerializeField] private TMP_Text _scoreText;


    //Other Scripts Hooks
    private GameManager _gameManager;





    // Start is called before the first frame update
    void Start()
    {
        //_thrusterBar.value = 100;
        //_liveSprites[CurrentPlayer = 3];
        _scoreText.text = "Score:" + 0;
        _shieldImages.gameObject.SetActive(false);
        //the .gameobject causes unity to reconize the text variables as gameobjects
        _gameOverText.gameObject.SetActive(false);
        _gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        _maxLives = GameObject.Find("Player").GetComponent<Player>();
        

        if (_gameManager == null)
        {

            Debug.LogError("Game manager is null");
        }

    }




    //Update Sscore 
    public void UpdateScore(int playerScore)
    {
        _scoreText.text = "Score:" + playerScore;
        if(playerScore == 1000)
        {
            SceneManager.LoadScene(0);
        }
        
       
    }




    //Update Ammo
    public void UpadateAmmo(int currentAmmo)
    {
        _ammoImages.sprite = _ammoSprites[currentAmmo];
        
    }




    //Update Shiled
    public void UpdateShield(int currentShield)
    {
        _shieldImages.gameObject.SetActive(true);
        _shieldImages.sprite = _shieldSprite[currentShield];

    }




    //Update thrusters
    public void UpdateThrusterBoost(float currentBoostLevel)
    {
        currentBoostLevel = Mathf.Clamp(currentBoostLevel,0f, 100f);
        _thrusterBar.value = currentBoostLevel;
        _thrusterBarPercentage.text = Mathf.RoundToInt(currentBoostLevel) + "%";
                  
    }
    



    //Update Lives
    public void UpdateLives(int currentLives)
    {
        
        //display image sprite
        //get a new one based on the currentLives index
        _livesImages.sprite = _liveSprites[currentLives];
       
        if (currentLives >= 3)
        {
            currentLives = 3;
            _maxLives.LifePowerupDisabled();
            Debug.Log("Max Lives Reached");
        }
       
            


        if (currentLives == 0)
        {
            _gameManager.GameOver();
            //_gameOverText.gameObject.SetActive(true);
            StartCoroutine(GameOverFlickerRoutine());
            StartCoroutine(RestartLevelRoutine());
            
        }

        
    
    }

    /// <summary>
    /// End of Level Text
    /// </summary>
    /// <returns></returns>

    IEnumerator GameOverFlickerRoutine()
    {
        while (true)
        {
            _gameOverText.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.3f);
            _gameOverText.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.3f);
        }
    }
    IEnumerator RestartLevelRoutine()
    {
        while (true)
        {
            _restartLevelText.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.9f);
            _restartLevelText.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.9f);
        }
    }

    


}
