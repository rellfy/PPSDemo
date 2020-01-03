using System;
using System.Collections.Generic;
using UnityEngine;
using PPS;
using Object = UnityEngine.Object;

[Serializable]
public class ShooterProfile : Profile {

    private Transform shootPoint;
    private Transform cameraPoint;
    private Transform head;
    private Rigidbody rigidbody;
    private AudioSource audioSource;
    [SerializeField]
    private bool isAI = true;
    [SerializeField]
    private float health = 100f;
    [SerializeField]
    private List<BulletTracker> shotsFired = new List<BulletTracker>();
    public float lastShotTimeSeconds = 0;

    public Transform ShootPoint => this.shootPoint;
    public Transform CameraPoint => this.cameraPoint;
    public Transform Head => this.head;
    public Rigidbody Rigidbody => this.rigidbody;
    public AudioSource AudioSource => this.audioSource;
    public List<BulletTracker> ShotsFired => this.shotsFired;
    public bool IsAI {
        get => this.isAI;
        set => this.isAI = value;
    }
    public float Health {
        get => this.health;
        set {
            this.health = value;
            HealthChange?.Invoke(this, value);
        }
    }

    public event EventHandler<float> HealthChange;

    public struct BulletTracker {
        public Vector3 startPosition;
        public Transform bullet;
        public Vector3 direction;

        public BulletTracker(Transform bullet, Vector3 direction) {
            this.bullet = bullet;
            this.direction = direction;
            this.startPosition = bullet.transform.position;
        }
    }

    public ShooterProfile(GameObject gameObject) : base(gameObject) {
        ShooterReferencing referencing = gameObject.GetComponent<ShooterReferencing>();

        this.shootPoint = referencing.shootPoint;
        this.cameraPoint = referencing.cameraPoint;
        this.rigidbody = referencing.rigidbody;
        this.audioSource = referencing.audioSource;
        this.head = referencing.head;

        Object.DestroyImmediate(referencing);
    }
}