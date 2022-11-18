using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CameraMove : MonoBehaviour
{
    private bool clicked_;
    Vector3 lastPosition = Vector3.zero;
    Vector2 lastTouchPosition = Vector2.zero;
    public float moveSpeed;
    public float MaxLimitX;
    public float MinLimitX;
    public float MaxLimitZ;
    public float MinLimitZ;
    private Camera thisCamera;
    private float touchDist = 0;
    private float lastDist = 0;
    public float MaxZoom;
    public float MinZoom;
    [Range (1f, 50f)] public float zoomSpeed;
    private void Awake () {
        thisCamera = GetComponent<Camera> ();
        DebugPanel.Instance.SliderValueChanged += SetZoomSpeed;
    }

    // Start is called before the first frame update
    void Start() {
        DebugPanel.Instance.Change_Attr_2_Value(""+touchDist);
        DebugPanel.Instance.Change_Attr_3_Value(""+lastDist);
        DebugPanel.Instance.Change_Attr_1_Value(""+thisCamera.fieldOfView);
        DebugPanel.Instance.ChangeSliderValue (zoomSpeed, 1f, 50f);
    }

    // Update is called once per frame
    void Update() {
        if(!GameController.Instance.IsGameRunning()) return;
        //only move for z and x axis
#if  UNITY_EDITOR
        if (Input.GetMouseButtonDown(0)) {
            if (!clicked_) clicked_ = true;
            if (lastPosition == Vector3.zero) {
                lastPosition = Input.mousePosition;
                return;
            }

        } else if (Input.GetMouseButtonUp(0)) {
            clicked_ = false;
            lastPosition = Vector3.zero;
        }

        if (clicked_) {
            if(lastPosition != Vector3.zero) {
                Vector2 Direction = (lastPosition - Input.mousePosition) * 0.5f;
                float xValue = transform.position.x + (Direction.x / 10);
                float zValue = transform.position.z + (Direction.y / 10);
                if(zValue > MaxLimitZ || zValue < MinLimitZ) {
                    zValue = transform.position.z;
                }
                if (xValue > MaxLimitX || xValue < MinLimitX) {
                    xValue = transform.position.x;
                } 
                
                this.transform.position = new Vector3(xValue, transform.position.y, zValue);
                lastPosition = Input.mousePosition;
            }
            
        }
        
            
#elif UNITY_ANDROID

        if(Input.touchCount == 1 && Input.touches[0].phase == TouchPhase.Began) {
            clicked_ = true;
            if (lastTouchPosition == Vector2.zero) {
                lastTouchPosition = Input.touches[0].position;
                return;
            }
        }else if(Input.touchCount == 2){
             Touch touch1 = Input.GetTouch(0);
            Touch touch2 = Input.GetTouch(1);
     
            if (touch1.phase == TouchPhase.Began && touch2.phase == TouchPhase.Began)
            {
                lastDist = Vector2.Distance(touch1.position, touch2.position);
            }
     
            if (touch1.phase == TouchPhase.Moved && touch2.phase == TouchPhase.Moved)
            {
                float newDist = Vector2.Distance(touch1.position, touch2.position);
                if (lastDist < 0.5f) {
                    lastDist = newDist;
                    return;
                }

                float tempView = thisCamera.fieldOfView;

                if (lastDist < newDist) {
                    tempView = thisCamera.fieldOfView + (zoomSpeed * Time.deltaTime);
                } else {
                    tempView = thisCamera.fieldOfView - (zoomSpeed * Time.deltaTime);
                }
                
                touchDist = lastDist - newDist;
                lastDist = newDist;
                
                DebugPanel.Instance.Change_Attr_2_Value(""+touchDist);
                DebugPanel.Instance.Change_Attr_3_Value(""+lastDist);
                // Your Code Here
                
                if (tempView < MaxZoom && tempView > MinZoom) {
                    thisCamera.fieldOfView = tempView;
                }
                DebugPanel.Instance.Change_Attr_1_Value(""+thisCamera.fieldOfView);
            }

            if (touch1.phase == TouchPhase.Ended || touch2.phase == TouchPhase.Ended) {
                lastDist = 0f;
            }
        }

        if(Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Ended) {
            clicked_ = false;
            lastTouchPosition = Vector2.zero;
        }

        if (clicked_) {
            if (lastTouchPosition != Vector2.zero) {

                Vector2 Direction = (lastTouchPosition - Input.touches[0].position) * 0.5f;
                Debug.Log(Direction);
                float xValue = transform.position.x + (Direction.x / 10);
                float zValue = transform.position.z + (Direction.y / 10);
                if (zValue > MaxLimitZ || zValue < MinLimitZ) {
                    zValue = transform.position.z;
                }
                if (xValue > MaxLimitX || xValue < MinLimitX) {
                    xValue = transform.position.x;
                }

                this.transform.position = new Vector3(xValue, transform.position.y, zValue);
                lastTouchPosition = Input.touches[0].position;
            }

        }
        
#endif

    }

    private void IsExceedingLimit() {
        
    }

    public void SetZoomSpeed () {
        zoomSpeed = DebugPanel.Instance.GetZoomSliderValue (1f, 50f);
    }
}
