using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxController : MonoBehaviour
{

    private Transform m_transform;
    private Material m_material;
    private int color_index;
    private int frane_count;
    private Color[] colors = new Color[]{Color.blue, Color.red, Color.green};
    // Start is called before the first frame update
    void Start()
    {
        m_transform = this.transform;
        m_material = this.GetComponent<MeshRenderer>().material;
        color_index = 0;
        frane_count = 0;
    }

    // Update is called once per frame
    void Update()
    {
        m_transform.Rotate(1, 1.5f, 1);
        if (frane_count > 24) {
            //color_index++;
            frane_count = 0;
        }
        m_material.color = colors[color_index%colors.Length];
        frane_count++;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            color_index++;
        }
    }
    
    public void ChangeColor() {
        color_index++;
        m_material.color = colors[color_index%colors.Length];
    }
}
