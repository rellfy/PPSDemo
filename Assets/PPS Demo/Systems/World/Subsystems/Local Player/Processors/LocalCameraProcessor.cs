using UnityEngine;
using PPS;

public class LocalCameraProcessor : Processor {

    private LocalCameraProfile profile;
    private ShooterProcessor shooter;

    public LocalCameraProcessor(LocalCameraProfile profile, ShooterProcessor shooter) {
        this.profile = profile;
        this.shooter = shooter;
        InitialiseCamera();
        ToggleMouseLock();
    }

    private void InitialiseCamera() {
        this.profile.Camera = new GameObject("LocalCamera").AddComponent<Camera>();
        this.profile.Camera.transform.parent = this.shooter.Profile.Head;
    }

    public override void Update() {
        base.Update();

        if (Input.GetKeyDown(KeyCode.Escape))
            ToggleMouseLock();
    }

    public override void LateUpdate() {
        if (Cursor.lockState != CursorLockMode.None) {
            this.profile.x += Input.GetAxis("Mouse X") * this.profile.xSpeed;
            this.profile.y -= Input.GetAxis("Mouse Y") * this.profile.ySpeed;
            this.profile.y = ClampAngle(this.profile.y, this.profile.yMinLimit, this.profile.yMaxLimit);
        }

        Quaternion rotation = Quaternion.Euler(this.profile.y, this.profile.x, 0);
        Vector3 offsetTarget = this.shooter.Profile.CameraPoint.transform.position;

        if (Physics.Linecast(offsetTarget, this.profile.Camera.transform.position, out RaycastHit hit)) {
            this.profile.distance -= hit.distance;
            this.profile.totalFallbackDistance += hit.distance;
        }

        if (this.profile.totalFallbackDistance > 0) {
            float fallback = this.profile.totalFallbackDistance * this.profile.fallbackSpeed * Time.deltaTime;
            this.profile.distance += fallback;
            this.profile.totalFallbackDistance -= fallback;
        }

        Vector3 negDistance = new Vector3(0.0f, 0.0f, -this.profile.distance);
        Vector3 position = rotation * negDistance + offsetTarget;

        this.profile.Camera.transform.rotation = rotation;
        this.profile.Camera.transform.position = Vector3.Lerp(this.profile.Camera.transform.position, position, 1 / this.profile.smoothFactor);
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