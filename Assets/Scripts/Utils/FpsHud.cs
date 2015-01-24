﻿using UnityEngine;

public class FpsHud : MonoBehaviour
{
    // Attach this to a GUIText to make a frames/second indicator.
    //
    // It calculates frames/second over each updateInterval,
    // so the display does not keep changing wildly.
    //
    // It is also fairly accurate at very low FPS counts (<10).
    // We do this not by simply counting frames per interval, but
    // by accumulating FPS for each frame. This way we end up with
    // correct overall FPS even if the interval renders something like
    // 5.5 frames.

    public float updateInterval = 0.5F;

    private float accum = 0; // FPS accumulated over the interval
    private int frames = 0; // Frames drawn over the interval
    private float timeleft; // Left time for current interval
	
    private void Awake()
    {
        useGUILayout = false;
        _text.font.RequestCharactersInTexture("0123456789.FPS", _text.fontSize, _text.fontStyle);
    }

    private void Start()
    {
        timeleft = updateInterval;
    }

    private void Update()
    {
        timeleft -= Time.deltaTime;
        accum += Time.timeScale / Time.deltaTime;
        ++frames;

        // Interval ended - update GUI text and start new interval
        if (timeleft <= 0.0)
        {
            // display two fractional digits (f2 format)
            float fps = accum / frames;
            _stringBuilder.Length = 0;
            _stringBuilder.Append(fps.ToString("F2"));
            _stringBuilder.Append(FPS_NAME);
            _text.text = _stringBuilder.ToString();

            if (fps < 30)
            {
                _text.renderer.material.color = Color.yellow;
            }
            else
            {
                if (fps < 10)
                {
                    _text.renderer.material.color = Color.red;
                }
                else
                {
                    _text.renderer.material.color = Color.green;
                }

                timeleft = updateInterval;
                accum = 0.0F;
                frames = 0;
            }
        }
    }

    [SerializeField]
    private TextMesh _text;

    private System.Text.StringBuilder _stringBuilder = new System.Text.StringBuilder();
    private const string FPS_NAME = " FPS";
}