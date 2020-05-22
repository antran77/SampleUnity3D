using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Player : MovingObject
{
    public int wallDamage = 1;
    public int pointsPerFood = 10;
    public int pointsPerSoda = 20;
    public float restartLevelDelay = 1f;
    private Animator animator;
    private int food;
    public Text foodText;	
    // Start is called before the first frame update
    protected override void Start()
    {
        animator = GetComponent<Animator>();
        food = GameManager.instance.playerFoodPoints;
        foodText.text = "Food: " + food;
        base.Start();
    }

    private void OnDisable() {
        GameManager.instance.playerFoodPoints = food;
    }
    // Update is called once per frame
    void Update()
    {
        if(!GameManager.instance.playersTurn) return;
			
        int horizontal = 0;  	//Used to store the horizontal move direction.
        int vertical = 0;		//Used to store the vertical move direction.

        
        horizontal = (int) (Input.GetAxisRaw ("Horizontal"));
        vertical = (int) (Input.GetAxisRaw ("Vertical"));
        
        if(horizontal != 0)
        {
            vertical = 0;
        }
        if(horizontal != 0 || vertical != 0)
        {
            AttemptMove<Wall> (horizontal, vertical);
        }
    }


    private void CheckIfGameOver(){
        if (food <= 0)
            GameManager.instance.GameOver();
    }
    protected override void AttemptMove <T> (int xDir, int yDir) 
    {
        Debug.Log("Attemp move");
        food--;
        foodText.text = "Food: " + food;
        base.AttemptMove<T>(xDir, yDir);
        RaycastHit2D hit;
        Move (xDir, yDir, out hit);
        CheckIfGameOver();
        GameManager.instance.playersTurn = false;
    }
    protected override void OnCantMove <T> (T component)
    {
        Wall hitWall = component as Wall;
        hitWall.DamageWall (wallDamage);
        animator.SetTrigger ("playerChop");
    }
    private void OnTriggerEnter2D (Collider2D other)
    {
        if(other.tag == "Exit")
        {
            Invoke ("Restart", restartLevelDelay);
            
            enabled = false;
        }
        
        else if(other.tag == "Food")
        {
            food += pointsPerFood;
            foodText.text = "+" + pointsPerFood + " Food: " + food;
            other.gameObject.SetActive (false);
        }
        
        else if(other.tag == "Soda")
        {
            food += pointsPerSoda;
            foodText.text = "+" + pointsPerSoda + " Food: " + food;
            other.gameObject.SetActive (false);
        }
    }

    private void Restart(){
        Application.LoadLevel(Application.loadedLevel);
    }
    public void LoseFood (int loss)
    {
        
        animator.SetTrigger ("playerHit");
        
        food -= loss;
        
        foodText.text = "-"+ loss + " Food: " + food;
        CheckIfGameOver ();
    }
}
