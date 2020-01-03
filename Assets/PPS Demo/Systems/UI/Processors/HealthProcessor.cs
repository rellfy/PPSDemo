using System;
using PPS;

public class HealthProcessor : Processor<UISystem, UIProfile> {

    public HealthProcessor(UISystem system, UIProfile profile) : base(system, profile) {
        this.system.WorldSystem.LocalSystem.LocalInstance.Shooter.Profile.HealthChange += OnShooterHealthChange;
    }

    private void OnShooterHealthChange(object sender, float newHealth) {
        this.profile.SetHealth(newHealth / 100f);
    }
}