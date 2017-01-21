#pragma strict

var txt:String="Blow in your Microphone.";

function OnGUI () {
GUILayout.Label ("");
GUILayout.Label ("");
GUILayout.Label (txt);


}

function Update(){

if( Input.GetKeyDown( KeyCode.Escape )){
Application.Quit();
}

}