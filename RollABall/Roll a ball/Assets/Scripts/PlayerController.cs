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
    private bool run;

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
        //SimplePool.Preload(myPrefab);
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
                pickup.GetComponent<MeshRenderer>().material.color = new Color(Random.value, Random.value, Random.value);
          //      GameObject pickup = SimplePool.Spawn(myPrefab, moveToPostion, Quaternion.identity);
                //GameObject pickup = Instantiate(myPrefab, moveToPostion, Quaternion.identity, parent);
          //      pickup.GetComponent<MeshRenderer>().material.color = new Color(Random.value, Random.value, Random.value);
            }
        }

        if(Input.GetKey("down")){
            run = true;
        }

        if(currentSpawn > currentMove && run)
        {
            if (spawnPosArray[currentMove].y == 0) {
                currentMove++;
            } else {
                this.transform.position = Vector3.MoveTowards(this.transform.position, spawnPosArray[currentMove], speed * Time.deltaTime );
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
            if(other.transform.position == spawnPosArray[currentMove]){
                currentMove++;
            }
            else {
                for (int i = currentMove; i < currentSpawn; i++) {
                    if (spawnPosArray[i] == other.transform.position) {
                        spawnPosArray[i].y = 0;
                    }
                }
            }
            other.gameObject.Kill();
            // if (count >= numberOfPickup) {
            //     winText.text = "You win!";
            // }
            if (currentMove >= currentSpawn) {
                run = false;
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
