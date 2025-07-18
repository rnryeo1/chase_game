﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class VariableJoystick : Joystick
{
    public float MoveThreshold { get { return moveThreshold; } set { moveThreshold = Mathf.Abs(value); } }

    [SerializeField] private float moveThreshold = 1;
    [SerializeField] private JoystickType joystickType = JoystickType.Fixed;

    private Vector2 fixedPosition = Vector2.zero;

    public void SetMode(JoystickType joystickType)
    {
        this.joystickType = joystickType;
        if (joystickType == JoystickType.Fixed)
        {
            background.anchoredPosition = fixedPosition;
            background.gameObject.SetActive(true);
        }
        else
        {
            background.gameObject.SetActive(true);
        }
    }

    protected override void Start()
    {
        base.Start();

        fixedPosition = background.anchoredPosition;
        SetMode(joystickType);
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (joystickType != JoystickType.Fixed)
        {
            background.anchoredPosition = ScreenPointToAnchoredPosition(eventData.position);
            fixedPosition = background.anchoredPosition;
            background.gameObject.SetActive(true);
            // if (background.position.x < 2 && background.position.y < 2)
            // {

            //     fixedPosition = new Vector2(55.0f, 25.0f);
            //     background.anchoredPosition = fixedPosition;
            // }
        }
        base.OnPointerDown(eventData);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        if (joystickType != JoystickType.Fixed)
        {
            background.gameObject.SetActive(true);
            //background.anchoredPosition = fixedPosition;
        }

        // if (background.position.x < 2 && background.position.y < 2)
        // {
        //     Debug.Log("확인");
        //     fixedPosition = new Vector2(55.0f, 25.0f);
        //     background.anchoredPosition = fixedPosition;
        // }
        base.OnPointerUp(eventData);
    }

    protected override void HandleInput(float magnitude, Vector2 normalised, Vector2 radius, Camera cam)
    {
        if (joystickType == JoystickType.Dynamic && magnitude > moveThreshold)
        {
            Vector2 difference = normalised * (magnitude - moveThreshold) * radius;
            background.anchoredPosition += difference;

            //fixedPosition = new Vector2(55.0f, 25.0f);
            background.anchoredPosition = fixedPosition;
        }

        base.HandleInput(magnitude, normalised, radius, cam);
    }
}

public enum JoystickType { Fixed, Floating, Dynamic }