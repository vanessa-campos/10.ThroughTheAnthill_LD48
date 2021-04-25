using System.Collections;
using UnityEngine;
using UnityEngine.UI;

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

    int record;
    public int Record { get { return record; } set { record = value; recordText.text = "RECORD: " + record; } }


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
        Record = 0;
        PlayerPrefs.SetInt("PPRecord", Record);
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
                panelMenu.SetActive(true);
                panelGameOver.SetActive(false);
            }
        }
        if (Input.GetButtonDown("Cancel"))
        {
            panelMenu.SetActive(true);
            panelPlay.SetActive(false);
            PlayerPrefs.GetInt("PPRecord", Record);
        }
    }

    IEnumerator BackToMenu()
    {
        yield return new WaitForSeconds(5);
        panelMenu.SetActive(true);
        panelLevelCompleted.SetActive(false);
        PlayerPrefs.GetInt("PPRecord", Record);
    }

}
