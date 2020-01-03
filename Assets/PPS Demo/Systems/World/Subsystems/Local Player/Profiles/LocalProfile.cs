using System;
using UnityEngine;
using PPS;

[Serializable]
public class LocalProfile : Profile {

    [SerializeField]
    private LocalCameraProfile cameraProfile;
    public LocalCameraProfile CameraProfile => this.cameraProfile;

    public LocalProfile(GameObject gameObject) : base(gameObject) {
        this.cameraProfile = new LocalCameraProfile(gameObject);
    }
}