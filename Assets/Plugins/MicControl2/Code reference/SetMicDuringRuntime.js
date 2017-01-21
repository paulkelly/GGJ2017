#pragma strict



//Place here the 'controller' of which you want to set a microphone during runtime.
 var micController:GameObject;  

 //here we use an int to set the microphone in unity.
 var yourDevice:int;
 //In here we will place the device reference so that we can visually see which device we are talking to.
 var selectedDevice: String;



 function SetCustomMicrophone(){ 

//this value is purely to check in the editor if we have the device we want
 selectedDevice= Microphone.devices[yourDevice];




//Below we will setup our own mic during runtime.

// First lets stop the current microphone connection in the controller. We do this by calling the controller's StopMicrophone function.
micController.GetComponent(MicControl).StopMicrophone();

//Then we simply set "yourDevice" in the controller's input device slot
micController.GetComponent(MicControl).InputDevice=yourDevice;


//Then lastly we need to initialise and start recording on this device. This is done by simply calling the controller's start function.
micController.GetComponent(MicControl).Start();


//That's it. You can create a simple GUI that changes the value of "yourDevice" on button press and you can even use selectedDevice to get that device name.
//The editor script uses a for loop, to loop through each and every device, gets their position and name, then creates a button for each one. The same can be done
//during a runtime GUI. But during runtime, you will always need to folow the above steps.
 }