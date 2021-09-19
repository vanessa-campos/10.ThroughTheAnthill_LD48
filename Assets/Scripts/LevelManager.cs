using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class LevelManager : MonoBehaviour
{
    public int currentScene;
    [SerializeField] GameObject panelPlay, panelLevelCompleted, TextLevelCompleted, panelGameOver, panelLevelTitle;
    [SerializeField] TextMeshProUGUI levelText;
    [HideInInspector] public bool levelCompleted, gameOver;

    public FloatingJoystick floatingJoystick;
    public TextMeshProUGUI pointsText, barText, leafText;
    public Slider bar;
    public Image[] ants;
    public GameObject leavesText, tipText;

    GameManager gameManager;
    GameObject minimap;

    private void Start(){
        currentScene = SceneManager.GetActiveScene().buildIndex;
        minimap = GameObject.FindGameObjectWithTag("minimap");
        leavesText.SetActive(false);
        tipText.SetActive(false);
        minimap.SetActive(false);
        TextLevelCompleted.SetActive(false);
        StartCoroutine(StartLevel());
    }

    IEnumerator StartLevel(){
        levelText.text = "Level " + (currentScene);
        panelLevelTitle.SetActive(true);
        yield return new WaitForSeconds(3);
        panelLevelTitle.SetActive(false);
        panelPlay.SetActive(true);
        minimap.SetActive(true);
#if UNITY_ANDROID
        floatingJoystick.gameObject.SetActive(true);
#endif
    }

    private void Update(){
        if (levelCompleted){
            panelLevelCompleted.SetActive(true);
            panelPlay.SetActive(false);
            if (currentScene >= 3){
                TextLevelCompleted.SetActive(true);
                LoadScene(0, 3);
            }else{
                LoadScene(currentScene + 1, 1);
            }
            PlayerPrefs.SetInt("PPLvlsWon", currentScene + 1);
        }

        if (gameOver){
            panelPlay.SetActive(false);
            panelGameOver.SetActive(true);
            if (Input.anyKeyDown){
                gameOver = false;
                LoadScene(currentScene, 1);
            }
        }

        if (Input.GetButtonDown("Cancel")){
            LoadScene(0, 1);
        }
    }

    IEnumerator SceneDelay(int SceneNumber, float delay){
        yield return new WaitForSecondsRealtime(delay);
        SceneManager.LoadScene(SceneNumber);
    }

    public void LoadScene(int SceneNumber, float delay = 0){
        StartCoroutine(SceneDelay(SceneNumber, delay));
    }
}
