#pragma strict
public var yourCursor : Texture2D;
public var cursorSizeX : int = 50;
public var cursorSizeY : int = 50;

function Start () {
	Cursor.visible = false;
}

function Update () {

}

function OnGUI(){
	GUI.DrawTexture(Rect(Event.current.mousePosition.x, Event.current.mousePosition.y, cursorSizeX, cursorSizeY), yourCursor);
}