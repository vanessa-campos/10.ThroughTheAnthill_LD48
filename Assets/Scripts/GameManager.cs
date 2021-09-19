using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] int LvlsWon;
    [SerializeField] GameObject panelLvls;
    [SerializeField] GameObject[] buttonsLvls;

    private void Start(){   
        panelLvls.SetActive(false);
        LvlsWon = PlayerPrefs.GetInt("PPLvlsWon");
        for (int i = 0; i < buttonsLvls.Length; i++) {
            if(PlayerPrefs.GetInt("PPLvlsWon") <= i){
                buttonsLvls[i].GetComponent<Button>().interactable = false;
            }
        }
        buttonsLvls[0].GetComponent<Button>().interactable = true;
    }

    public void PlayClicked(){
        PlayerPrefs.SetInt("Points", 0);
        panelLvls.SetActive(true);
    }

    public void QuitClicked(){
        Application.Quit();
    }

    public void LoadLvl(int Lvl){       
        SceneManager.LoadScene(Lvl);            
    }
}
