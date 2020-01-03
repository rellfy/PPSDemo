using System;
using PPS;

public class ScoreProcessor : Processor<UISystem, UIProfile> {

    public ScoreProcessor(UISystem system, UIProfile profile) : base(system, profile) {
        foreach (ShooterProcessor shooterProcessor in this.system.WorldSystem.ShooterSystem.Instances) {
            shooterProcessor.Dead += OnShooterDead;
        }

        this.system.WorldSystem.ShooterSystem.InstanceDeployed += OnShooterDeployed;
        UpdateEnemyCount();
    }

    private void OnShooterDeployed(object sender, Type instanceType) {
        ShooterProcessor shooter = (ShooterProcessor)sender;
        shooter.Dead += OnShooterDead;
        UpdateEnemyCount();
    }

    private void OnShooterDead(object sender, ShooterProcessor killer) {
        bool localScore = killer == this.system.WorldSystem.LocalSystem.LocalInstance.Shooter;

        if (localScore) {
            this.profile.AddLocalScore(10);
        } else {
            this.profile.AddEnemyScore(1);
        }

        UpdateEnemyCount();
    }

    private void UpdateEnemyCount() {
        int count = this.system.WorldSystem.ShooterSystem.Instances.Count - 1;
        this.profile.SetEnemyScoreLabel($"ENEMIES [{count}] SCORE:");
    }
}