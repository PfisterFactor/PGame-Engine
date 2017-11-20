// .NET stuff
using System;
using System.Collections.Generic;
using System.Linq;
// XNA stuff
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace PfisterGameEngine
{
    public static class GameLogic
    {
        private static SoundEffect backgroundMusic;
        private static SoundEffectInstance backgroundMusicInstance;
        
        public static void SetBackgroundMusic(SoundEffect soundEffect)
        {
            if (soundEffect != null)
            {
                backgroundMusic = soundEffect;
            }
            else
            {
                throw new ArgumentNullException(nameof(soundEffect), "Background music cannot be null!");
            }
        }
        public static void StartBackgroundMusic()
        {
            backgroundMusicInstance = backgroundMusic?.CreateInstance();
            if (backgroundMusicInstance != null)
            {
                backgroundMusicInstance.IsLooped = true;
                backgroundMusicInstance.Volume = 0.1f;
                backgroundMusicInstance.Play();
            }
        }

        public static void PauseBackgroundMusic(bool pause)
        {
                if (pause)
                    backgroundMusicInstance?.Pause();
                else
                    backgroundMusicInstance?.Play();
            
        }
        public static void StopBackgroundMusic()
        {
            backgroundMusicInstance?.Stop();
        }
    }
}
