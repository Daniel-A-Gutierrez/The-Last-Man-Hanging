using UnityEngine;
using System.Collections;

public class CountdownStart : MonoBehaviour {

    // Use this for initialization
    float cameraSpeed;
    float timeStart;
    bool poop = false;
    GameObject theCanvas;
	void Start ()
    {
        theCanvas = GameObject.Find("Canvas");
        timeStart = Time.time;
        PauseEverything();
	}
	
    void PauseEverything()
    {
        cameraSpeed = GameObject.FindGameObjectWithTag("MainCamera").gameObject.GetComponent<CameraScroll>().speed;
        GameObject.FindGameObjectWithTag("MainCamera").gameObject.GetComponent<CameraScroll>().speed = 0;
        GameObject[] gos = GameObject.FindGameObjectsWithTag("Player");
        foreach(GameObject go in gos)
        {
            go.GetComponent<Platformer2DUserControl>().noInput();
        }
        //actually do this in start everything. //transform.Find("MainCamera").gameObject.GetComponent<StartSong>().Play();
    }
    void StartEverything()
    {
        foreach(GameObject go in GameObject.FindGameObjectsWithTag("UI"))
        {
            Destroy(go);
        }
        
        GameObject.FindGameObjectWithTag("MainCamera").gameObject.GetComponent<CameraScroll>().speed = cameraSpeed;
        
        GameObject[] gos = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject go in gos)
        {
            go.GetComponent<Platformer2DUserControl>().startInput();
        }
        GameObject.FindGameObjectWithTag("MainCamera").gameObject.GetComponent<StartSong>().Play();
    }
	// Update is called once per frame
	void Update ()
    {
	    if(Time.time - timeStart > 3 &!poop)
        {
            StartEverything();
            poop = true;
        }
        else if (Time.time - timeStart > 2 & !poop)
        {
            theCanvas.GetComponent<CountdownManager>().SetText("<b>1</b>");
        }
        else if (Time.time - timeStart > 1 & !poop)
        {
            theCanvas.GetComponent<CountdownManager>().SetText("<b>2</b>");
        }

    }
}
