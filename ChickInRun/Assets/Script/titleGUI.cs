using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class titleGUI : MonoBehaviour
{
    public GUIStyle labelStyle;
    private float guiAlpha;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Jump"))
        {
            Destroy(gameObject);
        }
        LerpAlpha();
    }
    private void OnGUI()
    {
        GUI.color = new Color()
        {
            a = guiAlpha
        };
        int sw = Screen.width;
        int sh = Screen.height;
        GUI.Label(new Rect(0.0f, 2.0f * sh / 3, sw, sh / 4), "시작하려면 Space 키를 누르세요", labelStyle);
    }

    void LerpAlpha()
    {
        float lerpvar = Mathf.PingPong(Time.time, 1.0f) / 1.0f;
        guiAlpha = Mathf.Lerp(0.0f, 1.0f, lerpvar);
    }
}
