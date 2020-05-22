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
    public GameObject m_kachujin;
    
    public GameObject myPrefab;
    public GameObject myPrefabFire;
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

    Quaternion kachujinOriginalRotation;

    public GameObject PS_Explosion;
    public GameObject PS_Respawn;

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
        
        kachujinOriginalRotation = m_kachujin.transform.rotation;
        //m_kachujin.transform.parent.Rotate(0,90,0);
    }

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
                    int color = (int)Random.Range(0,3);
                    pickup.GetComponent<Animator>().SetInteger("ColorChoice", color);
                    pickup.GetComponent<Animator>().SetBool("isActive", true);
                    pickup.SetActive(true);
                    if(color ==1 )//red
                    {
                        GameObject fire = myPrefabFire.Spawn(moveToPostion, parent);
                        fire.gameObject.name = "PS_Fire_" + pickup.name;
                        pickup.gameObject.tag = "PickupFire";
                    }
                   
                }
          
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
                m_Animator.Play("Walking");
                Transform kachujin = m_kachujin.transform;
                Vector3 relativePos = spawnPosArray[currentMove] - kachujin.position;
                Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
                m_kachujin.transform.rotation = rotation;
                Debug.Log("kachujin rot:" + m_kachujin.transform.rotation.eulerAngles);
            }
            
        }
        
        GameObject obj = GameObject.FindGameObjectWithTag("PickupExit");
        if(obj != null)
        {
            ResetPickup(obj);
        }
    }

    private void ResetPickup(GameObject obj)
    {
        if(obj.gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            obj.gameObject.GetComponent<BoxCollider>().isTrigger = true;
            obj.gameObject.tag = "Pickup";
            obj.gameObject.GetComponent<Animator>().SetBool("isActive", true);
            obj.gameObject.Kill();
            //Debug.Log("Reset Pickups");
        }
    }

    void LateUpdate()
    {
        this.transform.localRotation = Quaternion.Euler(new Vector3(0,0,0));
    }
   

    private void OnTriggerEnter(Collider other) {
        bool isReachPickup = other.gameObject.CompareTag("Pickup");
        bool isReachPickupFire = other.gameObject.CompareTag("PickupFire");
        if (isReachPickup || isReachPickupFire) {
            count++;
            SetCountText();
            currentMove++;
            if(isReachPickupFire){
            GameObject ps = GameObject.Find("PS_Fire_"+other.gameObject.name);
                if(ps!= null){
                    ps.SetActive(false);
                }
                PS_Explosion.SetActive(true);
                PS_Explosion.transform.position = other.transform.position;
                PS_Explosion.GetComponent<ParticleSystem>().Play();
            }
            other.gameObject.GetComponent<BoxCollider>().isTrigger = false;
            other.gameObject.GetComponent<Animator>().Rebind();
            other.gameObject.GetComponent<Animator>().SetBool("isActive", false);
            other.gameObject.GetComponent<Animator>().Play("FadeOut");
            other.gameObject.tag = "PickupExit";
            
            if (currentMove >= currentSpawn) {
                m_Animator.Rebind();
                m_Animator.SetBool("isIdle", true);
                m_Animator.Play("Idle");
                float currentRotY = m_kachujin.transform.rotation.eulerAngles.y;
                m_kachujin.transform.localRotation = Quaternion.Euler(0, currentRotY, 0);
            }
            else
            {
                Vector3 relativePos = spawnPosArray[currentMove] - m_kachujin.transform.position;
                Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
                m_kachujin.transform.rotation = rotation;
                Debug.Log("kachujin rot:" + m_kachujin.transform.rotation.eulerAngles);
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
