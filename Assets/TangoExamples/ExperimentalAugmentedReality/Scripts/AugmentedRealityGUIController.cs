//-----------------------------------------------------------------------
// <copyright file="AugmentedRealityGUIController.cs" company="Google">
//
// Copyright 2015 Google Inc. All Rights Reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Tango;

/// <summary>
/// GUI controller controls all the debug overlay to show the data for poses.
/// </summary>
public class AugmentedRealityGUIController : MonoBehaviour
{
    // Constant value for controlling the position and size of debug overlay.
    public const float UI_LABEL_START_X = 15.0f;
    public const float UI_LABEL_START_Y = 15.0f;
    public const float UI_LABEL_SIZE_X = 1920.0f;
    public const float UI_LABEL_SIZE_Y = 35.0f;
    public const float UI_LABEL_GAP_Y = 3.0f;
    public const float UI_BUTTON_SIZE_X = 250.0f;
    public const float UI_BUTTON_SIZE_Y = 130.0f;
    public const float UI_BUTTON_GAP_X = 5.0f;
    public const float UI_CAMERA_BUTTON_OFFSET = UI_BUTTON_SIZE_X + UI_BUTTON_GAP_X; 
    public const float UI_LABEL_OFFSET = UI_LABEL_GAP_Y + UI_LABEL_SIZE_Y;
    public const float UI_FPS_LABEL_START_Y = UI_LABEL_START_Y + UI_LABEL_OFFSET;
    public const float UI_EVENT_LABEL_START_Y = UI_FPS_LABEL_START_Y + UI_LABEL_OFFSET;
    public const float UI_POSE_LABEL_START_Y = UI_EVENT_LABEL_START_Y + UI_LABEL_OFFSET;
    public const float UI_DEPTH_LABLE_START_Y = UI_POSE_LABEL_START_Y + UI_LABEL_OFFSET;
    public const string UI_FLOAT_FORMAT = "F3";
    public const string UI_FONT_SIZE = "<size=25>";
    
    public const float UI_TANGO_VERSION_X = UI_LABEL_START_X;
    public const float UI_TANGO_VERSION_Y = UI_LABEL_START_Y;
    public const float UI_TANGO_APP_SPECIFIC_START_X = UI_TANGO_VERSION_X;
    public const float UI_TANGO_APP_SPECIFIC_START_Y = UI_TANGO_VERSION_Y + (UI_LABEL_OFFSET * 2);
    
    public const string UX_SERVICE_VERSION = "Service version: {0}";
    public const string UX_TANGO_SERVICE_VERSION = "Tango service version: {0}";
    public const string UX_TANGO_SYSTEM_EVENT = "Tango system event: {0}";
    public const string UX_TARGET_TO_BASE_FRAME = "Target->{0}, Base->{1}:";
    public const string UX_STATUS = "\tstatus: {0}, count: {1}, position (m): [{2}], orientation: [{3}]";
    public const float SECOND_TO_MILLISECOND = 1000.0f;

    /// <summary>
    /// How big (in pixels) is a tap.
    /// </summary>
    public const float TAP_PIXEL_TOLERANCE = 40;

    /// <summary>
    /// Minimum inlier percentage to consider a plane a fit.
    /// </summary>
    public const float MIN_PLANE_FIT_PERCENTAGE = 0.8f;

    public ARScreen m_arScreen;

    /// <summary>
    /// The location prefab to place on taps.
    /// </summary>
    public GameObject obj_ui;

    /// <summary>
    /// The point cloud object in the scene.
    /// </summary>
    public TangoPointCloud m_pointCloud;

    private const float FPS_UPDATE_FREQUENCY = 1.0f;
    private string m_fpsText;
    private int m_currentFPS;
    private int m_framesSinceUpdate;
    private float m_accumulation;
    private float m_currentTime;
    
    private Rect m_label;
    private TangoApplication m_tangoApplication;
    private string m_tangoServiceVersion;

    /// <summary>
    /// If set, this is the selected marker.
    /// </summary>
    private ARLocationMarker m_selectedMarker;

