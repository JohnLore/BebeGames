using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour 
{
	//setup gui stuff
	public GUISkin guiSkin;
	public Texture2D background;
	public bool DragWindow = false;

	//setup menu stuff
	private bool onMenu;
	private string clicked;
	private Rect WindowRect = new Rect((Screen.width / 2) - 150, Screen.height / 2, 200, 225);

	//server/multiplayer stuff
	private string serverName = "DefaultServer";
	private int connectionPort = 8000;
	private string connectionIP = "0.0.0.0";
	private string playerName = "DefaultPlayer";

	
	private void Start()
	{
		//start on menu
		clicked = "";
		onMenu = true;
		Time.timeScale = 0;
	}
	
	private void OnGUI()
	{
		if (onMenu) {

			//setup menu
			if (background != null)
			{
				GUI.DrawTexture (new Rect (0, 0, Screen.width, Screen.height), background);
			}
			GUI.skin = guiSkin;

			//menu actions
			if (clicked == "") 
			{
				WindowRect = GUI.Window (0, WindowRect, menuFunc, "Menu");
			} 
			else if (clicked == "play")
			{
				Time.timeScale = 1;
				onMenu = false;
			}
			else if (clicked == "reset")
			{
				//where 0 is the index of the level -- for now, DevRoom
				Application.UnloadLevel(0);
				Application.LoadLevel(0);

				Time.timeScale = 1;
				onMenu = false;
			}
			else if (clicked == "multiplayer") 
			{
				WindowRect = GUI.Window (1, WindowRect, multiplayerFunc, "Multiplayer");
			}
			else if (clicked == "connectServer") 
			{
				WindowRect = GUI.Window (1, WindowRect, connectToServerFunc, "Connect to Server");
			} 
			else if (clicked == "setupServer") 
			{
				WindowRect = GUI.Window (1, WindowRect, setupServerFunc, "Setup Server");
			} 
			else if (clicked == "options") 
			{
				WindowRect = GUI.Window (1, WindowRect, optionsFunc, "Options");
			} 
			else if (clicked == "close")
			{
				//unpause time and close menu
				Time.timeScale = 1;
				onMenu = false;
			}
		}
	}
	
	private void Update()
	{
		if (Input.GetKey (KeyCode.Escape)) 
		{
			//on mneu, start at menu, and pause game
			onMenu = true;
			clicked = "";
			Time.timeScale = 0;
		}
	}


	private void multiplayerFunc(int id)
	{
		if (GUILayout.Button("Connect to Server"))
		{
			clicked = "connectServer";
		}
		if (GUILayout.Button("Setup Server"))
		{
			clicked = "setupServer";
		}
		if (GUILayout.Button("Back"))
		{
			clicked = "";
		}
	}

	public void setupServerFunc(int id)
	{
		GUILayout.Label ("Enter Server Name:");
		serverName = GUILayout.TextField (serverName);
		GUILayout.Label ("Server Connection Port:");
		connectionPort = int.Parse (GUILayout.TextField (connectionPort.ToString()));

		if (GUILayout.Button("Start Server"))
		{
			//maybe prelim validate fields entered
			//start server
		}

		if (GUILayout.Button("Back"))
		{
			clicked = "multiplayer";
		}
	}

	public void connectToServerFunc(int id)
	{
		GUILayout.Label ("Enter Player Name:");
		playerName = GUILayout.TextField (playerName);
		GUILayout.Label ("Server Connection IP:");
		connectionIP = GUILayout.TextField (connectionIP);
		GUILayout.Label ("Server Connection Port:");
		connectionPort = int.Parse (GUILayout.TextField (connectionPort.ToString ()));

		if (GUILayout.Button("Connect to Server"))
		{
			//maybe prelim validate fields entered
			//connect to server
		}

		if (GUILayout.Button("Back"))
		{
			clicked = "multiplayer";
		}
	}

	private void optionsFunc(int id)
	{
		//options buttons/toggles
		if (GlobalVars.invertXAxis = GUILayout.Toggle (GlobalVars.invertXAxis, "Invert X-axis"))
		{
		}
		if (GlobalVars.invertYAxis = GUILayout.Toggle(GlobalVars.invertYAxis, "Invert Y-axis"))
		{
		}
		if (GUILayout.Button("Back"))
		{
			clicked = "";
		}
		if (DragWindow)
			GUI.DragWindow(new Rect (0,0,Screen.width,Screen.height));
	}
	
	private void menuFunc(int id)
	{
		//buttons
		if (GUILayout.Button ("Play")) 
		{
			clicked = "play";
		}
		if (GUILayout.Button("Reset"))
		{
			clicked = "reset";
		}
		if (GUILayout.Button("Multiplayer"))
		{
			clicked = "multiplayer";
		}
		if (GUILayout.Button("Options"))
		{
			clicked = "options";
		}
		if (GUILayout.Button("Close"))
		{
			clicked = "close";
		}

		//functionality
		GUILayout.Label ("Press esc to open menu");
    	if (DragWindow)
      		GUI.DragWindow(new Rect(0, 0, Screen.width, Screen.height));
	}
}