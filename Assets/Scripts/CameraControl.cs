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
    private bool clicked_;
    Vector3 lastPosition = Vector3.zero;
    public float moveSpeed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR
        //if (Input.GetMouseButtonDown(0)) {
        //    if (!clicked_) clicked_ = true;
        //    if (lastPosition == Vector3.zero) {
        //        lastPosition = Input.mousePosition;
        //        return;
        //    }

        //} else if (Input.GetMouseButtonUp(0)) {
        //    clicked_ = false;
        //    lastPosition = Vector3.zero;
        //}

        //if (clicked_) {
        //    if (lastPosition != Vector3.zero) {
        //        Vector3 Direction = (lastPosition - Input.mousePosition) * 0.5f;
        //        float differenceX = lastPosition.x - Input.mousePosition.x;
        //        float differenceZ = lastPosition.x - Input.mousePosition.x;
        //        Debug.Log(Direction);
        //        this.transform.position = new Vector3(transform.position.x + (Direction.x / 10), transform.position.y, transform.position.z + (Direction.y / 10));
        //        lastPosition = Input.mousePosition;
        //    }
        //}
#elif UNITY_ANDROID
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
                    //isJump = true;
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
        
#endif

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
