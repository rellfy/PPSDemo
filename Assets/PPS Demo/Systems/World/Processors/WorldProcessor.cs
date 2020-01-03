using System;
using System.Collections.Generic;
using PPS;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class WorldProcessor : Processor<WorldSystem, WorldProfile> {

    public WorldProcessor(WorldSystem system, WorldProfile profile) : base(system, profile) {
        this.subProcessors.Add(new SpawnerProcessor(system, profile));
    }

    public bool FindShooter(Transform transform, out ShooterProcessor processor) {
        foreach (ShooterProcessor shooterProcessor in this.system.ShooterSystem.Instances) {
            if (shooterProcessor.Profile.Transform != transform)
                 continue;

            processor = shooterProcessor;
            return true;
        }

        processor = null;
        return false;
    }
}