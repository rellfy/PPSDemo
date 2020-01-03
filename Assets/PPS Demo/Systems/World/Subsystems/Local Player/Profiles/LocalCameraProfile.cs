using System;
using UnityEngine;
using PPS;

[Serializable]
public class LocalCameraProfile : Profile {

    [Header("Target")]
    public float distance = 5.0f;
    public float fallbackSpeed = 10f;
    public float smoothFactor = 0.5f;
    public float totalFallbackDistance;
    public float x;
    public float y;
    [Header("Speed")]
    public float xSpeed = 1.0f;
    public float yMaxLimit = 80f;
    [Header("Limits and Smoothing")]
    public float yMinLimit = -20f;
    public float ySpeed = 1.0f;
    [Header("Camera")]
    private Camera camera;

    public Camera Camera {
        get => this.camera;
        set => this.camera = value;
    }

    public LocalCameraProfile(GameObject gameObject) : base(gameObject) { }
}