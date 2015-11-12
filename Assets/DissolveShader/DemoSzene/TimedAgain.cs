using UnityEngine;
using System.Collections;

public class TimedAgain : MonoBehaviour
{
   
    float m_fDestruktionSpeed = 0.2F;
    public Material m_Mat;
    public float m_fTime;

    public Timed scriptTimed;

    void Start()
    {         
        m_Mat = GetComponent<Renderer>().material;
    }

    bool b_visible = true;
    bool b_again = false;

    void Update()
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
                StartCoroutine(again());
            }
        }

        if (b_again)
        {
            m_fTime += Time.deltaTime * m_fDestruktionSpeed;
            if (m_fTime >= 1.5f)
                m_fTime = 0;
            m_Mat.SetFloat("_Amount", m_fTime);

            if (m_fTime >= 1.45f)
            {                
                b_again = false;
                StartCoroutine(Backagain());
            }
        }
    }

    public GameObject objOtherHeart;
    public GameObject objRealHeart;

    IEnumerator Backagain()
    {
        yield return new WaitForSeconds(3);

        scriptTimed.b_visible = true;
        scriptTimed.m_fTime = 1f;

        m_Mat.SetFloat("_Amount", 1);
        scriptTimed.m_Mat.SetFloat("_Amount", 1);

        m_fTime = 1f;
        b_again = false;
        b_visible = true;

        objOtherHeart.SetActive(false);
        objRealHeart.SetActive(true);
    }


    IEnumerator again()
    {
        yield return new WaitForSeconds(2);

        b_again = true;
    }
}
