using System;
using System.Collections.Generic;
using PPS;
using UnityEngine;
using Object = System.Object;
using Random = UnityEngine.Random;

[Serializable]
public class SpawnerProcessor : Processor<WorldSystem, WorldProfile> {

    public SpawnerProcessor(WorldSystem system, WorldProfile profile) : base(system, profile) { }

    public override void Update() {
        base.Update();

        SpawnHealthContainers();
        SpawnShooter();
    }

    private Vector3 RandomPositionAroundPlayer(float radius) {
        return System.LocalSystem.LocalInstance.Shooter.Profile.Rigidbody.position + new Vector3(Random.Range(-radius, radius), 0.5f, Random.Range(-radius, radius));
    }

    private void SpawnHealthContainers() {
        float timeSinceLastSpawn = Time.timeSinceLevelLoad - Profile.LastHealthContainerSpawnedSeconds;
        if (timeSinceLastSpawn < 60f/System.HealthSpawner.frequencyPM)
            return;

        Profile.LastHealthContainerSpawnedSeconds = Time.timeSinceLevelLoad;

        GameObject healthPotion = UnityEngine.Object.Instantiate(System.HealthPotion, RandomPositionAroundPlayer(120f), Quaternion.identity);
        // Add CollisionDelegate component from PPS and listen to TriggerEnter events.
        healthPotion.AddComponent<CollisionDelegate>().TriggerEnter += (sender, collider) => {
            if (!System.WorldInstance.FindShooter(collider.transform.parent, out ShooterProcessor processor))
                return;

            processor.Profile.Health = Mathf.Clamp(processor.Profile.Health + 30f, 0f, 100f);
            UnityEngine.Object.Destroy(healthPotion);
        };
    }

    private void SpawnShooter() {
        float timeSinceLastSpawn = Time.timeSinceLevelLoad - Profile.LastShooterSpawnedSeconds;
        if (timeSinceLastSpawn < 60f/System.EnemySpawner.frequencyPM || System.EnemySpawner.maxObjects <= System.ShooterSystem.Instances.Count)
            return;

        Profile.LastShooterSpawnedSeconds = Time.timeSinceLevelLoad;

        ShooterProcessor shooter = System.ShooterSystem.DeployInstance();
        shooter.Profile.Rigidbody.position = RandomPositionAroundPlayer(120f);
    }
}