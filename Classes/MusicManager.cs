using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;

namespace Les_Loubards
{
    enum MusicState
    {
        Fading,
        Rising,
        Holding
    }

    static class MusicManager
    {

        public static MusicState State = MusicState.Holding;
        public static float VolumeTarget = 1f;
        public static float RateOfChange = 0.01f;

        public static void Update()
        {
            switch (State)
            {
                case MusicState.Rising:
                    if (MediaPlayer.Volume < VolumeTarget)
                    {
                        MediaPlayer.Volume += RateOfChange;
                        if (MediaPlayer.Volume > VolumeTarget)
                        {
                            MediaPlayer.Volume = VolumeTarget;
                            State = MusicState.Holding;
                        }
                    }
                    break;

                case MusicState.Fading:
                    if (MediaPlayer.Volume > VolumeTarget)
                    {
                        MediaPlayer.Volume -= RateOfChange;
                        if (MediaPlayer.Volume < VolumeTarget)
                        {
                            MediaPlayer.Volume = VolumeTarget;
                            State = MusicState.Holding;
                        }
                    }
                    break;
            }
        }

        public static void ChangeToVolume(float setTo)
        {
            VolumeTarget = setTo;

            if (VolumeTarget < MediaPlayer.Volume)
                State = MusicState.Fading;
            else
                State = MusicState.Rising;
        }

        public static void PlaySong(Song title)
        {
            MediaPlayer.Play(title);
        }
        public static void StopSong()
        {
            MediaPlayer.Stop();
        }
        public static void SetRepeating(bool IsRepeating)
        {
            MediaPlayer.IsRepeating = IsRepeating;
        }
    }
}
