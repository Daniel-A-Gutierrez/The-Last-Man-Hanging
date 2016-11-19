using UnityEngine;
using System.Collections;

public class AirBomb : MonoBehaviour {


	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Finish") //Change this to a finally player tag, however we eventually tag players indivudally or in one group
        {
            Instantiate(Resources.Load("Explosion"), transform.position, transform.rotation);
            Destroy(this.gameObject);
        }
    }

}
