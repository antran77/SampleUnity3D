using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject menu_main, menu_igm, menu_option;
    public GameObject heal_bar;
    public Text timerText;
    private float secondsCount;
    private int minuteCount;
    private int hourCount;
    
    void Start() {
        OnMainMenu();
    }

    void Update() {
        if(GameManager.gameState == GameManager.GAMESTATE.Gameplay) {
            UpdateTimerUI();    
        }
        heal_bar.GetComponent<Healbar>().SetBlood(GameManager.characterBlood);
    }    
     
    public void UpdateTimerUI(){
        secondsCount += Time.deltaTime;
        if(hourCount > 0) 
            timerText.text = hourCount +":"+ minuteCount +":"+(int)secondsCount;
        else
            timerText.text = minuteCount +":"+(int)secondsCount;
        if(secondsCount >= 60){
            minuteCount++;
            secondsCount = 0;
        }else if(minuteCount >= 60){
            hourCount++;
            minuteCount = 0;
        }    
    }

    public void OnIGM() {
        GameManager.gameState = GameManager.GAMESTATE.IGM;
        menu_igm.SetActive(true);
        menu_main.SetActive(false);
        menu_option.SetActive(false);
    }

    public void OnMainMenu() {
        GameManager.gameState = GameManager.GAMESTATE.IGM;
        menu_igm.SetActive(false);
        menu_main.SetActive(true);
        menu_option.SetActive(false);
    }

    public void OnOption() {
        menu_igm.SetActive(false);
        menu_main.SetActive(false);
        menu_option.SetActive(true);
    }

    

    public void OnOptionBack(){
        if(GameManager.gameState == GameManager.GAMESTATE.IGM) {
            OnIGM();
        }
        else if(GameManager.gameState == GameManager.GAMESTATE.Mainmenu) {
            OnMainMenu();
        }
    }

    public void OnExit() {
        Application.Quit();
    }

    public void OnPlayGame() {
        menu_igm.SetActive(false);
        menu_main.SetActive(false);
        menu_option.SetActive(false);
        GameManager.gameState = GameManager.GAMESTATE.Gameplay;

    }

    public void OnPause() {
        OnIGM();
    }
}
