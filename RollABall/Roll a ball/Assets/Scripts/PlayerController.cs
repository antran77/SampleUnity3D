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

    void Start()
    {
        count = 0;        
        SetCountText();
        winText.text = "";
        
        //numberOfPickup = GameObject.FindGameObjectsWithTag("Pickup").Length;

        moveToPostion = this.transform.position;
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
                Instantiate(myPrefab, moveToPostion, Quaternion.identity, parent);
            }
        }

        if(this.transform.position != moveToPostion)
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, moveToPostion, speed * Time.deltaTime );
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
            other.gameObject.SetActive(false);
            count++;
            SetCountText();
            // if (count >= numberOfPickup) {
            //     winText.text = "You win!";
            // }
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
