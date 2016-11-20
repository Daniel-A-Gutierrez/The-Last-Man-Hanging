using UnityEngine;
using System.Collections;

public class Parralax : MonoBehaviour {

    // Use this for initialization
    GameObject[] layers;
    float[] sets;
    float[] speeds;
    float[] initialx;
    float[] widths;
     public    float width0;
    float width1;
    float width2;
    public float width3;
    float width4;
    float width5;

    public float speed0;
    public float speed1;
    public float speed2;
    public float speed3;
    public float speed4;
    public float speed5;

    float set0;
    float set1;
    float set2;
    float set3;
    float set4;
    float set5;


    int I;
    void Start()
    {
        int childCount = transform.childCount;
        Vector3 mc = GameObject.FindWithTag("MainCamera").transform.position;
        transform.position = new Vector3(mc.x, mc.y, transform.position.z);
        widths = new float[]{ width0, width1, width2,width3,width4,width5};
        speeds = new float[] { speed0, speed1, speed2, speed3, speed4, speed5 };
        layers = new GameObject[childCount];
        initialx = new float[childCount];
        sets = sets = new float[childCount]; 
        I = 0;
        layers = GameObject.FindGameObjectsWithTag("BackgroundLayer");
        foreach (GameObject go in layers)
        {
            
                
                initialx[I] = transform.GetChild(I).localPosition.x;
                I++;
            
            
        }
        I = 0;
        foreach (GameObject go in layers)
        {
            print(go.GetComponent<BoxCollider2D>().size.x);

            widths[I] =  go.transform.position.x;
            sets[I] = widths[I];
            I++;
        }
    }
	
	// Update is called once per frame
	void Update ()
    {

        GameObject mc = GameObject.FindGameObjectWithTag("MainCamera");
        transform.position = new Vector3(mc.transform.position.x, mc.transform.position.y, transform.position.z);
        for (int i = 0; i < I; i++)
        {
            
            layers[i].transform.localPosition = new Vector2(layers[i].transform.localPosition.x -
                speeds[i] * mc.GetComponent<CameraScroll>().speed * Time.deltaTime, layers[i].transform.localPosition.y);
            if(initialx[i] - layers[i].transform.localPosition.x  >= sets[i])
            {
                print("asdfasdfasdf");
                layers[i].transform.localPosition = new Vector2(initialx[i],0);
            }
        }

    }
}
