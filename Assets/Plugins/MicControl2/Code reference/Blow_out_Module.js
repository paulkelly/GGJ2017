#pragma strict

//here we place the controller to listen to (important if you have a multi mic setup).
var micController:GameObject;

//Only if the player blows above this value we will start checking our loop values.
var minBlowStrength:float;

//This module has all the objects, sounds and lights that need to be turned of if the player blows hard enough.
var InModules: InModule[];



function Start () {
//if not set auto search for the first controller we find
	if(!micController){
		micController=GameObject.Find("MicController");
			}

}



function Update () {

//Use this in your script to call loudnes data from selected controller 
 var ldness: float = micController.GetComponent(MicControl).loudness;

 //if above min start loop
 if(ldness >=minBlowStrength){

//loop through our objects that need to be turned off
 	for (var i:int=0; i<=InModules.Length-1;i++){

		//only turn off if blow strength is above the max, this way each InModules specified can have its own condition to be turned off.
 			if(ldness >= InModules[i].maxBlowStrength){

 			//disable these
 			if(InModules[i].disableObject){
				InModules[i].disableObject.SetActive(false);
				}


				//enable these
				if(InModules[i].enableObject){
				InModules[i].enableObject.SetActive(true);
				}


 				}

 					} 
 						}


}




//In these classes users can define which objects, ligts and audio clips will be turned on/off if the maxBlowStrength is passed by the loudness.


class InModule {

var maxBlowStrength:float;

var disableObject:GameObject;

var enableObject:GameObject;



}