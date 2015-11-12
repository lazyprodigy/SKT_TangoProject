using UnityEngine;
using System.Collections;

public class HeartRealSystem : MonoBehaviour 
{
    public static HeartRealSystem Instance;

    public GameObject objBrain;
    public Animation objBrainAnimation;
    public TimedAgainBrain scriptBrain;
    GazeInputModule gazeInputModule;



    void Awake()
    {
        Instance = GetComponent<HeartRealSystem>();
        gazeInputModule = FindObjectOfType<GazeInputModule>();
                    
    }

    public void StartBrain()
    {
        StartCoroutine(hideHeart());
    }

    IEnumerator hideHeart()
    {
        yield return new WaitForSeconds(0.01f);

        objBrain.SetActive(true);
        scriptBrain.b_visible = true;

        objBrainAnimation.Play("Start");
        objBrainAnimation.PlayQueued("Idle");

        gazeInputModule.SetGazePointerEnable(true);

        yield return new WaitForSeconds(0.01f);

        gameObject.SetActive(false);

    }

	void Update () 
    {
	    
	}
}
