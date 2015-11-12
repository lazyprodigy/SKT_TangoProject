using UnityEngine;
using System.Collections;

public class ScriptManager : MonoBehaviour 
{
    public static ScriptManager Instance;

    float timeA;
    public int fps;
    public int lastFPS;
    public GUIStyle textStyle;
    public const string UI_FONT_SIZE = "<size=25>";
    public const string UI_FONT_COLOR = "<color=#ffffff>";
    void Awake()
    {
        Instance = GetComponent<ScriptManager>();
    }

    public string strChecker = "Ready";

    // Use this for initialization
    void Start()
    {
        timeA = Time.timeSinceLevelLoad;
        DontDestroyOnLoad(this);
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(Time.timeSinceLevelLoad+" "+timeA);
        if (Time.timeSinceLevelLoad - timeA <= 1)
        {
            fps++;
        }
        else
        {
            lastFPS = fps + 1;
            timeA = Time.timeSinceLevelLoad;
            fps = 0;
        }
    }
    /*
    void OnGUI()
    {
        GUI.color = Color.white;
        GUI.Label(new Rect(250, 10, 100, 50), UI_FONT_SIZE + UI_FONT_COLOR + "FPS : " + fps + "/" + lastFPS + "</color></size>", textStyle);
        GUI.Label(new Rect(250, 60, 100, 50), UI_FONT_SIZE + UI_FONT_COLOR + strChecker + "</color></size>", textStyle);
    }
     */ 
}
