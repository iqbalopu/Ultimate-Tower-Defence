using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    //Touch Input Controlls
    private Vector3 startPos;
    private bool isTouch;
    public float minSwipeDistX;
    public float minSwipeDistY;
    private bool isSwipe;
    private bool isJump;
    private bool mouseDown_;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_ANDROID
        if (Input.touchCount > 0) {
            Touch touch = Input.touches[0];
            switch (touch.phase) {
                case TouchPhase.Began:
                    startPos = touch.position;
                    break;
                case TouchPhase.Moved:
                    isTouch = true;
                    float swipeDistHorizontal = (new Vector3(touch.position.x, 0, 0) - new Vector3(startPos.x, 0, 0)).magnitude;
                    float swipeDistVertical = (new Vector3(0, touch.position.y, 0) - new Vector3(0, startPos.y, 0)).magnitude;
                    if (swipeDistHorizontal > minSwipeDistX) {
                        float swipeValue = Mathf.Sign(touch.position.x - startPos.x);
                        if (swipeValue > 0 && !isSwipe) {//to right swipe
                            isTouch = false;
                            isSwipe = true;
                            Debug.Log("Right");
                            RightSwipe();
                        } else if (swipeValue < 0 && !isSwipe) {////to left swipe
                            isTouch = false;
                            isSwipe = true;
                            Debug.Log("Left");
                            LefttSwipe();
                        }
                    }
                    // add swipe to up
                    if (swipeDistVertical > minSwipeDistY) {
                        float swipeValueY = Mathf.Sign(touch.position.y - startPos.y);
                        if (swipeValueY > 0 && !isSwipe) {
                            isTouch = false;
                            isSwipe = true;
                            Debug.Log("Up");
                            UpSwipe();
                        }else if(swipeValueY < 0 && !isSwipe) {
                            isTouch = false;
                            isSwipe = true;
                            Debug.Log("Down");
                            DownSwipe();
                        }
                    }
                    break;
                case TouchPhase.Stationary:
                    isJump = true;
                    break;
                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    isTouch = false;
                    isSwipe = false;
                    break;
                default:
                    break;
            }
        }
#elif UNITY_EDITOR

        if (Input.GetMouseButton(0)) {
            mouse
        } else if (Input.GetMouseButtonUp(0)) {

        }
        
#endif

        //if (Input.GetMouseButton(0)) {
        //    if (mouseDown_) {
        //        Vector3 mousePos = Input.mousePosition;
        //        isTouch = true;
        //        float swipeDistHorizontal = (new Vector3(mousePos.x, 0, 0) - new Vector3(startPos.x, 0, 0)).magnitude;
        //        float swipeDistVertical = (new Vector3(0, mousePos.y, 0) - new Vector3(0, startPos.y, 0)).magnitude;
        //        transform.Translate(transform.position.x+swipeDistHorizontal, transform.position.y+swipeDistVertical, transform.position.z);

        //        if (swipeDistHorizontal > minSwipeDistX) {
        //            float swipeValue = Mathf.Sign(mousePos.x - startPos.x);
        //            if (swipeValue > 0 && !isSwipe) {//to right swipe
        //                isTouch = false;
        //                isSwipe = true;
        //                Debug.Log("Right");
        //                RightSwipe();
        //            } else if (swipeValue < 0 && !isSwipe) {////to left swipe
        //                isTouch = false;
        //                isSwipe = true;
        //                Debug.Log("Left");
        //                LefttSwipe();
        //            }
        //        }
        //        // add swipe to up
        //        if (swipeDistVertical > minSwipeDistY) {
        //            float swipeValueY = Mathf.Sign(mousePos.y - startPos.y);
        //            if (swipeValueY > 0 && !isSwipe) {
        //                isTouch = false;
        //                isSwipe = true;
        //                Debug.Log("Up");
        //                UpSwipe();
        //            } else if (swipeValueY < 0 && !isSwipe) {
        //                isTouch = false;
        //                isSwipe = true;
        //                Debug.Log("Down");
        //                DownSwipe();
        //            }
        //        }
        //    } else {
        //        mouseDown_ = true;
        //        startPos = Input.mousePosition;
        //        isTouch = false;
        //        isSwipe = false;
        //    }
        //} else if (Input.GetMouseButtonUp(0)) {
        //    mouseDown_ = false;
        //    isTouch = false;
        //    isSwipe = false;
        //}
    }

    private void RightSwipe() {
        //
    }

    private void LefttSwipe() {
        //
    }

    private void UpSwipe() {
        //
    }
    private void DownSwipe() {

    }


}
