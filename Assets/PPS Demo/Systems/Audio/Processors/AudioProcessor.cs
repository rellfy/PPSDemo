using PPS;

public class AudioProcessor : Processor<AudioSystem, AudioProfile> {

    public AudioProcessor(AudioSystem system, AudioProfile profile) : base(system, profile) {
        SubProcessors.Add(new ShootingProcessor(system, profile));
    }
}