    /// <summary>
    /// If set, this is the rectangle bounding the selected marker.
    /// </summary>
    private Rect m_selectedRect;

    /// <summary>
    /// If set, this is the rectangle for the Hide All button.
    /// </summary>
    private Rect m_hideAllRect;

    /// <summary>
    /// If set, show debug text.
    /// </summary>
    private bool m_showDebug = false;
    
    /// <summary>
    /// Unity Start() callback, we set up some initial values here.
    /// </summary>
    string tDebug = "";
    public static AugmentedRealityGUIController Instance;
    Vector3 planeOriginPosition = Vector3.zero;

    void Awake()
    {
        Instance = GetComponent<AugmentedRealityGUIController>();
    }

    /// <summary>
    /// findPlane 실행시간 확인
    /// </summary>
    int intTime = 0;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="b_state"></param>
    /// <param name="b_camera"></param>
    public void ShowHideRader(bool b_state, bool b_camera)
    {
        if (b_state)
        {
            if (b_camera)
            {
                SoundManager.Instance.PlaySound(1);
                intTime = 5;
            }
            else
            {
                intTime = 0;
            }

            Debug.Log("TIME : " + intTime);
            Invoke("StartRader", intTime);
        }
        else
        {
            objPivot.SetActive(false);
            b_findPlane = false;
        }
    }

    public void StartRader()
    {
        objPivot.SetActive(true);
        b_findPlane = true;
    }


    public void Start() 
    {
        ShowHideRader(false, false);   
        /*
        m_currentFPS = 0;
        m_framesSinceUpdate = 0;
        m_currentTime = 0.0f;
        m_fpsText = "FPS = Calculating";
        m_label = new Rect((Screen.width * 0.025f) - 50, (Screen.height * 0.96f) - 25, 600.0f, 50.0f);
         */ 
        m_tangoApplication = FindObjectOfType<TangoApplication>();
        m_tangoServiceVersion = TangoApplication.GetTangoServiceVersion();
    }

    public bool b_findPlane = false;

    /// <summary>
    /// Updates UI and handles player input.
    /// </summary>
    public void Update() 
    {
        /*
        m_currentTime += Time.deltaTime;
        ++m_framesSinceUpdate;
        m_accumulation += Time.timeScale / Time.deltaTime;
        if (m_currentTime >= FPS_UPDATE_FREQUENCY)
        {
            m_currentFPS = (int)(m_accumulation / m_framesSinceUpdate);
            m_currentTime = 0.0f;
            m_framesSinceUpdate = 0;
            m_accumulation = 0.0f;
            m_fpsText = "FPS: " + m_currentFPS;
        }
        */
        if (b_findPlane)
        {

            _UpdateAutoLocationMarker();
            //_UpdateLocationMarker();

        }
    }
    
