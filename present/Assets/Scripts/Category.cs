using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Category : MonoBehaviour {

    public Transform scroll;

    public RawImage color;
    public GameObject holder;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    
    public void AddCard(Transform card)
    {
        card.SetParent(this.transform);
        card.gameObject.SetActive(false);
    }

    public void Open()
    {
        foreach (Transform c in holder.transform)
        {
            c.SetParent(scroll);
            c.gameObject.SetActive(true);
        }
    }
}
