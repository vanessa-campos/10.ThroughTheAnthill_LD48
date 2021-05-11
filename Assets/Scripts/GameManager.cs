using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    PlayerController player;

    public int currentScene;
    public int totalPoints;
    // public bool joystick;


    public void PlayClicked()
    {
        LoadScene(1, 1);
        PlayerPrefs.SetInt("Points", 0);
    }

    public void QuitClicked()
    {
        Application.Quit();
    }

    // public void JoystickOn()
    // {
    //     if (joystick)
    //     {
    //         joystick = false;
    //     }
    //     else
    //     {
    //         joystick = true;
    //     }
    // }
    private void Start()
    {
        player = FindObjectOfType<PlayerController>();
        currentScene = SceneManager.GetActiveScene().buildIndex;
    }


    IEnumerator SceneDelay(int SceneNumber, float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        SceneManager.LoadScene(SceneNumber);
    }
    public void LoadScene(int SceneNumber, float delay = 0)
    {
        StartCoroutine(SceneDelay(SceneNumber, delay));
    }


}
