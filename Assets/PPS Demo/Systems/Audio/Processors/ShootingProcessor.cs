using System;
using PPS;
using UnityEngine;

public class ShootingProcessor : Processor {

    private AudioProfile profile;
    private AudioSystem system;

    public ShootingProcessor(AudioSystem system, AudioProfile profile) {
        this.system = system;
        this.profile = profile;

        foreach (ShooterProcessor shooterSystemInstance in this.system.WorldSystem.ShooterSystem.Instances) {
            shooterSystemInstance.Fire += OnShooterFire;
        }

        this.system.WorldSystem.ShooterSystem.InstanceDeployed += OnShooterDeployed;
    }

    private void OnShooterDeployed(object sender, Type instanceType) {
        ShooterProcessor shooter = (ShooterProcessor)sender;
        shooter.Fire += OnShooterFire;
    }

    private void OnShooterFire(object sender, EventArgs e) {
        AudioSource source = (AudioSource)sender;
        source.PlayOneShot(this.system.BlasterClip, 1f);
    }
}