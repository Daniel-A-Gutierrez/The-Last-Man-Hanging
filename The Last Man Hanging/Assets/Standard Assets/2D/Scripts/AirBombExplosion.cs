using UnityEngine;
using System.Collections;

public class AirBombExplosion : MonoBehaviour {

    public float explosionPower = 500;

	// Use this for initialization
	void Start () {
        Destroy(this.gameObject, .3f);
    }
	
	// Update is called once per frame
	void Update () {
	    
	}
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Finish") //Change this to a finally player tag, however we eventually tag players indivudally or in one group
        {
            Vector2 direction = other.gameObject.transform.position - this.transform.position;
            float radius = Mathf.Pow((Mathf.Pow(direction.x, 2) + Mathf.Pow(direction.y, 2)), (1 / 2));
            float explosiveness = explosionPower * 1 / Mathf.Pow(radius, 2f);
            print(direction);
            other.gameObject.GetComponent<Rigidbody2D>().AddForce(explosiveness * direction, ForceMode2D.Impulse);

        }
    }

}
