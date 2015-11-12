using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Tango;
public class CameraTriggerSystem : MonoBehaviour
{
    public GameObject objPopUp;
    Animation AnimationPop;
    MeshRenderer RenderPop;

    Image imgSelected;

    public Texture[] tex_PopUp;

    public Texture[] tex_yes;
    public Texture[] tex_no;
    public Texture[] tex_next;
    public Texture[] tex_prev;
    public Texture[] tex_view;

    public AudioSource audioBG;

    float f_count = 0;

    bool b_once = true;
    bool b_count = true;

    GameManager gameManager;
    GazeInputModule gazeInputModule;
    Vector3 vec3_remember;
    public Transform _transform;
    Vector3 planeCenterPosition = Vector3.zero;
    string tDebug = "";
    bool testMode = false;

    void Start()
    {
        gazeInputModule = FindObjectOfType<GazeInputModule>();
        AnimationPop = objPopUp.GetComponent<Animation>();
        RenderPop = objPopUp.GetComponent<MeshRenderer>();
        objPopUp.SetActive(false);

        _transform = this.gameObject.transform;
        gameManager = FindObjectOfType<GameManager>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            testMode = true;
        }
    }
    void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Trigger Enter : " + other.name);

        if (other.CompareTag("btn"))
        {
            if (other.gameObject.name == "btnYes")
            {
                other.gameObject.GetComponent<Renderer>().material.mainTexture = tex_yes[1];
            }
            else if (other.gameObject.name == "btnNo")
            {
                other.gameObject.GetComponent<Renderer>().material.mainTexture = tex_no[1];
            }
            b_once = true;
            b_count = true;
            f_count = 0; // 초기화      
        }
        else if (other.CompareTag("Popup"))
        {
            b_once = true;
            b_count = true;
            f_count = 0; // 초기화      

            imgSelected = other.GetComponent<Image>();
        }
        else if (other.CompareTag("btnNext"))
        {
            b_once = true;
            b_count = true;
            f_count = 0; // 초기화
            other.transform.GetChild(1).GetComponent<MeshRenderer>().material.mainTexture = tex_next[1];            
        }
        else if (other.CompareTag("btnPrev"))
        {
            b_once = true;
            b_count = true;
            f_count = 0; // 초기화
            other.transform.GetChild(1).GetComponent<MeshRenderer>().material.mainTexture = tex_prev[1];
        }
        else if (other.CompareTag("btnView"))
        {
            b_once = true;
            b_count = true;
            f_count = 0; // 초기화
            other.transform.GetChild(1).GetComponent<MeshRenderer>().material.mainTexture = tex_view[1];
        }
        else { }
      
    }

    void OnTriggerStay(Collider other)
    {      
        #region 
        if (b_once)
        {
            if (other.CompareTag("btn"))
            {                
                f_count = f_count + 0.01f; // 증가
           
                if (b_count)
                {
                    if (f_count > 1) //1초 체크
                    {             
                        b_once = false;
                        b_count = false;
                        f_count = 0; //초기화
                        
                        if (other.gameObject.name == "btnYes")
                        {
                            AugmentedRealityGUIController.Instance.hideUI();
                            //AugmentedRealityGUIController.Instance.ShowHeart(true);
                            //Transform tangoPointCloud = FindObjectOfType<TangoPointCloud>().transform;
                            //tangoPointCloud.gameObject.SetActive(false);
                            gameManager.FindPlaneComplete();
                            SoundManager.Instance.PlaySound(0);
                        }
                        else if (other.gameObject.name == "btnNo")
                        {
                            AugmentedRealityGUIController.Instance.hideUI();
                            AugmentedRealityGUIController.Instance.ShowHideScan(true);                            
                        }                        
                    }
                }
            }
        }
        #endregion // 일반 메뉴 // tag menu version

        #region // tag popup version
        if (b_once)
        {
            if (other.CompareTag("Popup"))
            {
                f_count = f_count + 0.01f; // 증가
                imgSelected.fillAmount = f_count / 1f;
                if (b_count)
                {
                    if (f_count > 1) //1초 체크
                    {
                        b_once = false;
                        b_count = false;
                        f_count = 0; //초기화

                        SoundManager.Instance.PlaySound(0);

                        if (other.gameObject.name == "RightAtriums")
                        {
                            RenderPop.material.mainTexture = tex_PopUp[0];
                        }
                        else if (other.gameObject.name == "LeftAtriums")
                        {
                            RenderPop.material.mainTexture = tex_PopUp[1];
                        }
                        else if (other.gameObject.name == "RightVentriculars")
                        {
                            RenderPop.material.mainTexture = tex_PopUp[2];
                        }
                        else if (other.gameObject.name == "LeftVentriculars")
                        {
                            RenderPop.material.mainTexture = tex_PopUp[3];
                        }
                        
                        StartCoroutine(autoUISwitch());
                    }
                }
            }
        }
        #endregion // 일반 메뉴
        
        #region // Button Next, Prev, View version
        if (b_once)
        {
            if (other.CompareTag("btnNext") || other.CompareTag("btnPrev") || other.CompareTag("btnView"))
            {
                f_count = f_count + 0.01f; // 증가                
                if (b_count)
                {
                    //Debug.Log(f_count);
                    
                    if (f_count >= 1.0f) //1초 체크
                    {
                        b_once = false;
                        b_count = false;
                        f_count = 0; //초기화

                        SoundManager.Instance.PlaySound(0);

                        if (other.gameObject.name == "BtnNext")
                        {
                            other.transform.GetChild(1).GetComponent<MeshRenderer>().material.mainTexture = tex_next[0];
                            HeartRealSystem.Instance.StartBrain();                            
                        }
                        else if (other.gameObject.name == "BtnPrev")
                        {
                            other.transform.GetChild(1).GetComponent<MeshRenderer>().material.mainTexture = tex_prev[0];
                            gazeInputModule.SetGazePointerEnable(false);
                            ContentsResetSystem.Instance.ResetStart();
                        }
                        else if (other.gameObject.name == "BtnView")
                        {
                            other.transform.GetChild(1).GetComponent<MeshRenderer>().material.mainTexture = tex_view[0];
                            gameManager.CameraSwitch(); // 양안,단안 스위칭
                        }
                    }
                }
            }
        }
        #endregion // 일반 메뉴

        #region // 뇌
        if (other.CompareTag("HeadSphere"))
        {
            GazeInputModule gazeInputModule = FindObjectOfType<GazeInputModule>();
            gazeInputModule.SetScannerEnable(true);
        }
        #endregion //뇌
    }

    IEnumerator autoUISwitch()
    {
        objPopUp.SetActive(true);
        AnimationPop.Play("Open");

        yield return new WaitForSeconds(5);

        AnimationPop.Play("Close");

        yield return new WaitForSeconds(1);

        objPopUp.SetActive(true);
        
        f_count = 0; // 초기화         
        b_once = true;
        b_count = true;
    }

    void OnTriggerExit(Collider other)
    {
        //Debug.Log("Trigger Exit");
       
        if (other.CompareTag("btn"))
        {           
            f_count = 0; // 초기화         
            b_once = true;
            b_count = true;

            if (other.gameObject.name == "btnYes")
            {
                other.gameObject.GetComponent<Renderer>().material.mainTexture = tex_yes[0];
            }
            else if (other.gameObject.name == "btnNo")
            {
                other.gameObject.GetComponent<Renderer>().material.mainTexture = tex_no[0];
            }
        }
        else if (other.CompareTag("Popup"))
        {
            f_count = 0; // 초기화         
            b_once = true;
            b_count = true;

            imgSelected.fillAmount = 1;
        }
        else if (other.CompareTag("btnNext"))
        {
            f_count = 0; b_once = true; b_count = true;
            other.transform.GetChild(1).GetComponent<MeshRenderer>().material.mainTexture = tex_next[0];
        }
        else if (other.CompareTag("btnPrev"))
        {
            f_count = 0; b_once = true; b_count = true;
            other.transform.GetChild(1).GetComponent<MeshRenderer>().material.mainTexture = tex_prev[0];
        }
        else if (other.CompareTag("btnView"))
        {
            f_count = 0; b_once = true; b_count = true;
            other.transform.GetChild(1).GetComponent<MeshRenderer>().material.mainTexture = tex_view[0];
        }
        else if (other.CompareTag("HeadSphere"))
        {
            GazeInputModule gazeInputModule = FindObjectOfType<GazeInputModule>();
            gazeInputModule.SetScannerEnable(false);
        }
        else { }        
    }

    //getter/setter----------------------------------------
    public Vector3 GetCameraPosition()
    {
        return _transform.position;
    }

    public Quaternion GetCameraRotation()
    {
        return _transform.rotation;
    }
    public void SetPositionToPlane(Transform tr)
    {
        Vector3 dir = tr.position - transform.position;
        dir.y = 0;
        dir.Normalize();
        if (!testMode)
        {
            planeCenterPosition = AugmentedRealityGUIController.Instance.GetPlaneCenterPosition();
            //tDebug += "getPlaneCenter: " + AugmentedRealityGUIController.Instance.GetPlaneCenterPosition() + "\n";
        }
        else
        {
            planeCenterPosition = Vector3.zero;
        }
        tr.transform.position = planeCenterPosition;
        tr.transform.rotation = Quaternion.LookRotation(dir);
       // tDebug += "trPos: " + tr.transform.position + "\n";
    }
    /*
    void OnGUI()
    {
        GUI.Label(new Rect(10, Screen.height/2, 400, 200), "<size=25><color=#ffffff>" + tDebug + "</color></size>"); //왼쪽 중단
    }
     */ 
}
