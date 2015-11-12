using UnityEngine;
using System.Collections;

public class Timed : MonoBehaviour
{
    float m_fDestruktionSpeed = 0.2F;
    public Material m_Mat;
    public float m_fTime;

    void Start()
    {
        m_Mat = GetComponent<Renderer>().material;
    }

    public bool b_visible = true;

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
            }
        }
    }
}
