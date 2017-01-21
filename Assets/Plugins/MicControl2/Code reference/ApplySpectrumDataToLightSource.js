#pragma strict

var micController:GameObject;
var lightSrc:Light[];
var sensitivity:float=10;

function Update () {

var spctrm: float[] = micController.GetComponent(MicControl).spectrumData;

for (var i:int=0; i<=lightSrc.length-1 ; i++){

//Use this in your script to call loudnes data from selected controller 
 if(spctrm.Length-1>=0){
 lightSrc[i].intensity = 0.1+spctrm[i]*sensitivity;

 }
}


}