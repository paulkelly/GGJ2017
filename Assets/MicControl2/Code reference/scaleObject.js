#pragma strict

var micController:GameObject;
var amp:float=1;

function Start(){

if(micController==null){

micController = GameObject.Find("MicController");

}

}


function Update () {

//Calls the loudness value of selected controller (in this case the controller in micController variable).
var getLoudness=micController.GetComponent(MicControl).loudness;

//scales the gameObject heigt based on input stream gathered from MicControl.loudness
transform.localScale=Vector3(1+getLoudness*amp,1+getLoudness*amp,1+getLoudness*amp);
}