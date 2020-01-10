using System;
using PPS;

public class ScoreProcessor : Processor<UISystem, UIProfile> {

    public ScoreProcessor(UISystem system, UIProfile profile) : base(system, profile) {
        foreach (ShooterProcessor shooterProcessor in System.WorldSystem.ShooterSystem.Instances) {
            shooterProcessor.Dead += OnShooterDead;
        }

        System.WorldSystem.ShooterSystem.InstanceDeployed += OnShooterDeployed;
        UpdateEnemyCount();
    }

    private void OnShooterDeployed(object sender, Type instanceType) {
        ShooterProcessor shooter = (ShooterProcessor)sender;
        shooter.Dead += OnShooterDead;
        UpdateEnemyCount();
    }

    private void OnShooterDead(object sender, ShooterProcessor killer) {
        bool localScore = killer == System.WorldSystem.LocalSystem.LocalInstance.Shooter;

        if (localScore) {
            Profile.AddLocalScore(10);
        } else {
            Profile.AddEnemyScore(1);
        }

        UpdateEnemyCount();
    }

    private void UpdateEnemyCount() {
        int count = System.WorldSystem.ShooterSystem.Instances.Count - 1;
        Profile.SetEnemyScoreLabel($"ENEMIES [{count}] SCORE:");
    }
}