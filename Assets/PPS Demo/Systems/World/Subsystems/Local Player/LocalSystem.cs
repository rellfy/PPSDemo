using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using PPS;

[Serializable]
public class LocalSystem : PPS.Subsystem<LocalProcessor> {

    /// <summary>
    /// LocalSystem only has one instance.
    /// </summary>
    [SerializeField]
    private LocalProcessor localInstance;

    public LocalProcessor LocalInstance => this.localInstance;

    /// <summary>
    /// Subsystems are serialized, therefore they are initialised through Awake.
    /// </summary>
    public override void Awake(Transform transform, ISystem parentSystem) {
        base.Awake(transform, parentSystem);
    }

    /// <summary>
    /// Unity 2019 does not serialize generics. For that reason, we convert the generic
    /// to the specific type that we want to serialize here.
    /// </summary>
    protected override void UpdateSerializableInstances(object sender, Type instanceType) {
        this.localInstance = this.instances?[0];
    }
}