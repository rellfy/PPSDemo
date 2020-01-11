using System;
using PPS;

public class HealthProcessor : Processor {

    private UISystem system;
    private UIProfile profile;

    public HealthProcessor(UISystem system, UIProfile profile) {
        this.system = system;
        this.profile = profile;
        this.system.WorldSystem.LocalSystem.LocalInstance.Shooter.Profile.HealthChange += OnShooterHealthChange;
    }

    private void OnShooterHealthChange(object sender, float newHealth) {
        this.profile.SetHealth(newHealth / 100f);
    }
}