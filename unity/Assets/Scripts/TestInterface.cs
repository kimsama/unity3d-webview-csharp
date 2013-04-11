using UnityEngine;
using System.Collections;

public class TestInterface : MonoBehaviour 
{
	public GUISkin guiSkin;
	public GameObject redBoxPrefab;
	public GameObject blueBoxPrefab;
	
	private string note;
	
	// Use this for initialization
	void Start () 
	{
		WebMediator.Install();
	}
	
	// Update is called once per frame
	void Update () 
	{
	    if (WebMediator.IsVisible()) 
		{
	        ProcessMessages();
	    } 
		else if (Input.GetButtonDown("Fire1") && Input.mousePosition.y < Screen.height / 2) 
		{
	        ActivateWebView();
	    }	
	}
	
	// Show the web view (with margins) and load the index page.
	private void ActivateWebView() 
	{
	    WebMediator.LoadUrl("http://keijiro.github.com/unity-webview-integration/index.html");
	    WebMediator.SetMargin(12, Screen.height / 2 + 12, 12, 12);
	    WebMediator.Show();
	}
	
	// Hide the web view.
	private void DeactivateWebView() 
	{
	    WebMediator.Hide();
	    // Clear the state of the web view (by loading a blank page).
	    WebMediator.LoadUrl("about:blank");
	}
	
	// Process messages coming from the web view.
	private void ProcessMessages() 
	{
	    while (true) 
		{
	        // Poll a message or break.
	        WebMediator.WebMediatorMessage message = WebMediator.PollMessage();
	        if (message == null) break;
	
	        if (message.path == "/spawn") 
			{
				GameObject prefab = null;
				
	            // "spawn" message.
	            if (message.args.ContainsKey("color")) 
				{
	                prefab = (message.args["color"] == "red") ? redBoxPrefab : blueBoxPrefab;
	            } 
				else 
				{
	                prefab = Random.value < 0.5 ? redBoxPrefab : blueBoxPrefab;
	            }
				
	            var box = Instantiate(prefab, redBoxPrefab.transform.position, Random.rotation) as GameObject; 
	            if (message.args.ContainsKey("scale")) 
				{
	                box.transform.localScale = Vector3.one * float.Parse(message.args["scale"] as string);
	            }
	        } 
			else if (message.path == "/note") 
			{
	            // "note" message.
	            note = message.args["text"] as string;
	        } 
			else if (message.path == "/print") 
			{
	            // "print" message.
	            var text = message.args["line1"] as string;
	            if (message.args.ContainsKey("line2")) 
				{
	                text += "\n" + message.args["line2"] as string;
	            }
	            Debug.Log(text);
	            Debug.Log("(" + text.Length + " chars)");
	        } 
			else if (message.path == "/close") 
			{
	            // "close" message.
	            DeactivateWebView();
	        }
	    }
	}	

	void OnGUI() 
	{
	    int sw = Screen.width;
	    int sh = Screen.height;
	    GUI.skin = guiSkin;
	    if (!string.IsNullOrEmpty(note)) GUI.Label(new Rect(0, 0, sw, 0.5f * sh), note);
	    GUI.Label(new Rect(0, 0.5f * sh, sw, 0.5f * sh), "TAP HERE", "center");
	}
	
}