    /// <summary>
    /// Display simple GUI.
    /// </summary>
    public void OnGUI()
    {
        //GUI.Label(new Rect(Screen.width - 200, 10, 200, 200), "<size=25><color=#fffff>" + tDebug + "</Color></size>"); //왼쪽 하단
        if (m_showDebug && m_tangoApplication.HasRequestedPermissions())
        {
            Color oldColor = GUI.color;
            GUI.color = Color.white;
            
            GUI.color = Color.black;
            GUI.Label(new Rect(UI_LABEL_START_X, 
                               UI_LABEL_START_Y, 
                               UI_LABEL_SIZE_X, 
                               UI_LABEL_SIZE_Y), 
                      UI_FONT_SIZE + String.Format(UX_TANGO_SERVICE_VERSION, m_tangoServiceVersion) + "</size>");
            
            GUI.Label(new Rect(UI_LABEL_START_X, 
                               UI_FPS_LABEL_START_Y, 
                               UI_LABEL_SIZE_X, 
                               UI_LABEL_SIZE_Y),
                      UI_FONT_SIZE + m_fpsText + "</size>");
            
            // MOTION TRACKING
            GUI.Label(new Rect(UI_LABEL_START_X, 
                               UI_POSE_LABEL_START_Y - UI_LABEL_OFFSET,
                               UI_LABEL_SIZE_X, 
                               UI_LABEL_SIZE_Y),
                      UI_FONT_SIZE + String.Format(UX_TARGET_TO_BASE_FRAME, "Device", "Start") + "</size>");
            
            Vector3 pos = m_arScreen.transform.position;
            Quaternion quat = m_arScreen.transform.rotation;
            string positionString = pos.x.ToString(UI_FLOAT_FORMAT) + ", " + 
                pos.y.ToString(UI_FLOAT_FORMAT) + ", " + 
                    pos.z.ToString(UI_FLOAT_FORMAT);
            string rotationString = quat.x.ToString(UI_FLOAT_FORMAT) + ", " + 
                quat.y.ToString(UI_FLOAT_FORMAT) + ", " + 
                    quat.z.ToString(UI_FLOAT_FORMAT) + ", " + 
                    quat.w.ToString(UI_FLOAT_FORMAT);
            string statusString = String.Format(UX_STATUS,
                                                _GetLoggingStringFromPoseStatus(m_arScreen.m_status),
                                                _GetLoggingStringFromFrameCount(m_arScreen.m_frameCount),
                                                positionString, rotationString);
            GUI.Label(new Rect(UI_LABEL_START_X, 
                               UI_POSE_LABEL_START_Y,
                               UI_LABEL_SIZE_X, 
                               UI_LABEL_SIZE_Y), 
                      UI_FONT_SIZE + statusString + "</size>");
            GUI.color = oldColor;
        }

        if (m_selectedMarker != null)
        {
            Renderer selectedRenderer = m_selectedMarker.GetComponent<Renderer>();

            // GUI's Y is flipped from the mouse's Y
            Rect screenRect = WorldBoundsToScreen(Camera.main, selectedRenderer.bounds);
            float yMin = Screen.height - screenRect.yMin;
            float yMax = Screen.height - screenRect.yMax;
            screenRect.yMin = Mathf.Min(yMin, yMax);
            screenRect.yMax = Mathf.Max(yMin, yMax);

            if (GUI.Button(screenRect, "<size=30>Hide</size>"))
            {
                m_selectedMarker.SendMessage("Hide");
                m_selectedMarker = null;
                m_selectedRect = new Rect();
            }
            else
            {
                m_selectedRect = screenRect;
            }
        }
        else
        {
            m_selectedRect = new Rect();
        }

        if (GameObject.FindObjectOfType<ARLocationMarker>() != null)
        {
            m_hideAllRect = new Rect(Screen.width - UI_BUTTON_SIZE_X - UI_BUTTON_GAP_X,
                                     Screen.height - UI_BUTTON_SIZE_Y - UI_BUTTON_GAP_X,
                                     UI_BUTTON_SIZE_X,
                                     UI_BUTTON_SIZE_Y);
            if (GUI.Button(m_hideAllRect, "<size=30>Hide All</size>"))
            {
                foreach (ARLocationMarker marker in GameObject.FindObjectsOfType<ARLocationMarker>())
                {
                    marker.SendMessage("Hide");
                }
                tDebug = "";
            }
        }
        else
        {
            m_hideAllRect = new Rect(0, 0, 0, 0);
        }
    }



