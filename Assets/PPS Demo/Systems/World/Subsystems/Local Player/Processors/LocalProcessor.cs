using System;
using UnityEngine;
using PPS;
using UnityEngine.SceneManagement;

[Serializable]
public class LocalProcessor : Processor<LocalSystem, LocalProfile> {

    [SerializeField]
    private LocalProfile localProfile;
    [SerializeField]
    private ShooterProcessor shooter;
    private WorldSystem parentSystem;

    public ShooterProcessor Shooter => this.shooter;

    public LocalProcessor(LocalSystem system, LocalProfile profile) : base(system, profile) {
        this.localProfile = profile;
        this.parentSystem = (WorldSystem)System.Parent;
        this.shooter = this.parentSystem.ShooterSystem.DeployInstance();
        this.shooter.Profile.IsAI = false;
        this.shooter.Profile.Rigidbody.position = new Vector3(50f, 0.5f, 50f);
        
        SubProcessors.Add(new LocalCameraProcessor(system, profile.CameraProfile, this.shooter));

        this.shooter.Dead += OnShooterDead;
    }

    private void OnShooterDead(object sender, ShooterProcessor e) {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public override void Update() {
        base.Update();

        if (Input.GetMouseButtonDown(0))
            this.shooter.Shoot();
    }

    public override void FixedUpdate() {
        base.FixedUpdate();

        Vector3 torqueVector = Vector3.zero;

        if (Input.GetKey(KeyCode.W))
            torqueVector += Profile.CameraProfile.Camera.transform.right;

        if (Input.GetKey(KeyCode.S))
            torqueVector -= Profile.CameraProfile.Camera.transform.right;

        if (Input.GetKey(KeyCode.A))
            torqueVector += Profile.CameraProfile.Camera.transform.forward;

        if (Input.GetKey(KeyCode.D))
            torqueVector -= Profile.CameraProfile.Camera.transform.forward;

        this.shooter.Aim(Profile.CameraProfile.Camera.transform.forward);
        this.shooter.RotationalMovement(torqueVector * this.shooter.System.MoveSpeed);
    }
}