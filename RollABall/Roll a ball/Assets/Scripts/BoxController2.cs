using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxController2 : MonoBehaviour
{
    public GameObject cube1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            cube1 = GameObject.Find("Cube1");
            cube1.GetComponent<BoxController>().ChangeColor();
        }
    }
}
