using System;
using System.Media;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using VKApplication.Model;
using System.Collections.ObjectModel;
using VKApplication.ViewModel;
using System.ComponentModel;

namespace VKApplication.Model
{
    public class AudioService : BaseVM
    {
        private static AudioService _Instance = new AudioService();
        public static AudioService GetInstance() => _Instance;
     
        private AudioService() 
        {
            _MediaPlayer = new MediaPlayer();
            _MediaPlayer.Volume = 0.5;
            _MediaPlayer.MediaEnded += _MediaPlayer_MediaEnded;
        }

        private void _MediaPlayer_MediaEnded(object sender, EventArgs e)
        {
        
        }

        private static MediaPlayer _MediaPlayer { get; set; }
        public Item CurrentItem { get; set; }
        public bool IsPlaying { get; set; } = false;

        
        public void StartPlay(Item item)
        {
            if (CurrentItem == null)
            {
                CurrentItem = item;
                _MediaPlayer.Open(CurrentItem.Path);
                PlayPause();
            }
            else if (CurrentItem != item)
            {
                if (IsPlaying) PlayPause();
                CurrentItem = item;
                _MediaPlayer.Open(CurrentItem.Path);
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