    /// <summary>
    /// Convert a 3D bounding box into a 2D Rect.
    /// </summary>
    /// <returns>The 2D Rect in Screen coordinates.</returns>
    /// <param name="cam">Camera to use.</param>
    /// <param name="bounds">3D bounding box.</param>
    private Rect WorldBoundsToScreen(Camera cam, Bounds bounds)
    {
        Vector3 center = bounds.center;
        Vector3 extents = bounds.extents;
        Bounds screenBounds = new Bounds(cam.WorldToScreenPoint(center), Vector3.zero);
        
        screenBounds.Encapsulate(cam.WorldToScreenPoint(center + new Vector3(+extents.x, +extents.y, +extents.z)));
        screenBounds.Encapsulate(cam.WorldToScreenPoint(center + new Vector3(+extents.x, +extents.y, -extents.z)));
        screenBounds.Encapsulate(cam.WorldToScreenPoint(center + new Vector3(+extents.x, -extents.y, +extents.z)));
        screenBounds.Encapsulate(cam.WorldToScreenPoint(center + new Vector3(+extents.x, -extents.y, -extents.z)));
        screenBounds.Encapsulate(cam.WorldToScreenPoint(center + new Vector3(-extents.x, +extents.y, +extents.z)));
        screenBounds.Encapsulate(cam.WorldToScreenPoint(center + new Vector3(-extents.x, +extents.y, -extents.z)));
        screenBounds.Encapsulate(cam.WorldToScreenPoint(center + new Vector3(-extents.x, -extents.y, +extents.z)));
        screenBounds.Encapsulate(cam.WorldToScreenPoint(center + new Vector3(-extents.x, -extents.y, -extents.z)));
        return Rect.MinMaxRect(screenBounds.min.x, screenBounds.min.y, screenBounds.max.x, screenBounds.max.y);
    }

    /// <summary>
    /// Construct readable string from TangoPoseStatusType.
    /// </summary>
    /// <param name="status">Pose status from Tango.</param>
    /// <returns>Readable string corresponding to status.</returns>
    private string _GetLoggingStringFromPoseStatus(TangoEnums.TangoPoseStatusType status)
    {
        string statusString;
        switch (status)
        {
        case TangoEnums.TangoPoseStatusType.TANGO_POSE_INITIALIZING:
            statusString = "initializing";
            break;
        case TangoEnums.TangoPoseStatusType.TANGO_POSE_INVALID:
            statusString = "invalid";
            break;
        case TangoEnums.TangoPoseStatusType.TANGO_POSE_UNKNOWN:
            statusString = "unknown";
            break;
        case TangoEnums.TangoPoseStatusType.TANGO_POSE_VALID:
            statusString = "valid";
            break;
        default:
            statusString = "N/A";
            break;
        }
        return statusString;
    }
    
    /// <summary>
    /// Reformat string from vector3 type for data logging.
    /// </summary>
    /// <param name="vec">Position to display.</param>
    /// <returns>Readable string corresponding to vec.</returns>
    private string _GetLoggingStringFromVec3(Vector3 vec)
    {
        if (vec == Vector3.zero)
        {
            return "N/A";
        }
        else
        {
            return string.Format("{0}, {1}, {2}", 
                                 vec.x.ToString(UI_FLOAT_FORMAT),
                                 vec.y.ToString(UI_FLOAT_FORMAT),
                                 vec.z.ToString(UI_FLOAT_FORMAT));
        }
    }
    
    /// <summary>
    /// Reformat string from quaternion type for data logging.
    /// </summary>
    /// <param name="quat">Quaternion to display.</param>
    /// <returns>Readable string corresponding to quat.</returns>
    private string _GetLoggingStringFromQuaternion(Quaternion quat)
    {
        if (quat == Quaternion.identity)
        {
            return "N/A";
        }
        else
        {
            return string.Format("{0}, {1}, {2}, {3}",
                                 quat.x.ToString(UI_FLOAT_FORMAT),
                                 quat.y.ToString(UI_FLOAT_FORMAT),
                                 quat.z.ToString(UI_FLOAT_FORMAT),
                                 quat.w.ToString(UI_FLOAT_FORMAT));
        }
    }
    
    /// <summary>
    /// Return a string to the get logging from frame count.
    /// </summary>
    /// <returns>The get logging string from frame count.</returns>
    /// <param name="frameCount">Frame count.</param>
    private string _GetLoggingStringFromFrameCount(int frameCount)
    {
        if (frameCount == -1.0)
        {
            return "N/A";
        }
        else
        {
            return frameCount.ToString();
        }
    }
    
