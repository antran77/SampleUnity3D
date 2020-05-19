using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed;
    public Rigidbody m_rigibody;
    private int count;
    public Text countText;
    public Text winText;
    //private int numberOfPickup;
    
    public GameObject myPrefab;
    public Transform parent;
    public Camera camera;

    public float raylength;
    public LayerMask layerMask;
    public Vector3 moveToPostion;

    private int maxSpawn;
    private int currentMove;
    private int currentSpawn;
    private Vector3[] spawnPosArray;
    private bool run = true;

    Animator m_Animator;
    public bool isGround;

    public int ColorChoice = 0;
    int colorState = 0;

    void Start()
    {
        count = 0;   
        run = false;     
        SetCountText();
        winText.text = "";
        
        //numberOfPickup = GameObject.FindGameObjectsWithTag("Pickup").Length;

        moveToPostion = this.transform.position;

        maxSpawn = 9999;
        currentMove = 0;
        currentSpawn = 0;
        spawnPosArray = new Vector3[maxSpawn];
        m_Animator = gameObject.GetComponent<Animator>();
        colorState = 0;
    }

    // void OnJump()
    // {
    //     m_rigibody.AddForce(jump * jumpForce, ForceMode.Impulse);
    //     isGround = false;
    // }

    void OnCollisionStay()
    {
        isGround = true;
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray,out hit, raylength, layerMask))
            {
                moveToPostion = hit.point;
                moveToPostion.y = 0.5f;
                spawnPosArray[currentSpawn++] = moveToPostion;
                GameObject pickup = myPrefab.Spawn(moveToPostion, parent);
                if (pickup != null) {
                    //pickup.GetComponent<MeshRenderer>().material.color = new Color(Random.value, Random.value, Random.value);
                    pickup.GetComponent<Animator>().SetInteger("ColorChoice", (int)Random.Range(0,3));
                    pickup.SetActive(true);
                }
          //      GameObject pickup = SimplePool.Spawn(myPrefab, moveToPostion, Quaternion.identity);
                //GameObject pickup = Instantiate(myPrefab, moveToPostion, Quaternion.identity, parent);
          //      pickup.GetComponent<MeshRenderer>().material.color = new Color(Random.value, Random.value, Random.value);
            }
        }

        //if(Input.GetKey("down"))
        {
            run = true;
        }

        if(currentSpawn > currentMove && run)
        {      
            GameObject parent = GameObject.Find("PlayerBound");
            parent.transform.position = Vector3.MoveTowards(parent.transform.position, spawnPosArray[currentMove], speed * Time.deltaTime);

            if (m_Animator.GetBool("isIdle"))
            {
                m_Animator.Rebind();
                m_Animator.SetBool("isIdle", false);
                m_Animator.Play("RunPlayer");
            }
            
        }
    }

    void FixedUpdate() {
        float moveH = Input.GetAxis("Horizontal");
        float moveV = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(moveH, 0.0f, moveV);
        m_rigibody.AddForce(movement * speed);
        
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Pickup")) {
            //SimplePool.Despawn(other.gameObject);
            //other.gameObject.SetActive(false);
            
            count++;
            SetCountText();
            currentMove++;
            other.gameObject.Kill();
            // if (count >= numberOfPickup) {
            //     winText.text = "You win!";
            // }
            if (currentMove >= currentSpawn) {
                //run = false;
                m_Animator.Rebind();
                m_Animator.SetBool("isIdle", true);
                m_Animator.Play("IdlePlayer");
            }
        }
    }

    private void SetCountText() {
        countText.text = "Catched: " + count.ToString();
    }

    public void Reset() {
        count = 0;
        SetCountText();
        winText.text = "";
    }
}
