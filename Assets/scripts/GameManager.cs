using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
public class GameManager : MonoBehaviour
{

    public static GameManager singleton;

    private GroundPiece[] allGroundPieces;

    public GameObject gameWinScreen;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI finalScore;
    public int score = 0;
    public int scoreMultiplier = 0;

    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        SetupNewLevel();
        gameWinScreen.SetActive(false);
        audioSource = GetComponent<AudioSource>();
    }

    public void UpdateScoreText()=> scoreText.text = score.ToString();

    public void SetupNewLevel(){
        allGroundPieces = FindObjectsOfType<GroundPiece>();
    }

    public void  Awake(){
        if(singleton == null){
            singleton = this;
        }else if(singleton != this){
            Destroy(gameObject);
            DontDestroyOnLoad(gameObject);
        }
    }

    private void OnEnable(){
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    private void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode){
        SetupNewLevel();
    }

    public void CheckComplete(){
        bool isFinished = true;
        for(int i = 0; i < allGroundPieces.Length; i++){
            if(allGroundPieces[i].isColored == false){
                isFinished = false;
                break;
            }
        }

        if(isFinished){
            // Next Level
            ShowGameWinScreen();
        }
    }

    public void ShowGameWinScreen(){
        gameWinScreen.SetActive(true);
        finalScore.text = score.ToString();
        audioSource.Play();

    }

    public void Replay(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void NextLevel(){
        Debug.Log("To next level");
        if(SceneManager.GetActiveScene().buildIndex == 5){
            SceneManager.LoadScene(0);
        }else{
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
