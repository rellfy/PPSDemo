using PPS;

public class AudioProcessor : Processor<AudioSystem, AudioProfile> {

    public AudioProcessor(AudioSystem system, AudioProfile profile) : base(system, profile) {
        this.subProcessors.Add(new ShootingProcessor(system, profile));
    }
}