using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mousecursor : MonoBehaviour
{
    public Texture2D yourCursor;
    public int cursorSizeX = 50;
    public int cursorSizeY = 50;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;   
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnGUI()
    {
        GUI.DrawTexture(new Rect(Event.current.mousePosition.x, Event.current.mousePosition.y, cursorSizeX, cursorSizeY), yourCursor);
    }
}
