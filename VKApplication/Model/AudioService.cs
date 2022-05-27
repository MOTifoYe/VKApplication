using System;
using System.Media;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using VKApplication.Model;

namespace VKApplication.Model
{
    public class AudioService : BaseVM
    {
        private static AudioService _Instance = new AudioService();
        public static AudioService GetInstance() => _Instance;
        private static MediaPlayer _MediaPlayer = new MediaPlayer();
        public static MediaPlayer GetMediaPlayer() => _MediaPlayer;

        private AudioService() { }
        //public Action<string, bool> Show { get; set; }

        public Item CurrentItem { get; set; }
        public bool IsPlaying { get; set; } = false;

        public void StartPlay(Item item)
        {
            if (CurrentItem == null)
            {
                CurrentItem = item;
                _MediaPlayer.Open(new Uri(CurrentItem.Path));
                PlayPause();
            }
            else if (CurrentItem != item)
            {
                if (IsPlaying) PlayPause();
                CurrentItem = item;
                _MediaPlayer.Open(new Uri(CurrentItem.Path));
                if (!IsPlaying) PlayPause();
            }
            else if (CurrentItem == item)
            {
                _MediaPlayer.Stop();
                _MediaPlayer.Play();
                IsPlaying = true;
            }
        }

        public void PlayPause()
        {
            if (IsPlaying == true)
            {
                _MediaPlayer.Pause();
                IsPlaying = false;
            }
            else if (IsPlaying == false)
            {
                _MediaPlayer.Play();
                IsPlaying = true;
            }
        }
    }
}
