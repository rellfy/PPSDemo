using System;
using PPS;

public class HealthProcessor : Processor<UISystem, UIProfile> {

    public HealthProcessor(UISystem system, UIProfile profile) : base(system, profile) {
        System.WorldSystem.LocalSystem.LocalInstance.Shooter.Profile.HealthChange += OnShooterHealthChange;
    }

    private void OnShooterHealthChange(object sender, float newHealth) {
        Profile.SetHealth(newHealth / 100f);
    }
}