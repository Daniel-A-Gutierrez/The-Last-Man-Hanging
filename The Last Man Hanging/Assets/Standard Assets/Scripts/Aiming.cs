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
        if (Input.GetAxis("J1_Horizontal") != 0f)
        {
            h = Input.GetAxis("J1_Horizontal");
        }
        if (Input.GetAxis("J1_Vertical") != 0f)
        {
            v = Input.GetAxis("J1_Vertical");
        }
        
        this.transform.eulerAngles = new Vector3(this.transform.eulerAngles.x, this.transform.eulerAngles.y, Mathf.Atan2(-h, -v) * Mathf.Rad2Deg);

        if (Input.GetKeyDown("joystick 1 button 0"))
        {
            print("Horizontal:" + h);
            print("Vertical: " + v);
        }
    }
	public Vector2 getAimVector(){
		return new Vector2 (h, -v);
	}
}