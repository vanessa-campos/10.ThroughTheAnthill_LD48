using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    [SerializeField] GameObject panelPlay;
    [SerializeField] GameObject panelLevelCompleted;
    [SerializeField] GameObject TextLevelCompleted;
    [SerializeField] GameObject panelGameOver;
    [SerializeField] GameObject panelLevelTitle;
    [SerializeField] Text levelText;
    [HideInInspector] public bool levelCompleted;
    [HideInInspector] public bool gameOver;

    public Text pointsText;
    public Text barText;
    public Text leafText;
    public Slider bar;
    public Image[] ants;
    public GameObject leavesText;
    public GameObject tipText;
    public GameObject helpText;
    GameManager gameManager;
    GameObject minimap;




    // IEnumerator StartLevel()
    // {
    //     yield return new WaitForSeconds(3);
    // }
    public void OnMouseEnter()
    {
        helpText.SetActive(true);
    }
    public void OnMouseExit()
    {
        helpText.SetActive(false);
    }

    private void Start()
    {
        gameManager = GetComponent<GameManager>();
        minimap = GameObject.FindGameObjectWithTag("minimap");
        gameManager.currentScene = SceneManager.GetActiveScene().buildIndex;
        leavesText.SetActive(false);
        tipText.SetActive(false);
        minimap.SetActive(false);
        TextLevelCompleted.SetActive(false);
        helpText.SetActive(false);
        StartCoroutine(StartLevel());
    }

    IEnumerator StartLevel()
    {
        levelText.text = "Level " + (gameManager.currentScene);
        panelLevelTitle.SetActive(true);
        yield return new WaitForSeconds(2);
        panelLevelTitle.SetActive(false);
        panelPlay.SetActive(true);
        minimap.SetActive(true);
    }
    private void Update()
    {

        if (levelCompleted)
        {
            panelLevelCompleted.SetActive(true);
            panelPlay.SetActive(false);
            if (gameManager.currentScene == 2)
            {
                TextLevelCompleted.SetActive(true);
                gameManager.LoadScene(0, 3);
            }
            else
            {
                gameManager.LoadScene(gameManager.currentScene + 1, 3);
            }
        }
        if (gameOver)
        {
            panelPlay.SetActive(false);
            panelGameOver.SetActive(true);
            if (Input.anyKeyDown)
            {
                gameOver = false;
                gameManager.LoadScene(gameManager.currentScene, 2);
            }
        }
        if (Input.GetButtonDown("Cancel"))
        {
            gameManager.LoadScene(0, 2);
        }


    }

}
