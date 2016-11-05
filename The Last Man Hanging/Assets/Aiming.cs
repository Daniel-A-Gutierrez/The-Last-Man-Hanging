using UnityEngine;
using System.Collections;

public class Aiming : MonoBehaviour
{
    float h = 0f;
    float v = 0f;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("JR_Horizontal") != 0f)
        {
            h = Input.GetAxis("JR_Horizontal");
        }
        if (Input.GetAxis("JR_Vertical") != 0f)
        {
            v = Input.GetAxis("JR_Vertical");
        }
        
        this.transform.eulerAngles = new Vector3(this.transform.eulerAngles.x, this.transform.eulerAngles.y, Mathf.Atan2(-h, -v) * Mathf.Rad2Deg);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            print("Horizontal:" + h);
            print("Vertical: " + v);
        }
    }
}