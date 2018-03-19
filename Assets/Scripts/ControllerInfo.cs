public class ControllerInfo
{
	//true if controller, false if keyboard
	public bool isController;
	//if controller, should be left stick, keyboard is WASD or arrow keys
	public string horAxis = "";
	public string verAxis = "";
	//should only be populated if using a controller
	public string horAxis2 = "";
	public string verAxis2 = "";
	public string fireAxis = "";
	public string switchColor = "";
	public int mouseFireButton = 0;
	public int mouseDefendButton = 1;


	//default attributes to reset randomization
	public float horizontalInvert = 1.0f;
	public float verticalInvert = 1.0f;
	public float horizontal2Invert = 1.0f;
	public float vertical2Invert = 1.0f;
}
