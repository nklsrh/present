using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MoodColorScript : MonoBehaviour {

    public TextMeshProUGUI txt;
    
    
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetColor(string m, int text)
    {
        if (m == "A")
        {
            GetComponent<RawImage>().color = Color.green;  
        }

        if (m == "B")
        {
            GetComponent<RawImage>().color = Color.blue;
        }

        if (m == "C")
        {
            GetComponent<RawImage>().color = Color.magenta;
        }

        txt.text = text.ToString();
    }
}
