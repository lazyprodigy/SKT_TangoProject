using UnityEngine;
using System.Collections;

public class ScrollGUI : MonoBehaviour 
{
    public GameObject videoPlane;

	// Use this for initialization
	void Start () 
    {
        vSliderValue = 1.6f;
	}

    public float vSliderValue = 0.0F;
    void OnGUI()
    {
      //  GUI.Label(new Rect(0, 0, 100, 100), vSliderValue.ToString());
      //  vSliderValue = GUI.VerticalSlider(new Rect(25, 25, 100, 100), vSliderValue, 1.6F, 0.0F);
    }

	// Update is called once per frame
	void Update () 
    {
        videoPlane.transform.localScale = new Vector3(vSliderValue, 1, 1);
	}
}
