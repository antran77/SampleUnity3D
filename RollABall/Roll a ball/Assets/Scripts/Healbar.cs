using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Healbar : MonoBehaviour
{
    public Image heal;
    public int fullBlood = 100;
    // Start is called before the first frame update
    
    public void SetBlood(int val) {
        float v = val;
        heal.fillAmount = v / fullBlood;
    }
}
