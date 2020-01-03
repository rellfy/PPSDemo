using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using PPS;
using Object = System.Object;

[Serializable]
public class ShooterSystem : Subsystem<ShooterProcessor, ShooterProfile> {

    private float bulletSpeed = 50f;
    private float moveSpeed = 5f;
    [SerializeField]
    private LayerMask shooterMask;
    [SerializeField]
    private GameObject bullet;
    /// <summary>
    /// Serializable instance list.
    /// </summary>
    [SerializeField]
    private List<ShooterProcessor> shooterInstances;

    public LayerMask ShooterMask => this.shooterMask;
    public GameObject Bullet => this.bullet;
    public float MoveSpeed => this.moveSpeed;

    /// <summary>
    /// Subsystems are serialized, therefore they are initialised through Awake.
    /// </summary>
    public override void Awake(Transform transform, ISystem parent) {
        base.Awake(transform, parent);
    }

    /// <summary>
    /// Unity 2019 does not serialize generics. For that reason, we convert the generic
    /// to the specific type that we want to serialize here.
    /// </summary>
    protected override void UpdateSerializableInstances(object sender, Type instanceType) {
        this.shooterInstances = this.instances;
    }

    /// <summary>
    /// In a data-oriented-design (DOD) way, update specific processor's logic depending on their state.
    /// </summary>
    public override void Update() {
        base.Update();

        // Reverse loop due to possible Processor disposal.
        for (int i = this.instances.Count; i-- > 0;) {
            ShooterProcessor processor = this.instances[i];
            List<ShooterProfile.BulletTracker> shotsFired = processor.Profile.ShotsFired;
            if (shotsFired.Count > 0)
                UpdateBullets(processor, shotsFired, this.bulletSpeed * Time.deltaTime);
        }
    }

    public static void UpdateBullets(ShooterProcessor processor, List<ShooterProfile.BulletTracker> bullets, float speed) {
        List<ShooterProfile.BulletTracker> toRemove = new List<ShooterProfile.BulletTracker>();

        foreach (ShooterProfile.BulletTracker tracker in bullets) {
            tracker.bullet.position += tracker.direction * speed;

            if (Vector3.Distance(tracker.startPosition, tracker.bullet.position) > speed + 150f) {
                toRemove.Add(tracker);
                continue;
            }

            Vector3 origin = tracker.bullet.transform.position - tracker.bullet.transform.forward;
            if (!Physics.Raycast(origin, tracker.direction, out RaycastHit hit, speed + 1.1f))
                continue;

            processor.OnShotHit(hit, tracker);
            toRemove.Add(tracker);
        }

        foreach (ShooterProfile.BulletTracker tracker in toRemove) {
            UnityEngine.Object.DestroyImmediate(tracker.bullet.gameObject);
            bullets.Remove(tracker);
        }
    }
}