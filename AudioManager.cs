using SplashKitSDK;
using System;
using System.Collections.Generic;

namespace Chess
{
    public static class AudioManager
    {
        private static Dictionary<string, SoundEffect> sounds;
        
        static AudioManager()
        {
            sounds = new Dictionary<string, SoundEffect>();
            LoadSounds();
        }
        
        private static void LoadSounds()
        {
            try
            {
                // Load all chess sound effects
                sounds["Move"] = SplashKit.LoadSoundEffect("Move", "bin/Debug/Move.wav");
                sounds["Capture"] = SplashKit.LoadSoundEffect("Capture", "bin/Debug/Capture.wav");
                sounds["Castle"] = SplashKit.LoadSoundEffect("Castle", "bin/Debug/Castle.wav");
                sounds["Check"] = SplashKit.LoadSoundEffect("Check", "bin/Debug/Check.wav");
                sounds["IllegalMove"] = SplashKit.LoadSoundEffect("IllegalMove", "bin/Debug/IllegalMove.wav");
                sounds["GameOver"] = SplashKit.LoadSoundEffect("GameOver", "bin/Debug/GameOver.wav");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to load some sound effects: {ex.Message}");
            }
        }
        
        public static void PlaySound(string soundName)
        {
            if (sounds.TryGetValue(soundName, out SoundEffect? sound) && sound != null)
            {
                try
                {
                    SplashKit.PlaySoundEffect(sound);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to play sound {soundName}: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine($"Sound {soundName} not found");
            }
        }
        
        public static void PlayMoveSound(bool isCapture = false)
        {
            PlaySound(isCapture ? "Capture" : "Move");
        }
        
        public static void PlayCastleSound()
        {
            PlaySound("Castle");
        }
        
        public static void PlayCheckSound()
        {
            PlaySound("Check");
        }
        
        public static void PlayIllegalMoveSound()
        {
            PlaySound("IllegalMove");
        }
        
        public static void PlayGameOverSound()
        {
            PlaySound("GameOver");
        }
        
        public static void Cleanup()
        {
            foreach (var sound in sounds.Values)
            {
                SplashKit.FreeSoundEffect(sound);
            }
            sounds.Clear();
        }
    }
} 