    /// <summary>
    /// Return a string to get logging of FrameDeltaTime.
    /// </summary>
    /// <returns>The get loggin string from frame delta time.</returns>
    /// <param name="frameDeltaTime">Frame delta time.</param>
    private string _GetLogginStringFromFrameDeltaTime(float frameDeltaTime)
    {
        if (frameDeltaTime == -1.0)
        {
            return "N/A";
        }
        else
        {
            return (frameDeltaTime * SECOND_TO_MILLISECOND).ToString(UI_FLOAT_FORMAT);
        }
    }

    public GameObject objPivot;
    public Renderer objPivotRender;
    float f_count = 0;

    bool b_once = true;
    bool b_count = true;

    public Color c_red;
    public Color c_green;
    
    Vector3 planeCenter = Vector3.zero;
    Plane plane;

    float f_uiTimer = 0f;
    bool b_uiTimer = false;

    GameObject objNewUI;


    public void hideUI()
    {
        Destroy(objNewUI);


        //obj_ui.transform.GetChild(0).GetComponent<Animation>().Play("Close");
        //Invoke("DeleteUI", 0.1f);
    }

    void DeleteUI()
    {
        Destroy(objNewUI);
    }
    /*
    public void ShowHeart(bool bState)
    {
        if (bState)
        {
            //Vector3 newVec = planeCenter;
            //newVec.y = 0;
            //planeCenter = newVec;
            
            Instantiate(objHeart, planeCenter, Quaternion.FromToRotation(Vector3.up, plane.normal));                
        }
        else
        { 
        
        }
    }
    */
    public Image imgload;
    public Text texState;

    public void ShowHideScan(bool bState)
    {
        b_uiTimer = bState;
        objPivot.SetActive(bState);
    }

    public Vector3 vecSave;

    /// <summary>
    /// findPlane point
    /// </summary>
    public Vector2 t = new Vector2(575, 595);

    private void _UpdateAutoLocationMarker()
    {
        if (b_uiTimer)
        {
            if (planeCenter != Vector3.zero)
            {
                Vector3 temp = objPivot.transform.position;
                objPivot.transform.position = Vector3.Lerp(temp, planeCenter, 0.1f);
                objPivot.transform.rotation = Quaternion.FromToRotation(Vector3.up, plane.normal);
            }

            f_uiTimer = f_uiTimer + 0.01f;
            imgload.fillAmount = f_uiTimer/1.5f;
            if (f_uiTimer > 1.5f)
            {
                if (planeCenter != Vector3.zero)
                {
                    b_uiTimer = false;

                    Debug.Log(f_uiTimer + "uiAppear!");

                    ShowHideScan(false);
                    
                    f_uiTimer = 0;
                    imgload.fillAmount = 0;
                    objNewUI = Instantiate(obj_ui, planeCenter, Quaternion.FromToRotation(Vector3.up, plane.normal)) as GameObject;
                    //vecSave = objNewUI.transform.position;

                    //vecSave = objNewUI.transform.localPosition;
                    planeOriginPosition = planeCenter;
                    Transform target = FindObjectOfType<CameraTriggerSystem>().transform;
                    //Vector3 dir = target.position - objNewUI.transform.position;
                    Vector3 dir = objNewUI.transform.position - target.position;
                    dir.y = 0.0f;
                    dir.Normalize();

                    objNewUI.transform.rotation = Quaternion.LookRotation(dir);
                    SoundManager.Instance.PlaySound(0);
                                        
                }
            }
            else
            {

            }
        }

        if (b_once)
        {
            f_count = f_count + 0.01f; // 증가
            if (b_count)
            {
                if (f_count > 0.3f) // 초 체크
                {
                    //b_once = false;
                    //b_count = false;
                    f_count = 0; //초기화

                    // Single tap -- place new location or select existing location.
                    
                    Vector2 guiPosition = new Vector2(t.x, Screen.height - t.y);
                    Camera cam = m_arScreen.m_renderCamera;
                    RaycastHit hitInfo;

                    if (m_selectedRect.Contains(guiPosition) || m_hideAllRect.Contains(guiPosition))
                    {
                        // do nothing, the button will handle it
                    }
                    else if (Physics.Raycast(cam.ScreenPointToRay(t), out hitInfo))
                    {
                        // Found a marker, select it (so long as it isn't disappearing)!
                        /*GameObject tapped = hitInfo.collider.gameObject;
                        if (!tapped.GetComponent<Animation>().isPlaying)
                        {
                            m_selectedMarker = tapped.GetComponent<ARLocationMarker>();
                            tDebug += "T : " + t;
                        }
                         */ 
                    }
                    else
                    {
                        // Place a new point at that location, clear selection
                        if (!m_pointCloud.FindPlane(cam, t,
                                                    TAP_PIXEL_TOLERANCE, MIN_PLANE_FIT_PERCENTAGE,
                                                    out planeCenter, out plane))
                        {
                            return;
                        }
                        //Instantiate(m_prefabLocation, planeCenter, Quaternion.FromToRotation(Vector3.up, plane.normal));
                        Vector3 planeVector = plane.normal;
                        if (planeVector.x > 0.2f || planeVector.x < -0.2f || planeVector.z > 0.2f || planeVector.z < -0.2f)
                        {
                            //tDebug += "This Plane is not a Floor.\n";
                            objPivotRender.material.SetColor("_TintColor", c_red);
                            texState.text = "X";
                            f_uiTimer = 0;

                            if (b_onces)
                            {
                                b_onces = false;
                                b_uiTimer = true;
                            }
                        }
                        else
                        {
                            objPivotRender.material.SetColor("_TintColor", c_green);
                            texState.text = "Find";
                            
                        }
                        m_selectedMarker = null;
                    }
                }
            }            
        }
    }

