﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoblieInput : MonoBehaviour
{

    private const float DEADZONE = 100.0f;
    public static MoblieInput Instance { set; get; }

    private bool tap, swipeLeft, swipeRight, swipeUp, swipeDown;

    private Vector2 swipeDelta, startTouch;

    public bool Tap { get { return tap; } }
    public bool SwipeLeft { get { return swipeLeft; } }
    public bool SwipeRight { get { return swipeRight; } }
    public bool SwipeUp { get { return swipeUp; } }
    public bool SwipeDown { get { return swipeDown; } }
    public Vector2 SwipeDelta { get { return swipeDelta; } }


    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        //Resetting all Bools
        tap = swipeLeft = swipeRight = swipeUp = swipeDown = false;


        // Check for Inputs

        #region Standalone Inputs
        if (Input.GetMouseButtonDown(0))
        {
            tap = true;
            startTouch = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            startTouch = swipeDelta = Vector2.zero;
        }
        #endregion

        #region Moblie Inputs
        if (Input.touches.Length != 0)
        {
            if (Input.touches[0].phase == TouchPhase.Began)
            {
                tap = true;
                startTouch = Input.mousePosition;
            }
            else if (Input.touches[0].phase == TouchPhase.Ended || Input.touches[0].phase == TouchPhase.Canceled)
            {
                startTouch = swipeDelta = Vector2.zero;
            }
        }

        #endregion

        //Calculate Distance
        swipeDelta = Vector2.zero;
        if (startTouch != Vector2.zero)
        {
            //Mobile
            if (Input.touches.Length != 0)
            {
                swipeDelta = Input.touches[0].position - startTouch;
            }
            //PC
            else if (Input.GetMouseButton(0))
            {
                swipeDelta = (Vector2)Input.mousePosition - startTouch;
            }
        }

        //Deadzone check
        if (swipeDelta.magnitude > DEADZONE)
        {
            //Confirmed Swipe
            float x = swipeDelta.x;
            float y = swipeDelta.y;

            if (Mathf.Abs(x) > Mathf.Abs(y))
            {
                //L/R
                if (x < 0)
                    swipeLeft = true;
                else
                    swipeRight = true;
            }
            else
            {
                //U/D
                if (y < 0)
                    swipeDown = true;    
                else
                      swipeUp= true;
            }

            startTouch = swipeDelta = Vector2.zero;
        }
    }
}