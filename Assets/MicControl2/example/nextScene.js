#pragma strict
import UnityEngine.SceneManagement;


var lvl:String;
var wait:int=6;

function Start () {

yield WaitForSeconds(wait);

//Application.LoadLevel(lvl);
SceneManager.LoadScene(lvl);
}

