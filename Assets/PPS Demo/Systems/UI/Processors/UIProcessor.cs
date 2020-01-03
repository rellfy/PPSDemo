using PPS;

public class UIProcessor : Processor<UISystem, UIProfile> {

    public UIProcessor(UISystem system, UIProfile profile) : base(system, profile) {
        this.subProcessors.Add(new ScoreProcessor(system, profile));
        this.subProcessors.Add(new HealthProcessor(system, profile));
    }
}