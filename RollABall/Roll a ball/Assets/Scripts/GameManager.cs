using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    GameObject character;
    public static Vector3 characterDiePos;
    
    public static int characterBlood = 100;
    public static GAMESTATE gameState;
    public enum GAMESTATE {
        Mainmenu,
        IGM, 
        Option,
        Gameplay
    }
    // Start is called before the first frame update
    void Start()
    {
        character = GameObject.Find("kachujin");
        gameState = GAMESTATE.Mainmenu;
    }

    // Update is called once per frame
    void Update()
    {
        GameObject obj = GameObject.Find("Respawn");
        if(obj != null)
        {
            Debug.Log(obj.GetComponent<SpawnEffect>().isFinish());
            if(obj.GetComponent<SpawnEffect>().isFinish()){
                Destroy(obj);
                GameObject parent = GameObject.Find("PlayerBound");
                parent.transform.position = new Vector3(0,0,0);
                character.SetActive(true);
            }
        }
    }

}
