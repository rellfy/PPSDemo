using System;
using PPS;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class SpawnerProcessor : Processor {

    private WorldProfile profile;
    private WorldSystem system;

    public SpawnerProcessor(WorldSystem system, WorldProfile profile) {
        this.system = system;
        this.profile = profile;
    }

    public override void Update() {
        base.Update();

        SpawnHealthContainers();
        SpawnShooter();
    }

    private Vector3 RandomPositionAroundPlayer(float radius) {
        return this.system.LocalSystem.LocalInstance.Shooter.Profile.Rigidbody.position + new Vector3(Random.Range(-radius, radius), 0.5f, Random.Range(-radius, radius));
    }

    private void SpawnHealthContainers() {
        float timeSinceLastSpawn = Time.timeSinceLevelLoad - this.profile.LastHealthContainerSpawnedSeconds;
        if (timeSinceLastSpawn < 60f/this.system.HealthSpawner.frequencyPM)
            return;

        this.profile.LastHealthContainerSpawnedSeconds = Time.timeSinceLevelLoad;

        GameObject healthPotion = UnityEngine.Object.Instantiate(this.system.HealthPotion, RandomPositionAroundPlayer(120f), Quaternion.identity);
        // Add CollisionDelegate component from PPS and listen to TriggerEnter events.
        healthPotion.AddComponent<CollisionDelegate>().TriggerEnter += (sender, collider) => {
            if (!this.system.WorldInstance.FindShooter(collider.transform.parent, out ShooterProcessor processor))
                return;

            processor.Profile.Health = Mathf.Clamp(processor.Profile.Health + 30f, 0f, 100f);
            UnityEngine.Object.Destroy(healthPotion);
        };
    }

    private void SpawnShooter() {
        float timeSinceLastSpawn = Time.timeSinceLevelLoad - this.profile.LastShooterSpawnedSeconds;
        if (timeSinceLastSpawn < 60f/this.system.EnemySpawner.frequencyPM || this.system.EnemySpawner.maxObjects <= this.system.LocalSystem.Instances.Count)
            return;

        this.profile.LastShooterSpawnedSeconds = Time.timeSinceLevelLoad;

        ShooterProcessor shooter = this.system.ShooterSystem.DeployInstance();
        shooter.Profile.Rigidbody.position = RandomPositionAroundPlayer(120f);
    }
}