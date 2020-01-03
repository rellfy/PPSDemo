using System;
using PPS;
using UnityEngine;

public class ShootingProcessor : Processor<AudioSystem, AudioProfile> {

    public ShootingProcessor(AudioSystem system, AudioProfile profile) : base(system, profile) {
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