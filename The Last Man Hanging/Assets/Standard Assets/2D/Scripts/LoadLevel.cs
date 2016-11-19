using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LoadLevel : MonoBehaviour
{
    Platformer2DUserControl control;
    public void Load_Level(int level)
    {
        SceneManager.LoadScene(level);
    }
    void Update()
    {
        if (Input.GetKeyDown("joystick 1 button 8") || Input.GetKeyDown("joystick 2 button 8") || Input.GetKeyDown("joystick 3 button 8") || Input.GetKeyDown("joystick 3 button 8"))
        {
            Load_Level(1);
        }
    }
}

