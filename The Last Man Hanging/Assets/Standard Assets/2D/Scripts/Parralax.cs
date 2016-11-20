using UnityEngine;
using System.Collections;

public class Parralax : MonoBehaviour {

    // Use this for initialization
    GameObject[] layers;
    float[] widths;
    float[] speeds;
    [SerializeField]
    public float width0;
    public float width1;
    public float width2;
    public float width3;
    public float width4;
    public float width5;
    public float speed0;
    public float speed1;
    public float speed2;
    public float speed3;
    public float speed4;
    public float speed5;
    int I;
    void Start()
    {
        transform.position = GameObject.FindWithTag("MainCamera").transform.position;
        Transform[] ts = gameObject.GetComponentsInChildren<Transform>();
        widths = new float[]{ width0, width1, width2,width3,width4,width5};
        speeds = new float[] { speed0, speed1, speed2, speed3, speed4, speed5 };
        layers = new GameObject[6];
        I = 0;
        foreach (Transform t in ts)
        {

            if (t != null && t.gameObject != null && t != gameObject)
            {
                layers[I] = t.gameObject;
                I++;
            }
            
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        GameObject mc = GameObject.FindGameObjectWithTag("MainCamera");
        transform.position = mc.transform.position;
        for(int i = 0; i < I; i++)
        {
            layers[i].transform.localPosition = new Vector2(layers[i].transform.localPosition.x -
                (speed0) * mc.GetComponent<CameraScroll>().speed, layers[i].transform.localPosition.y);
            if(layers[i].transform.localPosition.x <= -widths[i])
            {
                layers[i].transform.localPosition = new Vector2(0,0);
            }
        }

    }
}
