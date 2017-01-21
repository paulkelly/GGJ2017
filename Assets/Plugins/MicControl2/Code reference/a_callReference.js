#pragma strict

//in the inspector this is where you will drag the MicController of which you wish to receive loudness from
//if you have multiple scenes, be sure to set do not destroy on load for each controller.
//And then use a GameObject search as shown below to dynamically detect the controller in new upcoming scenes (scripts).
var micController:GameObject;


//you only need the start search if you have more than 1 scene. If you have only 1 scene it is better to simply drag the controlelr inside the inspector slot.
function Start(){

if(micController==null){

micController = GameObject.Find("MicController");//this name can by any name given to your controller, if you have multiple controllers be sure to type the correct name here.

}

}


function Update () {

//Calls the loudness value of selected controller (in this case the controller in micController variable).
var getLoudness=micController.GetComponent(MicControl).loudness;
}