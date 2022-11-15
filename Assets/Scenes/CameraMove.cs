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
    // Start is called before the first frame update
    void Start() {

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

        if(Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began) {
            clicked_ = true;
            if (lastTouchPosition == Vector2.zero) {
                lastTouchPosition = Input.touches[0].position;
                return;
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

    
    //private Vector3 Origin;
    //private Vector3 Difference;
    //private Vector3 ResetCamera;

    //private bool drag = false;



    //private void Start() {
    //    ResetCamera = this.transform.position;
    //}


    //private void Update() {
    //    if (Input.GetMouseButton(0)) {
    //        //Debug.Log("Calling");
    //        Difference = Input.mousePosition;
    //        if (drag == false) {
    //            drag = true;
    //            Origin = Input.mousePosition;
    //        }
    //    } else {
    //        drag = false;
    //    }

    //    if (drag) {
    //        Vector3 Temp = Origin - Difference * 0.5f;
    //        Debug.Log(Temp);
    //        this.transform.position = new Vector3(Temp.x, transform.position.y, Temp.y);
    //    }

    //    if (Input.GetMouseButton(1)) {
    //        this.transform.position = ResetCamera;
    //    }

    //}
}
