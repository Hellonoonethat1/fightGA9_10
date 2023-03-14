using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Button_controller_script : MonoBehaviour
{
    public GameObject panel;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void onOptionClick()
    {

        if (panel != null)
        {
            panel.SetActive(!panel.activeSelf);
        }



    }
    public void onReturnClick()
    {
        SceneManager.LoadScene("LevelSelect"); 
    }

    public void Loadthing()

    {
        SceneManager.LoadScene("1");      
    }
    public void Loadscene(int number)

    {
        SceneManager.LoadScene(number);
    }

    public void onQuit()
    {
      Application.Quit();
    }
}
