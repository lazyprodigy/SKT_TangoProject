using UnityEngine;
using System.Collections;

public class MriDisplayer : MonoBehaviour 
{
    public GameObject objScaner;
        
    public Texture[] texMri;
    MeshRenderer render;

	// Use this for initialization
	void Start () 
    {
        render = GetComponent<MeshRenderer>();
        //render.material.mainTexture = texMri[1];
	}
	
	// Update is called once per frame
	void Update () 
    {
        int intNum = (int)(Mathf.Exp(objScaner.transform.localPosition.y)/4);
        
        if (intNum <= 17 || intNum < 0)
        {
            //Debug.Log(intNum);   
            render.material.mainTexture = texMri[intNum];
        }

#if false
        Debug.Log(Mathf.Log10(objScaner.transform.localPosition.y)*10
            
            + "/1 \n"+
            Mathf.Acos(objScaner.transform.localPosition.y)
            + "/2 \n" +
            Mathf.Asin(objScaner.transform.localPosition.y)
            + "/3 \n" +
            Mathf.Atan(objScaner.transform.localPosition.y)
            + "/4 \n" +
            Mathf.CeilToInt(objScaner.transform.localPosition.y)
            + "/5 \n" +
            Mathf.Ceil(objScaner.transform.localPosition.y)
             + "/6 \n" +
            Mathf.Clamp01(objScaner.transform.localPosition.y)
             + "/7 \n" +
            Mathf.Cos(objScaner.transform.localPosition.y)
             + "/8 \n" +
            Mathf.Exp(objScaner.transform.localPosition.y)
             + "/9 \n" +
            Mathf.FloatToHalf(objScaner.transform.localPosition.y)
             + "/10 \n" +
            Mathf.Floor(objScaner.transform.localPosition.y)
              + "/11 \n" +
            Mathf.FloorToInt(objScaner.transform.localPosition.y)
              + "/12 \n" +
            Mathf.LinearToGammaSpace(objScaner.transform.localPosition.y)
              + "/13 \n" +
            Mathf.Log(objScaner.transform.localPosition.y)
              + "/14 \n" +
            Mathf.Round(objScaner.transform.localPosition.y)
                 + "/15 \n" +
            Mathf.RoundToInt(objScaner.transform.localPosition.y)
                 + "/16 \n" +
            Mathf.Ceil(objScaner.transform.localPosition.y)            
                 + "/ \n" +
            Mathf.Sign(objScaner.transform.localPosition.y)            
                 + "/ \n" +
            Mathf.Sin(objScaner.transform.localPosition.y)
            + "/ \n" +
            Mathf.Sqrt(objScaner.transform.localPosition.y)
            + "/ \n" +
            Mathf.Tan(objScaner.transform.localPosition.y)        
            );
#endif

	}
}
