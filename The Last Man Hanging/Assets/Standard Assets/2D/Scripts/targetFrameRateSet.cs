using UnityEngine;
using System.Collections;

public class targetFrameRateSet : MonoBehaviour {

    // Use this for initialization
    public int targetFrameRate;
	void Start () {
	
	}
	void Awake()
    {
        Application.targetFrameRate = targetFrameRate;
    }
	// Update is called once per frame
	void Update () {
        print(1 / Time.deltaTime);
	}
}
