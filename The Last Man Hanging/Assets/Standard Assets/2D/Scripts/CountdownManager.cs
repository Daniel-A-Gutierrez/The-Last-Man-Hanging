using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CountdownManager : MonoBehaviour
{
    Text counter;
    // Use this for initialization
    float startTime = Time.time;
	void Start ()
    {
        counter = transform.GetChild(0).gameObject.GetComponent<Text>();
	}
	
	// Update is called once per frame
    public void SetText(string newText)
    {
        counter.text = newText;
    }
	void Update ()
    {
	    
	}
}
