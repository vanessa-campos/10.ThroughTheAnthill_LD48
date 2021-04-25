using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject panelMenu;
    [SerializeField] GameObject panelPlay;
    [SerializeField] GameObject panelLevelCompleted;
    [SerializeField] GameObject panelGameOver;
    [SerializeField] GameObject player;
    [SerializeField] Text recordText;
    [HideInInspector] public bool levelCompleted;
    [HideInInspector] public bool gameOver;

    // int record;
    // public int Record { get { return record; } set { record = value; recordText.text = "RECORD: " + record; } }


    public void PlayClicked()
    {
        panelPlay.SetActive(true);
        panelMenu.SetActive(false);
        player.SetActive(true);
    }

    public void QuitClicked()
    {
        Application.Quit();
    }

    private void Start()
    {
        panelMenu.SetActive(true);
        player.SetActive(false);
    }

    private void Update()
    {
        if (levelCompleted)
        {
            panelLevelCompleted.SetActive(true);
            panelPlay.SetActive(false);
            StartCoroutine(BackToMenu());
        }
        if (gameOver)
        {
            panelPlay.SetActive(false);
            panelGameOver.SetActive(true);
            if (Input.anyKeyDown)
            {
                gameOver = false;
                StartCoroutine(BackToMenu());
            }
        }
        if (Input.GetButtonDown("Cancel"))
        {
            panelMenu.SetActive(true);
            panelPlay.SetActive(false);
        }
    }

    IEnumerator BackToMenu()
    {
        yield return new WaitForSeconds(5);
        panelMenu.SetActive(true);
        panelLevelCompleted.SetActive(false);
        panelGameOver.SetActive(false);
        SceneManager.LoadScene("Scene");
    }

}
