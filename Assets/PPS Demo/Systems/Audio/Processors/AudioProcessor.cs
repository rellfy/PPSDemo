using PPS;
using UnityEngine;

public class AudioProcessor : Processor<AudioSystem> {

    private AudioProfile profile;

    public AudioProcessor(AudioSystem system, GameObject instance) : base(system, instance) {
        this.profile = new AudioProfile(GameObject);
        SubProcessors.Add(new ShootingProcessor(system, this.profile));
    }
}