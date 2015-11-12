using UnityEngine;
using System.Collections;

public class TimedHologram : MonoBehaviour
{    

    public GameObject[] objArrow;
    public GameObject objLine;
    float m_fDestruktionSpeed = 0.2f;
    public Material m_Mat;
    public float m_fTime;
    public float FadeID;
    bool b_play = false;
    bool b_return = false;

    public GameObject objHeartDissorb;

	void Start () 
    {
        for (int i = 0; i < objArrow.Length; i++)
        {
            objArrow[i].SetActive(false);
        }
        objLine.SetActive(false);
        objHeartDissorb.SetActive(false);
        m_Mat = GetComponent<Renderer>().material;
        StartCoroutine(StartHologram());
	}

    public IEnumerator StartHologram()
    {
        b_return = false;
        yield return new WaitForSeconds(2);        
        b_play = true;
        m_fTime = 0.01f;
        GetComponent<AudioSource>().Play();
    }

	void Update () 
    {
        if (b_play)
        {
            if (b_return)
            {
                m_fTime -= Time.deltaTime * m_fDestruktionSpeed;
            }
            else
            {
                m_fTime += Time.deltaTime * m_fDestruktionSpeed;
            }

            if (m_fTime >= 1f) // 1초 뒤 라인 생기기
            {                
                //objLine.SetActive(true);
                //SoundManager.Instance.PlaySound(1);
            }

            if (m_fTime >= 2f) // 다시 삭제
            {
                m_fTime = 1;
                b_return = true;

                GetComponent<AudioSource>().Play();

                //objLine.SetActive(false);
            }

            if (m_fTime <= 0)
            {
                b_play = false;
                objHeartDissorb.SetActive(true);
                gameObject.transform.parent.gameObject.SetActive(false);
                // dissorb mesh on
            }

            // 화살표 나오는거 사용안함(홀로그램일경우)
            //if (m_fTime >= 1.5f)
            //{
            //    b_play = false;
            //    for (int i = 0; i < objArrow.Length; i++)
            //    {
            //        objArrow[i].SetActive(true);
            //    }
            //}
            m_Mat.SetFloat("_Fade", m_fTime);
        }
	}
}
