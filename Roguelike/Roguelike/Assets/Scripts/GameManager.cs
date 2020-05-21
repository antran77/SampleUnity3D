using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    public BoardManager boardScript;
    public int level = 3;
    [HideInInspector] public bool playersTurn = true;
    public int playerFoodPoints = 100;
    public float turnDelay = 0.1f;	
    private List<Enemy> enemies;
    private bool enemiesMoving;		

    private void Awake() {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
        enemies = new List<Enemy>();
        boardScript = GetComponent<BoardManager>();
        InitGame();
    }

    void InitGame()
    {
        enemies.Clear();
        boardScript.SetupScene(level);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public void GameOver()
    {
        enabled = false;
    }
    public void AddEnemyToList(Enemy script)
    {
        enemies.Add(script);
    }
    IEnumerator MoveEnemies()
    {
        enemiesMoving = true;
        
        yield return new WaitForSeconds(turnDelay);
        
        if (enemies.Count == 0) 
        {
            yield return new WaitForSeconds(turnDelay);
        }
        
        for (int i = 0; i < enemies.Count; i++)
        {
            enemies[i].MoveEnemy ();
            yield return new WaitForSeconds(enemies[i].moveTime);
        }
        playersTurn = true;
        
        enemiesMoving = false;
    }
}
