#pragma strict

var micController:GameObject;
var block:GameObject[];

function Update () {

var spctrm: float[] = micController.GetComponent(MicControl).spectrumData;

for (var i:int=0; i<=block.length-1 ; i++){

//Use this in your script to call loudnes data from selected controller 
 if(spctrm.Length-1>=0){
 block[i].transform.localScale.y = 1*spctrm[i];
 }
}


}