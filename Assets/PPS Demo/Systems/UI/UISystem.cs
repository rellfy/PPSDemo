using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using PPS;

[Serializable]
public class UISystem : System<UIProcessor> {

    /// <summary>
    /// UISystem only has one instance.
    /// </summary>
    [SerializeField]
    private UIProcessor instance;
    [SerializeField]
    private WorldSystem worldSystem;

    public WorldSystem WorldSystem => this.worldSystem;

    public override void Awake() {
        // Only start after WorldSystem has started.
        // Preferably, set MonoBehaviour script execution order via Project Settings so that WorldSystem fires first.
        if (!this.worldSystem.IsReady) {
            this.worldSystem.Ready += (sender, args) => {
                base.Awake();
            };
        } else {
            base.Awake();
        }
    }

    /// <summary>
    /// Unity 2019 does not serialize generics. For that reason, we convert the generic
    /// to the specific type that we want to serialize here.
    /// </summary>
    protected override void UpdateSerializableInstances(object sender, Type instanceType) {
        this.instance = this.instances[0];
    }
}