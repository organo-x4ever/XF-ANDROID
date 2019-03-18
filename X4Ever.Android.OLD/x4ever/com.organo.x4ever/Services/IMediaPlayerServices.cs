using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.organo.x4ever.Services
{
    public interface IMediaPlayerServices : INotifyPropertyChanged
    {
        Action TimedTextAction { get; set; }
        double CurrentPosition { get; set; }
        double Duration { get; set; }
        bool IsPlaying { get; set; }
        void SetVolume(float left, float right);
        void StartPlayer(String filePath);
        void Pause();
        void Play();
        void Stop();
        void Release();
    }
}