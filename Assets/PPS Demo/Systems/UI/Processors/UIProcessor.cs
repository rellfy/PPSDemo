using PPS;
using UnityEngine;

public class UIProcessor : Processor<UISystem> {

    private UIProfile profile;

    public UIProcessor(UISystem system, GameObject instance) : base(system, instance) {
        this.profile = new UIProfile(GameObject);

        SubProcessors.Add(new ScoreProcessor(system, this.profile));
        SubProcessors.Add(new HealthProcessor(system, this.profile));
    }
}