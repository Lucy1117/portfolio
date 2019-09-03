using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class titleMenu : MonoBehaviour
{
    private bool menuCheck;
    private bool menuAppear;
    public GUISkin start;
    public GUISkin exit;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Jump"))
        {
            menuCheck = true;
        }
        if(menuCheck)
        {
            StartCoroutine("WaitMenu");
            menuCheck = false;
        }
    }
    private IEnumerator WaitMenu()
    {
        yield return new WaitForSeconds(4.5f);
        menuAppear = true;
    }
    private void OnGUI()
    {
        int sw = Screen.width;
        int sh = Screen.height;

        if (menuAppear)
        {
            GUI.skin = start;
            if (GUI.Button(new Rect(sw / 3 + 50, sh * 2 / 3 - 10, 240, 110), "button") == true)
            {
                SceneManager.LoadScene("Main");
            }

            GUI.skin = exit;
            if (GUI.Button(new Rect(sw / 3 + 50, sh * 2 / 3 + 90, 240, 110), "button") == true)
            {
                Application.Quit();
            }
        }
    }
}
