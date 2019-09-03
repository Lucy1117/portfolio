using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class gameoverbgmoper : MonoBehaviour
{
    private GameObject deathBGMA;
    private GameObject deathBGMB;
    private GameObject deathBGMC;

    public bool overCheckA;
    public bool overCheckB;
    public bool overCheckC;

    public bool gameoverCheck;
    private bool playCheck;

    //GUI에 필요한 변수
    private bool menuappear;
    public GUISkin again;
    public GUISkin d_exit;


    // Start is called before the first frame update
    void Start()
    {
        deathBGMA = GameObject.FindGameObjectWithTag("Backgroundmusic1");
        deathBGMB = GameObject.FindGameObjectWithTag("Backgroundmusic2");
        deathBGMC = GameObject.FindGameObjectWithTag("Backgroundmusic3");

        playCheck = true;
    }

    // Update is called once per frame
    void Update()
    {
        overCheckA = deathBGMA.GetComponent<BgmOper>().deathCheck;
        overCheckB = deathBGMB.GetComponent<BgmOper>().deathCheck;
        overCheckC = deathBGMC.GetComponent<BgmOper>().deathCheck;

        if (overCheckA || overCheckB || overCheckC)
        {
            gameoverCheck = true;
        }

        if (playCheck)
        {
            DeathMusic();
        }
    }

    void DeathMusic()
    {
        if (gameoverCheck)
        {
            GetComponent<AudioSource> ().Play();
            StartCoroutine("WaitMenu");
            playCheck = false;
        }
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

        if (menuappear)
        {
            GUI.skin = again;
            if (GUI.Button(new Rect(sw / 3 + 50, sh * 2 / 3 - 10, 100, 140), "button") == true)
            {
                SceneManager.LoadScene("Main");
            }

            GUI.skin = d_exit;
            if (GUI.Button(new Rect(sw / 3 + 180, sh * 2 / 3 - 10, 100, 140), "button") == true)
            {
                Application.Quit();
            }
        }
    }

}