    bool b_onces = true;
    public Vector3 GetPlaneCenterPosition()
    {
        //tDebug += "planeCenter Position: " + planeOriginPosition + "\n";
        return planeOriginPosition;
    }
#if false
    /// <summary>
    /// Update location marker state.
    /// </summary>
    private void _UpdateLocationMarker()
    {
        if (Input.touchCount == 1)
        {
            // Single tap -- place new location or select existing location.
            Touch t = Input.GetTouch(0);
            Vector2 guiPosition = new Vector2(t.position.x, Screen.height - t.position.y);
            Camera cam = m_arScreen.m_renderCamera;
            RaycastHit hitInfo;

            if (t.phase != TouchPhase.Began)
            {
                return;
            }

            if (m_selectedRect.Contains(guiPosition) || m_hideAllRect.Contains(guiPosition))
            {
                // do nothing, the button will handle it
            }
            else if (Physics.Raycast(cam.ScreenPointToRay(t.position), out hitInfo))
            {
                // Found a marker, select it (so long as it isn't disappearing)!
                GameObject tapped = hitInfo.collider.gameObject;
                if (!tapped.GetComponent<Animation>().isPlaying)
                {
                    m_selectedMarker = tapped.GetComponent<ARLocationMarker>();
                    tDebug += "T : " + t.position;
                }
            }
            else
            {
                // Place a new point at that location, clear selection
                Vector3 planeCenter;
                Plane plane;
                if (!m_pointCloud.FindPlane(cam, t.position,
                                            TAP_PIXEL_TOLERANCE, MIN_PLANE_FIT_PERCENTAGE,
                                            out planeCenter, out plane))
                {
                    return;
                }
                Instantiate(m_prefabLocation, planeCenter, Quaternion.FromToRotation(Vector3.up, plane.normal));
                Vector3 planeVector = plane.normal;
                if (planeVector.x > 0.2f || planeVector.x < -0.2f || planeVector.z > 0.2f || planeVector.z < -0.2f)
                {
                    tDebug += "This Plane is not a Floor.\n";
                }
                m_selectedMarker = null;
            }
        }
        if (Input.touchCount == 2)
        {
            // Two taps -- toggle debug text
            Touch t0 = Input.GetTouch(0);
            Touch t1 = Input.GetTouch(1);

            if (t0.phase != TouchPhase.Began && t1.phase != TouchPhase.Began)
            {
                return;
            }

            m_showDebug = !m_showDebug;
            return;
        }

        if (Input.touchCount != 1)
        {
            return;
        }
    }
#endif
    
}
