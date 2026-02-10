using System;
using BaseGame;

namespace Runner.BaseGame
{
    [Serializable]
    public class SettingData
    {
        public bool musicOn;
        public bool soundOn;
        public bool hapticOn;

        public void LoadSavedData()
        {
            var savedData = SaveFile.Load<SettingData>();
            if (savedData != null)
            {
                musicOn = savedData.musicOn;
                soundOn = savedData.soundOn;
                hapticOn = savedData.hapticOn;
            }
            else
            {
                musicOn = true;
                soundOn = true;
                hapticOn = true;
                SaveFile.Save<GameData>(this);
            }
        }

        public void SaveMusicStatus(bool on)
        {
            musicOn = on;
            SaveFile.Save<GameData>(this);
        }
        public void SaveSoundStatus(bool on)
        {
            soundOn = on;
            SaveFile.Save<GameData>(this);
        }
        public void SaveHapticStatus(bool on)
        {
            hapticOn = on;
            SaveFile.Save<GameData>(this);
        }
    }

    public enum AudioID
    {
        UIMusic, GameplayMusic,
        ButtonClick, PopupOpen, PopupClose,
    }

    [Serializable]
    public class AudioController
    {
        public void PlayMusic(AudioID audioID)
        {
        }

        public void PlaySound(AudioID audioID)
        {
        }
    }

    public class DeviceManager : SingletonManager<DeviceManager>
    {
        public static SettingData SettingData;
        public static AudioController Audio;

        public static void Init()
        {
            LoadSettingData();
        }

        private static void LoadSettingData()
        {
            SettingData ??= new SettingData();
            SettingData.LoadSavedData();
        }
    }
}