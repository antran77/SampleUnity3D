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

    void Start()
    {
        count = 0;        
        SetCountText();
        winText.text = "";
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
            if (count >= 11) {
                winText.text = "You win!";
            }
        }
    }

    private void SetCountText() {
        countText.text = "Catched: " + count.ToString();
    }
}
