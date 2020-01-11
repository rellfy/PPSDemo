using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using PPS;

[Serializable]
public class WorldSystem : System<WorldProcessor> {

    [Serializable]
    public struct Spawner {
        public int frequencyPM;
        public int maxObjects;
    }

    [SerializeField]
    private Spawner enemySpawner;
    [SerializeField]
    private Spawner healthSpawner;
    /// <summary>
    /// World system only has one instance.
    /// </summary>
    [SerializeField]
    private WorldProcessor worldInstance;
    [SerializeField]
    private GameObject healthPotion;
    [SerializeField]
    private ShooterSystem shooterSystem;
    [SerializeField]
    private LocalSystem localSystem;

    public Spawner EnemySpawner => this.enemySpawner;
    public Spawner HealthSpawner => this.healthSpawner;
    public WorldProcessor WorldInstance => this.worldInstance;
    public GameObject HealthPotion => this.healthPotion;
    public ShooterSystem ShooterSystem => this.shooterSystem;
    public LocalSystem LocalSystem => this.localSystem;

    /// <summary>
    /// Unity 2019 does not serialize generics. For that reason, we convert the generic
    /// to the specific type that we want to serialize here.
    /// </summary>
    protected override void UpdateSerializableInstances(object sender, Type instanceType) {
        this.worldInstance = this.instances[0];
    }

    protected override void DeploySubsystems() {
        DeploySubsystem<ShooterSystem, ShooterProcessor>(ref this.shooterSystem);
        DeploySubsystem<LocalSystem, LocalProcessor>(ref this.localSystem);
    }
}