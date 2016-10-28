using UnityEngine;
using System.Collections;

public class Aiming : MonoBehaviour
{


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxis("J_Horizontal");
        float v = Input.GetAxis("J_Vertical");
        this.transform.eulerAngles = new Vector3(this.transform.eulerAngles.x, this.transform.eulerAngles.y, Mathf.Atan2(-h, -v) * Mathf.Rad2Deg);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            print("Horazontal:" + h);
            print("Vertical: " + v);
        }
    }
}