using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using PPS;

[Serializable]
public class AudioSystem : System<AudioProcessor> {

    [SerializeField]
    private AudioClip blasterClip;
    /// <summary>
    /// AudioSystem only has one instance.
    /// </summary>
    [SerializeField]
    private AudioProcessor audioInstance;
    [SerializeField]
    private WorldSystem worldSystem;

    public AudioClip BlasterClip => this.blasterClip;

    public WorldSystem WorldSystem => this.worldSystem;

    public override void Awake() {
        base.Awake();
    }

    /// <summary>
    /// Unity 2019 does not serialize generics. For that reason, we convert the generic
    /// to the specific type that we want to serialize here.
    /// </summary>
    protected override void UpdateSerializableInstances(object sender, Type instanceType) {
        this.audioInstance = this.instances[0];
    }
}