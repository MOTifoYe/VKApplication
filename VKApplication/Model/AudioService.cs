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
using System.Windows;
using System.Windows.Media.Animation;
using System.Windows.Controls;

namespace VKApplication.Model
{
    public class AudioService : BaseVM
    {
        private static AudioService _Instance = new AudioService();
        public static AudioService GetInstance() => _Instance;

        private AudioService() 
        {
            _MediaPlayer = new MediaPlayer();
            _MediaTimeline = new MediaTimeline();

            _MediaPlayer.MediaOpened += _MediaPlayer_MediaOpened;
            _MediaTimeline.CurrentTimeInvalidated += _MediaTimeline_CurrentTimeInvalidated;

            _MediaPlayer.Volume = 0.1;
        }

        private void _MediaPlayer_MediaOpened(object sender, EventArgs e)
        {
            if (_MediaPlayer.Clock.NaturalDuration.HasTimeSpan)
                TotalTime = _MediaPlayer.Clock.NaturalDuration.TimeSpan;
        }

        private void _MediaTimeline_CurrentTimeInvalidated(object sender, EventArgs e)
        {
            CurrentTime = _MediaPlayer.Clock.CurrentTime.Value;
        }

        public static MediaPlayer _MediaPlayer { get; set; }
        public static MediaTimeline _MediaTimeline { get; set; }
        public Item CurrentItem { get; set; }
        public MediaClock Clock { get; set; }
        public TimeSpan TotalTime { get; set; }
        public TimeSpan CurrentTime { get; set; }

                                                                                                       
        public void StartPlay(Item item)
        {
            CurrentItem = item;
            _MediaTimeline.Source = item.Path;
            Clock = _MediaTimeline.CreateClock();
            _MediaPlayer.Clock = Clock;
        }                                                  
        public void PlayPause()
        {
            if (_MediaPlayer.Clock.IsPaused == true)
                _MediaPlayer.Clock.Controller.Resume();
            else if (_MediaPlayer.Clock.IsPaused == false)
                _MediaPlayer.Clock.Controller.Pause();
        }

        public void Pause() => _MediaPlayer.Clock.Controller.Pause();
        public void Resume() => _MediaPlayer.Clock.Controller.Resume();
        public void Add5() 
        {
            _MediaPlayer.Clock.Controller.Seek(new TimeSpan(0, 0, 0, 5), TimeSeekOrigin.BeginTime);
        } 
    }
}
