using System;
using System.Collections.Generic;
using PPS;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class WorldProcessor : Processor<WorldSystem> {

    private WorldProfile profile;

    public WorldProcessor(WorldSystem system, GameObject instance) : base(system, instance) {
        this.profile = new WorldProfile(GameObject);

        SubProcessors.Add(new SpawnerProcessor(system, this.profile));
    }

    public bool FindShooter(Transform transform, out ShooterProcessor processor) {
        foreach (ShooterProcessor shooterProcessor in System.ShooterSystem.Instances) {
            if (shooterProcessor.Profile.Transform != transform)
                 continue;

            processor = shooterProcessor;
            return true;
        }

        processor = null;
        return false;
    }
}