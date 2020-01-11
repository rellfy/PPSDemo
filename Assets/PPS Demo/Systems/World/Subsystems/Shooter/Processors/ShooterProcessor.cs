using System;
using System.Numerics;
using PPS;
using UnityEngine;
using Object = UnityEngine.Object;
using Quaternion = System.Numerics.Quaternion;
using Random = System.Random;
using Vector3 = UnityEngine.Vector3;

[Serializable]
public class ShooterProcessor : Processor<ShooterSystem> {

    [SerializeField]
    private ShooterProfile profile;
    private WorldSystem worldSystem;

    public ShooterProfile Profile => this.profile;

    public event EventHandler<ShooterProcessor> KillAcquired;
    public event EventHandler<ShooterProcessor> Dead;
    public event EventHandler Fire;

    /// <summary>
    /// ShouldProcess tells a Processor whether to call the "Process" method.
    /// This is a way to have an update method without manually checking the state.
    /// In this case, the Process method is used for AI behaviour.
    /// </summary>
    protected override bool ShouldProcess => this.profile.IsAI;
    /// <summary>
    /// This member tells whether to call Process on FixedUpdate, if ShouldProcess returns true.
    /// </summary>
    protected override bool ProcessOnFixedUpdate => false;

    public ShooterProcessor(ShooterSystem system, GameObject instance) : base(system, instance) {
        this.profile = new ShooterProfile(GameObject);
        this.worldSystem = (WorldSystem)System.Parent;
        Dead += OnDead;
    }

    private void OnDead(object sender, ShooterProcessor e) {
        if (this.profile.IsAI)
            Dispose();
    }

    public override void Dispose() {
        for (int i = this.profile.ShotsFired.Count; i-- > 0;) {
            Object.Destroy(this.profile.ShotsFired[i].bullet.gameObject);
            this.profile.ShotsFired.Remove(this.profile.ShotsFired[i]);
        }

        base.Dispose();
    }

    public override void Update() {
        base.Update();

        // Set head to correct position.
        this.profile.Head.transform.position = this.profile.Rigidbody.transform.position + (Vector3.up * 1.15f);
    }

    /// <summary>
    /// The Process method here is handling AI behaviour.
    /// The best way to handle AI would be to have a separate system for this, but for
    /// the sake of this example the AI logic is embedded in this processor.
    /// </summary>
    protected override void Process() {
        // TODO: Set these as AI system constants.
        const float aiAwarenessRadius = 100f;
        const float idealDistance = 10f;

        Collider[] colliders = Physics.OverlapSphere(this.profile.Rigidbody.position, aiAwarenessRadius, System.ShooterMask);

        if (colliders.Length == 1)
            return;

        float closestDistance = Mathf.Infinity;
        ShooterProcessor closest = null;

        // Find closest shooter.
        for (int i = 0; i < colliders.Length; i++) {
            // There are only two colliders, which are direct child of the main instance transform.
            if (colliders[i].transform.parent == this.profile.Transform)
                continue;

            if (!this.worldSystem.WorldInstance.FindShooter(colliders[i].transform.parent, out ShooterProcessor iteration))
                continue;

            // Important note: Shooter's this.profile.Transform.position is always 0,0,0!
            // this.profile.Rigidbody or this.profile.Head hold the true position.
            float distance = Vector3.Distance(this.profile.Rigidbody.position, iteration.Profile.Rigidbody.position);

            if (distance >= closestDistance)
                continue;

            closestDistance = distance;
            closest = iteration;
        }

        if (closest == null) {
            // Roam.
            this.profile.Rigidbody.velocity = new Vector3(UnityEngine.Random.value, 0, UnityEngine.Random.value).normalized * System.MoveSpeed;
            return;
        }

        Vector3 enemyDirection = (closest.Profile.Rigidbody.position - this.profile.Rigidbody.position).normalized;

        this.profile.Rigidbody.velocity = closestDistance >= idealDistance ?
            enemyDirection.normalized * System.MoveSpeed :
            Vector3.zero;

        // Ready to fire.
        Aim(enemyDirection);
        Shoot();
    }

    public void Aim(Vector3 direction) {
        direction += this.profile.Head.transform.position;
        this.profile.Head.transform.LookAt(new Vector3(direction.x, this.profile.Head.transform.position.y, direction.z));
    }

    // TODO: Further enhance performance with object pooling.
    public void Shoot() {
        float timeSinceLastShot = Time.timeSinceLevelLoad - this.profile.lastShotTimeSeconds;
        if (timeSinceLastShot < (this.profile.IsAI ? 0.25f : 0.01f))
            return;

        Fire?.Invoke(this.profile.AudioSource, null);
        this.profile.lastShotTimeSeconds = Time.timeSinceLevelLoad; 
        Transform bullet = Object.Instantiate(System.Bullet, this.profile.ShootPoint.position, this.profile.Head.transform.rotation).GetComponent<Transform>();
        this.profile.ShotsFired.Add(new ShooterProfile.BulletTracker(bullet, this.profile.Head.transform.forward));
    }

    public void RotationalMovement(Vector3 torque) {
        this.profile.Rigidbody.AddTorque(torque, ForceMode.Acceleration);
    }

    /// <summary>
    /// Returns true if the attack killed the Shooter.
    /// </summary>
    public bool Attack(float damage, ShooterProcessor attacker) {
        this.profile.Health -= damage;
        bool dead = this.profile.Health <= 0;

        if (dead)
            Dead?.Invoke(this, attacker);

        return dead;
    }

    /// <summary>
    /// Handles logic after hitting something from a bullet.
    /// Tracker is there because it could carry extra needed information such as the weapon used.
    /// </summary>
    public void OnShotHit(RaycastHit hit, ShooterProfile.BulletTracker tracker) {
        if (!this.worldSystem.WorldInstance.FindShooter(hit.transform.parent, out ShooterProcessor shooter))
            // Do nothing if hit did not land on another Shooter.
            return;

        const float regularAttackDamage = 15f;
        bool dead = shooter.Attack(this.profile.IsAI ? regularAttackDamage / 3f : regularAttackDamage, this);

        if (dead)
            KillAcquired?.Invoke(this, shooter);
    }
}