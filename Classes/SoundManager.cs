using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;

namespace Les_Loubards
{
    public enum SoundEnum
    {
        ACTOR_DEATH,
        ATTACK,
        KNIFE_GET_OUT,
        KNIFE_THROW,
        PICK_UP_ITEM,
        TAKE_HIT,
        TRASH_CAN_HIT,
        VAGUE_SUIVANTE
    }

    public enum VoiceSoundEnum
    {
        OH,
        HE,
        YA,
        TITLE_SCREEN
    }
    static class SoundManager
    {
        static Dictionary<SoundEnum, SoundEffectInstance> soundEffectsDictionary = new Dictionary<SoundEnum, SoundEffectInstance>();
        static Dictionary<VoiceSoundEnum, SoundEffectInstance> voiceSoundEffectsDictionary = new Dictionary<VoiceSoundEnum, SoundEffectInstance>();

        public static void Initialize(ContentManager pContent)
        {
            pContent.RootDirectory = "Content";

            foreach (SoundEnum lSound in Enum.GetValues(typeof(SoundEnum)))
            {
                SoundEffectInstance lInstance = pContent.Load<SoundEffect>(@"Sounds\" + lSound.ToString().ToLower()).CreateInstance();
                soundEffectsDictionary.Add(lSound, lInstance);
            }
            foreach (VoiceSoundEnum lSound in Enum.GetValues(typeof(VoiceSoundEnum)))
            {
                SoundEffectInstance lInstance = pContent.Load<SoundEffect>(@"Sounds\" + lSound.ToString().ToLower()).CreateInstance();
                voiceSoundEffectsDictionary.Add(lSound, lInstance);
            }
        }

        public static void Update()
        {

        }

        public static void PlaySound(SoundEnum pSound)
        {
            soundEffectsDictionary[pSound].Play();
        }

        public static void PlayVoiceSound(VoiceSoundEnum pSound)
        {
            voiceSoundEffectsDictionary[pSound].Play();
        }

        public static void StopVoiceSound()
        {
            foreach (SoundEffectInstance lSoundInstance in voiceSoundEffectsDictionary.Values)
            {
                lSoundInstance.Stop();
            }
        }
    }
}
