#pragma strict

//here we will store our frequencies. we also use this length to determine to loop size
var sptrmDtaCatch:float[];
//how much frequencies do we want to catch?
var controller:GameObject;



function Start () {

CatchSpectrumData();

}


function CatchSpectrumData(){



var spectrumData:float[] = controller.GetComponent(MicControl).spectrumData;

//we need to check if our MicController even has any values in the array before looping.
if(spectrumData.length>=0.1){

		//now we can loop through our spectrumData array and only fetch the values we want.
			 for(var i:int=0; i<=sptrmDtaCatch.length-1;i++){
 					sptrmDtaCatch[i]= spectrumData[i];

 					//we put out a debug to check if the list is complete
 					 Debug.Log( sptrmDtaCatch[i] );

 					}
  				}

  //we add a one second timer to update the list, so that we can visually see it is working in the inspector. this can be removed without a problem.
 	yield WaitForSeconds(1);
 		Start ();


}

