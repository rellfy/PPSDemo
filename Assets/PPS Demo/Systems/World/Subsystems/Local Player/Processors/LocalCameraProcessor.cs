using System;
using UnityEngine;
using PPS;

/// <summary>
/// IMPORTANT: Do not serialize SubProcessors that contain a serialized 'SubProfile', or Unity
/// will throw an alert. This is the case as LocalCameraProfile is already serialized in LocalProfile.
/// </summary>
public class LocalCameraProcessor : Processor<LocalSystem, LocalCameraProfile> {

    private ShooterProcessor shooter;

    public LocalCameraProcessor(LocalSystem system, LocalCameraProfile profile, ShooterProcessor shooter) : base(system, profile) {
        this.shooter = shooter;
        InitialiseCamera();
        ToggleMouseLock();
    }

    private void InitialiseCamera() {
        Profile.Camera = new GameObject("LocalCamera").AddComponent<Camera>();
        Profile.Camera.transform.parent = this.shooter.Profile.Head;
    }

    public override void Update() {
        base.Update();

        if (Input.GetKeyDown(KeyCode.Escape))
            ToggleMouseLock();
    }

    public override void LateUpdate() {
        if (Cursor.lockState != CursorLockMode.None) {
            Profile.x += Input.GetAxis("Mouse X") * Profile.xSpeed;
            Profile.y -= Input.GetAxis("Mouse Y") * Profile.ySpeed;
            Profile.y = ClampAngle(Profile.y, Profile.yMinLimit, Profile.yMaxLimit);
        }

        Quaternion rotation = Quaternion.Euler(Profile.y, Profile.x, 0);
        Vector3 offsetTarget = this.shooter.Profile.CameraPoint.transform.position;

        if (Physics.Linecast(offsetTarget, Profile.Camera.transform.position, out RaycastHit hit)) {
            Profile.distance -= hit.distance;
            Profile.totalFallbackDistance += hit.distance;
        }

        if (Profile.totalFallbackDistance > 0) {
            float fallback = Profile.totalFallbackDistance * Profile.fallbackSpeed * Time.deltaTime;
            Profile.distance += fallback;
            Profile.totalFallbackDistance -= fallback;
        }

        Vector3 negDistance = new Vector3(0.0f, 0.0f, -Profile.distance);
        Vector3 position = rotation * negDistance + offsetTarget;

        Profile.Camera.transform.rotation = rotation;
        Profile.Camera.transform.position = Vector3.Lerp(Profile.Camera.transform.position, position, 1 / Profile.smoothFactor);
    }

    public static float ClampAngle(float angle, float min, float max) {
        if (angle < -360f)
            angle += 360f;

        if (angle > 360f)
            angle -= 360f;

        return Mathf.Clamp(angle, min, max);
    }

    private void ToggleMouseLock() {
        if (Cursor.lockState == CursorLockMode.Locked) {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        } else {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}