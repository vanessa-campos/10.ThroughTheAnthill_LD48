using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class LevelManager : MonoBehaviour
{
    [SerializeField] GameObject panelPlay;
    [SerializeField] GameObject panelLevelCompleted;
    [SerializeField] GameObject TextLevelCompleted;
    [SerializeField] GameObject panelGameOver;
    [SerializeField] GameObject panelLevelTitle;
    [SerializeField] TextMeshProUGUI levelText;
    [HideInInspector] public bool levelCompleted;
    [HideInInspector] public bool gameOver;

    public TextMeshProUGUI pointsText;
    public TextMeshProUGUI barText;
    public TextMeshProUGUI leafText;
    public Slider bar;
    public Image[] ants;
    public GameObject leavesText;
    public GameObject tipText;
    GameManager gameManager;
    GameObject minimap;





    private void Start()
    {
        gameManager = GetComponent<GameManager>();
        minimap = GameObject.FindGameObjectWithTag("minimap");
        gameManager.currentScene = SceneManager.GetActiveScene().buildIndex;
        leavesText.SetActive(false);
        tipText.SetActive(false);
        minimap.SetActive(false);
        TextLevelCompleted.SetActive(false);
        StartCoroutine(StartLevel());
    }

    IEnumerator StartLevel()
    {
        levelText.text = "Level " + (gameManager.currentScene);
        panelLevelTitle.SetActive(true);
        yield return new WaitForSeconds(3);
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
