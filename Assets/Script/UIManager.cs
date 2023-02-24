using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class UIManager : MonoBehaviour
{
    //this is a variable slot that allows the stage of "lives display image" within the the canvas/UIManager folder in the Hierachy
    [SerializeField] private Image _livesImages;
    private Player _maxLives;
    [SerializeField] private GameObject _gameOverText;
    [SerializeField] private GameObject _restartLevelText;

    //this is a referance to a sprite/image [] = array or list
    [SerializeField] private Sprite[] _liveSprites;
    [SerializeField] private TMP_Text _scoreText;

    private GameManager _gameManager;

    // Start is called before the first frame update
    void Start()
    {
        //_liveSprites[CurrentPlayer = 3];
        _scoreText.text = "Score:" + 0;
        //the .gameobject causes unity to reconize the text variables as gameobjects
        _gameOverText.gameObject.SetActive(false);
        _gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        _maxLives = GameObject.Find("Player").GetComponent<Player>();


        if (_gameManager == null)
        {

            Debug.LogError("Game manager is null");
        }

    }
    //update score 
   public void UpdateScore(int playerScore)
    {
        _scoreText.text = "Score:" + playerScore;
        if(playerScore == 100)
        {
            SceneManager.LoadScene(0);
        }
    }


    public void UpdateLives(int currentLives)
    {
       
        //display image sprite
        //get a new one based on the currentLives index
        _livesImages.sprite = _liveSprites[currentLives];
       
        if (currentLives == 3)
        {
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
