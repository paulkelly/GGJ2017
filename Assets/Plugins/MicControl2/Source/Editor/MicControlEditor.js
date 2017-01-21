﻿	#pragma strict



	@CustomEditor (MicControl)

	class MicControlEditor extends Editor  {


			
			
	/////////////////////////////////////////////////////////////////////////////////////////////////		
			function OnInspectorGUI() {


	var ListenToMic = Selection.activeGameObject;
	var micInputValue=ListenToMic.GetComponent(MicControl).loudness;

	ProgressBar (micInputValue, "Loudness: "+ListenToMic.GetComponent(MicControl).loudness,18,18);
	
	//this button copy's the basic code to call the value, for quick acces.
	//use horizontal mapping incase we add more menu buttons later.
	EditorGUILayout.BeginHorizontal();
	
	//copy complete example code to clipboard
	 if(GUILayout.Button("Copy Loudness setup", GUILayout.Width(140) ) ){
	 		//Refresh the string each time the button is pressed, so the memory gest replaced on the clipboard
	 			var CopyCache:String="//Place here the 'controller' you want to call loudness from in the inspector \n var micController:GameObject;  \n \n function Update(){ \n//Use this in your script to call loudnes data from selected controller \n var ldness: float = micController.GetComponent(MicControl).loudness; \n }";

	 		
	 				//clear memory first
	 					EditorGUIUtility.systemCopyBuffer = null;
	 						//Then add the CopyCache string to fill up the memory with the desired example code
          						EditorGUIUtility.systemCopyBuffer = CopyCache;
		       
		         }
		         
		         //copy only the call string to the clipboard
		         	 if(GUILayout.Button("Copy SpectrumData setup", GUILayout.Width(180) ) ){
	 					//Refresh the string each time the button is pressed, so the memory gest replaced on the clipboard
	 						var CopyCache2:String="//Place here the 'controller' you want to call the spectrum data from in the inspector \n var micController:GameObject;  \n \n function Update(){ \n//Use this in your script to call loudnes data from selected controller \n var ldness: float[] = micController.GetComponent(MicControl).spectrumData; \n }";

	 		
	 							//clear memory first
	 								EditorGUIUtility.systemCopyBuffer = null;
	 									//Then add the CopyCache string to fill up the memory with the desired example code
          									EditorGUIUtility.systemCopyBuffer = CopyCache2;
		       
		      			   }
		      			   
		      			   //help button redirects to website
		         	 if(GUILayout.Button("help") ){
	 					Application.OpenURL ("http://markduisters.com/asset-portfolio/");       
		      			   }
        
        EditorGUILayout.EndHorizontal();

       //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//show the selected device and create a device selection drop down.


   //this visually shows if the micrpohone is active or not
        	EditorGUILayout.BeginHorizontal();

        	try{
        	//if the application is focused (ingame) show a green circ
        	if(ListenToMic.GetComponent(MicControl).focused){
        	GUI.color = Color.green;
				GUILayout.Box("        ");
				GUI.color = Color.white;        	
					}
        				else{
        				GUI.color = Color.red;
        					GUILayout.Box("        ");
        					GUI.color = Color.white;
        						}

        						//show selected device
        			GUILayout.Label(Microphone.devices[ListenToMic.GetComponent(MicControl).InputDevice]);
        			}
        			catch (e){}

        EditorGUILayout.EndHorizontal();

	//Redirect ShowDeviceName ingame
	//count devices
	var count:int=0;
		for(device in Microphone.devices){
				count++;
			}

			//toggle if a default mic should be used
    ListenToMic.GetComponent(MicControl).useDefaultMic=GUILayout.Toggle(ListenToMic.GetComponent(MicControl).useDefaultMic,GUIContent ("Use default device microphone","When enabled the controller will always grab the default microphone of the device where the application is run from.(mobile mic, pc mic that is currently set as default,..."), GUILayout.Width(200));

    if( !ListenToMic.GetComponent(MicControl).useDefaultMic){

    // instead of using the buttons or default setting to select. Users can manually input a slot number (for android, etc..)
    ListenToMic.GetComponent(MicControl).SetDeviceSlot=GUILayout.Toggle(ListenToMic.GetComponent(MicControl).SetDeviceSlot,GUIContent ("Set device slot","Handy for building to mobile platyforms that use another microphone other than the internal one. If you know the device location/number position, you can type it here. Keep in mind that although you can set not connected slots now.Your microphone input will not work in the editor. A good rule of thumb to follow is, to always develop in the editor with your workstation default microphone and only at build set the slot to what Android would be using. If your android device only uses the internal microphone, simply select use default device and you are good to go."), GUILayout.Width(200));
       if(!ListenToMic.GetComponent(MicControl).SetDeviceSlot){

	ListenToMic.GetComponent(MicControl).ShowDeviceName= EditorGUILayout.Foldout(ListenToMic.GetComponent(MicControl).ShowDeviceName,GUIContent ("Detected devices: "+count,"Show a list of all detected devices (1 is showed as default device in the drop down menu)"));
		 if(ListenToMic.GetComponent(MicControl).ShowDeviceName){

				if(Microphone.devices.Length>=0){
	 
						var i=0;
										//count amount of devices connected
											for(device in Microphone.devices){
											if(!device){
											Debug.LogError("No usable device detected! Try setting your device as the system's default. Or set a slot manually");
											return;
											}
														i++;

																	GUILayout.BeginVertical();

																		//if selected slot is not equal to number count, make button grey.
																		if(ListenToMic.GetComponent(MicControl).InputDevice != i-1){
																			GUI.color=Color.grey;
																				}

																		//create a selection button
																		if(GUILayout.Button(device)){
																			ListenToMic.GetComponent(MicControl).InputDevice = i-1;
																					}

																					GUI.color=Color.white;

																							GUILayout.EndVertical();
													}

												}

												//throw error when no device is found.
													else{
														Debug.LogError("No connected device detected! Connect at least one device.");	
														return;
															}
													}
												}

													else{
														ListenToMic.GetComponent(MicControl).InputDevice = EditorGUILayout.IntField("Slot number =", ListenToMic.GetComponent(MicControl).InputDevice);

														}

											}


			if(!ListenToMic.GetComponent(MicControl).focused){
	GUILayout.Label("");
	GUILayout.Label("The microphone will only send data when the game window is active!");
	GUILayout.Label("");
	}

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	//show advanced variables
	ListenToMic.GetComponent(MicControl).advanced=EditorGUILayout.Foldout(ListenToMic.GetComponent(MicControl).advanced,GUIContent ("Advanced settings","Reveal all tweakable variables"));

	if(ListenToMic.GetComponent(MicControl).advanced){

	GUILayout.Label("");

	EditorGUILayout.BeginHorizontal();
//	GUILayout.Label("");

		//Redirect debug ingame
		ListenToMic.GetComponent(MicControl).debug=GUILayout.Toggle(ListenToMic.GetComponent(MicControl).debug,GUIContent ("Debug","This will show the connection progress in the console."), GUILayout.Width(60));

		//keep this controller persistend between scenes
		ListenToMic.GetComponent(MicControl).doNotDestroyOnLoad=GUILayout.Toggle(ListenToMic.GetComponent(MicControl).doNotDestroyOnLoad,GUIContent ("Don't destroy on load","If selected, this controller will be persistend when switching scenes during runtime"), GUILayout.Width(150));

		//Redirect Mute ingame
		ListenToMic.GetComponent(MicControl).Mute=GUILayout.Toggle(ListenToMic.GetComponent(MicControl).Mute,GUIContent ("Mute","Leave enabled when you only need the input value of your device. When dissabled you can listen to the playback of the device"), GUILayout.Width(60));


	EditorGUILayout.EndHorizontal();

	//enable or disable spectrum data analisys
		ListenToMic.GetComponent(MicControl).enableSpectrumData=GUILayout.Toggle(ListenToMic.GetComponent(MicControl).enableSpectrumData,GUIContent ("enable spectrum data analysis","When enabled users will have acces to the full frequency spectrum output by MicControl2. This is the big brother of the 'loudness' variable, instead of having a single float to represent the microphone's loudness, a full float array is filled with the frequency spectrum data. "), GUILayout.Width(500));

	GUILayout.Label("");

		//ListenToMic.GetComponent(MicControl).maxFreq = EditorGUILayout.FloatField(GUIContent ("Frequency (Hz)","Set the quality of the received data: It is recommended but not required, to match this to your selected microphone's frequency."), ListenToMic.GetComponent(MicControl).maxFreq);
		ListenToMic.GetComponent(MicControl).freq = EditorGUILayout.EnumPopup(GUIContent ("Frequency (Hz)","Select the quality of the received data: It is recommended but not required, to match this to your selected microphone's frequency."), ListenToMic.GetComponent(MicControl).freq);


		ListenToMic.GetComponent(MicControl).bufferTime = EditorGUILayout.FloatField(GUIContent ("RAM buffer time","How many seconds of audio should be loaded into RAM. This will then be filled up with the Sample amount."), ListenToMic.GetComponent(MicControl).bufferTime);
		//always have at least 1 second of audio to fill the ram.
		if(ListenToMic.GetComponent(MicControl).bufferTime<1){
		ListenToMic.GetComponent(MicControl).bufferTime=1;
		}


		ListenToMic.GetComponent(MicControl).amountSamples = EditorGUILayout.IntSlider(GUIContent ("Sample amount","This is basically how much the buffer gets filled (per frame) with samples and determines the precission of the loudness variable, if you are not sure, leave this between 256 and 1024 as it gives more than enough precision for basic tasks/scripts. Higher samples = more precision/quality of the loudness value and smoother results. However for spectrumData this is different. As spectrumData gives you direct acces to this sample buffer. This means that the bigger the sample buffer, the more data you have acces to. "), ListenToMic.GetComponent(MicControl).amountSamples,256,8192);
		//lock to increments
			var tempSamples:int=ListenToMic.GetComponent(MicControl).amountSamples;
				if(tempSamples >= 256 && tempSamples <= 512){
					ListenToMic.GetComponent(MicControl).amountSamples=256;
						}

				if(tempSamples >= 512 && tempSamples <= 1024){
					ListenToMic.GetComponent(MicControl).amountSamples=512;
						}

				if(tempSamples >= 1024 && tempSamples <= 2048){
					ListenToMic.GetComponent(MicControl).amountSamples=1024;
						}

				if(tempSamples >= 2048 && tempSamples <= 4096){
					ListenToMic.GetComponent(MicControl).amountSamples=2048;
						}

				if(tempSamples >= 4096 && tempSamples <= 8192){
					ListenToMic.GetComponent(MicControl).amountSamples=4096;
						}

				if(tempSamples >= 8192){
					ListenToMic.GetComponent(MicControl).amountSamples=8192;
						}

				ListenToMic.GetComponent(MicControl).sensitivity = EditorGUILayout.Slider(GUIContent ("Sensitivity","Set the sensitivity of your input: The higher the number, the more sensitive (higher) the -loudness- value will be (1 = raw input)"), ListenToMic.GetComponent(MicControl).sensitivity,ListenToMic.GetComponent(MicControl).minMaxSensitivity.x, ListenToMic.GetComponent(MicControl).minMaxSensitivity.y);
				EditorGUILayout.MinMaxSlider(GUIContent ("Sensitivity range", "Helps you tweak the sensitivity"),ListenToMic.GetComponent(MicControl).minMaxSensitivity.x, ListenToMic.GetComponent(MicControl).minMaxSensitivity.y, 0, 1000);

					EditorGUILayout.BeginHorizontal();
					GUILayout.Label("");
				GUILayout.Label("min: "+ListenToMic.GetComponent(MicControl).minMaxSensitivity.x,GUILayout.Width(100));
				GUILayout.Label("max: "+ListenToMic.GetComponent(MicControl).minMaxSensitivity.y,GUILayout.Width(100));
				EditorGUILayout.EndHorizontal();


				//show loudness progress bars
				GUILayout.Label("Raw");
				ProgressBar (ListenToMic.GetComponent(MicControl).rawInput, "rawInput",18,7);
				GUILayout.Label("Loudness");
					ProgressBar (micInputValue, "loudness",18,7);



						//show spectrum data progress bars
					if(ListenToMic.GetComponent(MicControl).enableSpectrumData){

					var micSpectrum:float[] = ListenToMic.GetComponent(MicControl).spectrumData;
					ListenToMic.GetComponent(MicControl).spectrumDropdown=EditorGUILayout.Foldout(ListenToMic.GetComponent(MicControl).spectrumDropdown,GUIContent ("Spectrum data","Reveal the complete frequency spectrum"));

					if(ListenToMic.GetComponent(MicControl).spectrumDropdown){
					for (var s:int=0; s<=micSpectrum.Length-1;s++){

						EditorGUILayout.BeginHorizontal();
							GUILayout.Label("spectrumData["+s+"]");
								ProgressBar (micSpectrum[s], "spectrumData: "+s,18,7);
									EditorGUILayout.EndHorizontal();

										}
											}
												}



	}


	//EditorUtility.SetDirty(target);
	  this.Repaint();
			
		// Show default inspector property editor
	//	DrawDefaultInspector ();
		}

		
		
			// Custom GUILayout progress bar.
		function ProgressBar (value : float, label : String, scaleX : int, scaleY : int) {
			
			// Get a rect for the progress bar using the same margins as a textfield:
			var rect : Rect = GUILayoutUtility.GetRect (scaleX, scaleY, "TextField");
			EditorGUI.ProgressBar (rect, value, label);
			
			
			EditorGUILayout.Space ();
		}
		
	}
		
		
		
		
		
		
		
		
		
		
		
		
		
		
		
		
		