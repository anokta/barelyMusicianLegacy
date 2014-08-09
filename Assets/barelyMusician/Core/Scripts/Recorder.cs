using System;
using System.IO;
using UnityEngine;

namespace BarelyAPI
{
    public class Recorder : MonoBehaviour
    {
        public string fileName;

        FileStream fileStream;
        string folderPath;

        bool recording;

        const int HEADER_SIZE = 44;
        const int RESCALE_FACTOR = 32767;

        void Awake()
        {
            folderPath = Application.dataPath + "/../Records/";
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                if (!recording)
                {
                    StartRecord();

                    Debug.Log("Recording started..");
                }
                else
                {
                    StopRecord();

                    Debug.Log("Recording stopped. File saved to: " + Path.Combine(folderPath, fileName));
                }
            }
        }

        void OnAudioFilterRead(float[] data, int channels)
        {
            if (recording)
            {
                ConvertAndWrite(ref data);
            }
        }

        public void StartRecord()
        {
            recording = true;

            StartWriting();
        }

        public void StopRecord()
        {
            recording = false;

            WriteHeader();
        }

        void StartWriting()
        {
            if (!fileName.ToLower().EndsWith(".wav"))
            {
                fileName += ".wav";
            }

            fileStream = new FileStream(Path.Combine(folderPath, fileName), FileMode.Create);
            byte emptyByte = new byte();

            for (int i = 0; i < HEADER_SIZE; i++) //preparing the header
            {
                fileStream.WriteByte(emptyByte);
            }
        }
        
        void ConvertAndWrite(ref float[] samples)
        {
            Int16[] intData = new Int16[samples.Length];
            //converting in 2 float[] steps to Int16[], //then Int16[] to Byte[]

            Byte[] bytesData = new Byte[samples.Length * 2];
            //bytesData array is twice the size of
            //dataSource array because a float converted in Int16 is 2 bytes.

            for (int i = 0; i < samples.Length; i++)
            {
                intData[i] = (short)(samples[i] * RESCALE_FACTOR);
                Byte[] byteArr = new Byte[2];
                byteArr = BitConverter.GetBytes(intData[i]);
                byteArr.CopyTo(bytesData, i * 2);
            }

            fileStream.Write(bytesData, 0, bytesData.Length);
        }

        void WriteHeader()
        {
            fileStream.Seek(0, SeekOrigin.Begin);

            Byte[] riff = System.Text.Encoding.UTF8.GetBytes("RIFF");
            fileStream.Write(riff, 0, 4);

            Byte[] chunkSize = BitConverter.GetBytes(fileStream.Length - 8);
            fileStream.Write(chunkSize, 0, 4);

            Byte[] wave = System.Text.Encoding.UTF8.GetBytes("WAVE");
            fileStream.Write(wave, 0, 4);

            Byte[] fmt = System.Text.Encoding.UTF8.GetBytes("fmt ");
            fileStream.Write(fmt, 0, 4);

            Byte[] subChunk1 = BitConverter.GetBytes(16);
            fileStream.Write(subChunk1, 0, 4);

            UInt16 two = 2;
            UInt16 one = 1;

            Byte[] audioFormat = BitConverter.GetBytes(one);
            fileStream.Write(audioFormat, 0, 2);

            Byte[] channels = BitConverter.GetBytes(two);
            fileStream.Write(channels, 0, 2);

            Byte[] sampleRate = BitConverter.GetBytes(AudioProperties.SAMPLE_RATE);
            fileStream.Write(sampleRate, 0, 4);

            Byte[] byteRate = BitConverter.GetBytes(AudioProperties.SAMPLE_RATE * two * 2); // sampleRate * bytesPerSample*number of channels, here 44100*2*2
            fileStream.Write(byteRate, 0, 4);

            UInt16 blockAlign = (ushort)(two * 2);
            fileStream.Write(BitConverter.GetBytes(blockAlign), 0, 2);

            UInt16 bps = 16;
            Byte[] bitsPerSample = BitConverter.GetBytes(bps);
            fileStream.Write(bitsPerSample, 0, 2);

            Byte[] datastring = System.Text.Encoding.UTF8.GetBytes("data");
            fileStream.Write(datastring, 0, 4);

            Byte[] subChunk2 = BitConverter.GetBytes(fileStream.Length - HEADER_SIZE);
            fileStream.Write(subChunk2, 0, 4);

            fileStream.Close();
        }
    }
}