using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class endingmenu : MonoBehaviour
{
    private bool menuappear;
    public GUISkin again;
    public GUISkin d_exit;
    public Texture myTexture;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("WaitMenu");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator WaitMenu()
    {
        yield return new WaitForSeconds(2.0f);
        menuappear = true;
    }

    private void OnGUI()
    {
        int sw = Screen.width;
        int sh = Screen.height;

        if(menuappear)
        {
            GUI.DrawTexture(new Rect(sw / 3 - 20, sh / 10, 365, 318), myTexture);

            GUI.skin = again;
            if (GUI.Button(new Rect(sw / 3 + 30, sh * 2 / 3 - 10, 100, 140), "button") == true)
            {
                SceneManager.LoadScene("Main");
            }

            GUI.skin = d_exit;
            if (GUI.Button(new Rect(sw / 3 + 170, sh * 2 / 3 - 10, 100, 140), "button") == true)
            {
                Application.Quit();
            }
        }
    }
}
