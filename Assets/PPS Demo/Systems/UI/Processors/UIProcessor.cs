using PPS;

public class UIProcessor : Processor<UISystem, UIProfile> {

    public UIProcessor(UISystem system, UIProfile profile) : base(system, profile) {
        SubProcessors.Add(new ScoreProcessor(system, profile));
        SubProcessors.Add(new HealthProcessor(system, profile));
    }
}