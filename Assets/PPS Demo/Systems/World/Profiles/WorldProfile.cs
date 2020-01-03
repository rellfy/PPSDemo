using System;
using System.Collections.Generic;
using UnityEngine;
using PPS;

[Serializable]
public class WorldProfile : Profile {

    private float lastHealthContainerSpawnedSeconds;
    private float lastShooterSpawnedSeconds;

    public float LastHealthContainerSpawnedSeconds {
        get => this.lastHealthContainerSpawnedSeconds;
        set => this.lastHealthContainerSpawnedSeconds = value;
    }
    public float LastShooterSpawnedSeconds {
        get => this.lastShooterSpawnedSeconds;
        set => this.lastShooterSpawnedSeconds = value;
    }

    public WorldProfile(GameObject gameObject) : base(gameObject) { }
}