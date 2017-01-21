#pragma strict

var controller:GameObject;



//This script can be used to manually stop the MicController and correctly restart it. We do this
//by simply controlling the state of MicControl's focused value (true or false).

//Keep in mind that MicControll will always override these changes when switching windows (clicking in the editor or alt tabbing out of your build).
//So if you need this to be consistent between switches (alt tabs), make sure you have a script whatching the window state, ready to overwrite this value again.

//Why does micControll force this behavior on window switches? Well, that is to prevent memory leaks. MicControll force stops/pauses the controller and microphone when
//the game window is not in focus, thus auto triggering the focused variable to true, when the game window becomes active again.




//Press space ingame to manually controll the micControllers state "stop(focused=false) or rec (focused=true".
function Update () {

var getCtrl=controller.transform.GetComponent.<MicControl>();

if(Input.GetKeyDown("space")&&getCtrl.focused==true){

getCtrl.focused=false;


	}

	else{

	if(Input.GetKeyDown("space")&&getCtrl.focused==false){

			getCtrl.focused=true;


				}

			}


}