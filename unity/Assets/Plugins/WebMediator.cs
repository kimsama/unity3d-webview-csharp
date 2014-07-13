using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

public class WebMediator : MonoBehaviour 
{
	private static WebMediator instance;
	private static bool isClearCache;
	
	private string lastRequestedUrl;
	private bool loadRequest;
	private bool visibility;
	private int leftMargin;
	private int topMargin;
	private int rightMargin;
	private int bottomMargin;

	// Message container class.
	public class WebMediatorMessage 
	{	
	    public string path;      // Message path
	    public Hashtable args;   // Argument table
	
	    public WebMediatorMessage(string rawMessage)
		{
	        // Retrieve a path.
	        var split = rawMessage.Split('?');
	        path = split[0];
	        // Parse arguments.
	        args = new Hashtable();
	        if (split.Length > 1) 
			{
	            foreach (var pair in split[1].Split('&'))
				{
	                var elems = pair.Split('=');
	                args[elems[0]] = WWW.UnEscapeURL(elems[1]);
	            }
	        }
	    }
	}
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	    UpdatePlatform();
    	instance.loadRequest = false;
	
	}
	
	// Install the plugin.
	// Call this at least once before using the plugin.
	public static void Install() 
	{
	    if (instance == null) 
		{
	        GameObject master = new GameObject("WebMediator");
	        DontDestroyOnLoad(master);
	        instance = master.AddComponent<WebMediator>();
			
	        InstallPlatform();
	    }
	}

	// Set margins around the web view.
	public static void SetMargin(int left, int top, int right, int bottom) 
	{
	    instance.leftMargin = left;
	    instance.topMargin = top;
	    instance.rightMargin = right;
	    instance.bottomMargin = bottom;
		
	    ApplyMarginsPlatform();
	}

	// Visibility functions.
	public static void Show()
	{
	    instance.visibility = true;
	}
	
	public static void Hide() 
	{
	    instance.visibility = false;
	}
	
	public static bool IsVisible() {
	    return instance.visibility;
	}

	public static void SetClearCache()
	{
	    isClearCache = true;
	}

	public static void  SetCache()
	{
	    isClearCache = false;
	}

	// Load the page at the URL.
	public static void LoadUrl(string url) 
	{
	    instance.lastRequestedUrl = url;
	    instance.loadRequest = true;
	}

#if UNITY_EDITOR
	// Unity Editor implementation.
	
	private static void InstallPlatform() { }
	private static void UpdatePlatform() { }
	private static void ApplyMarginsPlatform() { }
	public static WebMediatorMessage PollMessage()  { return null; }
	public static void MakeTransparentWebViewBackground() { }
#elif UNITY_IPHONE
	// iOS platform implementation.
	
	 [DllImport ("__Internal")] static extern private void _WebViewPluginInstall();
	 [DllImport ("__Internal")] static extern private void _WebViewPluginLoadUrl(string url, bool isClearCache);
	 [DllImport ("__Internal")] static extern private void _WebViewPluginSetVisibility(bool visibility);
	 [DllImport ("__Internal")] static extern private void _WebViewPluginSetMargins(int left, int top, int right, int bottom);
	 [DllImport ("__Internal")] static extern private string _WebViewPluginPollMessage();
	 [DllImport ("__Internal")] static extern private void _WebViewPluginMakeTransparentBackground();
	
	private static bool viewVisibility;
	
	private static void InstallPlatform() 
	{
	    _WebViewPluginInstall();
	}
	
	private static void ApplyMarginsPlatform() 
	{
	    _WebViewPluginSetMargins(instance.leftMargin, instance.topMargin, instance.rightMargin, instance.bottomMargin);
	}
	
	private static void UpdatePlatform() 
	{
	    if (viewVisibility != instance.visibility) 
		{
	        viewVisibility = instance.visibility;
	        _WebViewPluginSetVisibility(viewVisibility);
	    }
	    if (instance.loadRequest) 
		{
	        instance.loadRequest = false;
	        _WebViewPluginLoadUrl(instance.lastRequestedUrl, isClearCache);
	    }
	}
	
	public static WebMediatorMessage PollMessage()
	{
	    string message =  _WebViewPluginPollMessage();
	    return !string.IsNullOrEmpty(message) ? new WebMediatorMessage(message) : null;
	}
	
	public static void MakeTransparentWebViewBackground()
	{
	    _WebViewPluginMakeTransparentBackground();
	}

#elif UNITY_ANDROID
	// Android platform implementation.
		
	private static AndroidJavaClass unityPlayerClass;
	
	private static void InstallPlatform() 
	{
	    unityPlayerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
	}
	
	private static void ApplyMarginsPlatform() { }
	
	private static void UpdatePlatform() 
	{
	    var activity = unityPlayerClass.GetStatic<AndroidJavaObject>("currentActivity");
	    activity.Call("updateWebView", instance.lastRequestedUrl != null ? instance.lastRequestedUrl : "", instance.loadRequest, instance.visibility, instance.leftMargin, instance.topMargin, instance.rightMargin, instance.bottomMargin);
	}

	public static WebMediatorMessage PollMessage()
	{
	    var activity = unityPlayerClass.GetStatic<AndroidJavaObject>("currentActivity");
	    var message = activity.Call<string>("pollWebViewMessage");
		return message!= null ? new WebMediatorMessage(message) : null;
	}
	
	public static void MakeTransparentWebViewBackground()
	{
	    var activity = unityPlayerClass.GetStatic<AndroidJavaObject>("currentActivity");
	    activity.Call("makeTransparentWebViewBackground");
	}
#endif
	
}
