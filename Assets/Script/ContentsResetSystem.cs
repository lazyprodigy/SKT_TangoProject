using UnityEngine;
using System.Collections;

public class ContentsResetSystem : MonoBehaviour 
{
    public static ContentsResetSystem Instance;

    public TimedHologram scriptHeartHologram;
    public Timed scriptHeartDisorb;
    public TimedAgain scriptHeartDisorbCover;
    public HeartRealSystem scriptHeartReal;
    public TimedAgainBrain scriptBrain;

    public GameObject objbrain;
    public GameObject objheartHologram;
    public GameObject objheartDisorb;
    public GameObject objheartReal;

    public GameObject objScaner;
    public GameObject objHeadSphere;
    

    void Awake()
    {
        Instance = GetComponent<ContentsResetSystem>();
    }

    public void ResetStart()
    {
        Debug.Log("Reset!");
        objbrain.SetActive(false);
        objheartHologram.SetActive(true);
        objScaner.SetActive(false);
        objHeadSphere.SetActive(false);
        StartCoroutine(scriptHeartHologram.StartHologram());
    }

	void Start () 
    {
	
	}
		
	void Update () 
    {
	
	}

}
