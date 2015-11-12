using UnityEngine;
using System.Collections;

public class TimedAgainBrain : MonoBehaviour
{
    float m_fDestruktionSpeed = 0.15F;
    public Material m_Mat;
    public float m_fTime;

  
    public GameObject objRealBrain;

    public GameObject objScanner;
    public GameObject objHeadCollider;
    public GameObject objHeadDisplay;
    
    void Awake()
    {
        //Debug.Log(gameObject.transform.parent.name);
        objScanner.SetActive(false);
        objHeadCollider.SetActive(false);
        objHeadDisplay.SetActive(false);        
    }
    
	void Start () 
    {        
        m_Mat = GetComponent<Renderer>().material;
        
        //GetComponent<AudioSource>().Play();
	}


    public bool b_visible = true;
   // bool b_again = false;

	void Update () 
    {
        if (b_visible)
        {
            m_fTime -= Time.deltaTime * m_fDestruktionSpeed;
            if (m_fTime <= 0.001f)
                m_fTime = 1;
            m_Mat.SetFloat("_Amount", m_fTime);

            if (m_fTime <= 0.01f)
            {
                b_visible = false;
                //StartCoroutine(again());

                objScanner.SetActive(true);
                //Animator anima = objScanner.GetComponent<Animator>();                
                //anima.Play("Scanner 1", -1, 0);
                                
                objHeadCollider.SetActive(true);
                
                objHeadDisplay.SetActive(true);
               // objHeadDisplay.GetComponent<Animator>().Play("Display");
            }
        }

        //if (b_again)
        //{
        //    m_fTime += Time.deltaTime * m_fDestruktionSpeed;
        //    if (m_fTime >= 1.5f)
        //        m_fTime = 0;
        //    m_Mat.SetFloat("_Amount", m_fTime);

        //    //if (m_fTime >= 1.45f)
        //    //{
        //    //    b_again = false;
        //    //    objRealBrain.SetActive(false);
        //    //}
        //    if (m_fTime >= 0.8f)
        //    { 
        //        // 마지막 단계 프로젝트 스캐너 생김
              
        //    }
        //}
	}



    IEnumerator again()
    {
        yield return new WaitForSeconds(6.5f);

        m_fDestruktionSpeed = 0.3F;
        //b_again = true;
    }
}
