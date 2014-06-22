using UnityEngine;
using System.Collections;

public class SynthIntstrument : MelodicInstrument
{

    public Oscillator.OSCType oscType;

    public float attack, decay, sustain, release;

    public bool lfoEnabled;
    public float lfoFrequency;
    Oscillator lfo;

    protected override void Start()
    {
        for (int i = 0; i < voiceCount; ++i)
        {
            audibles.Add(new Voice(new Oscillator(oscType), new Envelope(attack, decay, sustain, release)));
        }

        lfo = new Oscillator(Oscillator.OSCType.SINE);
        lfo.Frequency = lfoFrequency;

        effects.Add(new Distortion(1.0f));

        base.Start();
    }

    protected override void OnAudioFilterRead(float[] data, int channels)
    {
        base.OnAudioFilterRead(data, channels);

        if (lfoEnabled)
        {
            for (int i = 0; i < data.Length; i += channels)
            {
                data[i] *= lfo.Next();

                if (channels == 2)
                    data[i + 1] = data[i];
            }
        }
    }

}
