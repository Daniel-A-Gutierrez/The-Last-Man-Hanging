using UnityEngine;
using System.Collections;

public class roperatio : MonoBehaviour
{

    public GameObject player;
    public Vector3 grabPos;
    public float ratio;

    // Use this for initialization
    void Start()
    {
        for (int i = 1; i < 5; i++)
        {
            player = GameObject.Find("Player" + i);
            if (player != null)
            {
                break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

        float scaleX = Vector3.Distance(player.transform.position, grabPos) / ratio;
        GetComponent<LineRenderer>().material.mainTextureScale = new Vector2(scaleX, 1f);
    }
}