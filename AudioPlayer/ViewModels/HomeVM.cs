using Plugin.Maui.Audio;
using System.Runtime.ExceptionServices;
using System.Windows.Input;

namespace AudioPlayer.ViewModels;

public class HomeVM : BaseVM
{
    IAudioManager audioManager;
    IAudioPlayer audioPlayer;
    IDispatcher dispatcher;
    bool isPositionChangeSystemDriven;
    bool IsSelectedFile = false;
    int index = 0;
    int CountOfSelectedFiles = 0;

    public HomeVM()
    {
        audioManager = AudioManager.Current;
        dispatcher = Dispatcher.GetForCurrentThread();
    }

    public async void Run()
    {
        //Uri u = new Uri("https://cdn.pixabay.com/download/audio/2022/10/12/audio_061cead49a.mp3");
        //audioPlayer = audioManager.CreatePlayer(u+"");
        //audioPlayer.Play();
        //return;
        index = 0;
        CountOfSelectedFiles = 0;

        IEnumerable<FileResult> files = await FilePicker.Default.PickMultipleAsync((new PickOptions
        {
            PickerTitle = "Select audio file",
            FileTypes = new FilePickerFileType(
                new Dictionary<DevicePlatform, IEnumerable<string>>
                {
                    { DevicePlatform.WinUI, new [] { "*.mp3", "*.m4a" } },
                    { DevicePlatform.Android, new [] { "audio/*" } },
                    { DevicePlatform.iOS, new[] { "public.audio" } },
                    { DevicePlatform.MacCatalyst, new[] { "public.audio" } }
                })
        }));
        
        if (files != null&& files.Count()>0)
        {
            List<FileResult> list = files.ToList();
            CountOfSelectedFiles = files.Count();
            async void loop()
            {
                if (IsSelectedFile && audioPlayer.IsPlaying)
                {
                    PlayPause();
                    audioPlayer.Stop();
                }
                IsSelectedFile = true;

                FileResult f = list[index];
                audioPlayer = audioManager.CreatePlayer(await f.OpenReadAsync());
                lblAudioName = f.FileName;
                max = audioPlayer.Duration;
                PlayPause();
                UpdatePlaybackPosition();
                audioPlayer.PlaybackEnded += delegate
                {
                    PlayPause();
                    value = 1;

                    if (index < list.Count - 1)
                    {
                        index++;
                        loop();
                    }
                };
            }
            loop();
            //lblMsg = audioPlayer.CanSeek + "";
            
        }
    }

    public void PlayPause()
    {
        if (btnPlayAnPauseText == "\uf04c")
        {
            audioPlayer.Pause();
            btnPlayAnPauseText = "\uf04b";
            txtDescription = "تشغيل";
        }
        else
        {
            audioPlayer.Play();
            btnPlayAnPauseText = "\uf04c";
            txtDescription = "إقاف مؤقط";
            UpdatePlaybackPosition();
        }
    }

    private string _btnPlayAnPauseText;
    public string btnPlayAnPauseText
    {
        get => _btnPlayAnPauseText?? "\uf04b";
        set { SetValue(ref _btnPlayAnPauseText,value); }
    }

    private string _txtDescription;
    public string txtDescription
    {
        get => _txtDescription?? "تشغيل";
        set { SetValue(ref _txtDescription,value); }
    }

    private string _lblAudioName;
    public string lblAudioName
    {
        get => _lblAudioName;
        set { SetValue(ref _lblAudioName,value); }
    }

    private double _max;
    public double max
    {
        get => _max;
        set { SetValue(ref _max, value); }
    }

    private double _value;
    public double value
    {
        get => audioPlayer?.CurrentPosition ?? 1;
        set
        {
            if (audioPlayer is not null &&
                audioPlayer.CanSeek &&
                isPositionChangeSystemDriven is false)
            {
                audioPlayer.Seek(value);
            }
            SetValue(ref _value, value);
        }
    }

    public ICommand btn
    {
        get
        {
            return new Command(() =>
            {
                Run();
            });
        }
    }

    public ICommand btnPlayAndPause
    {
        get
        {
            return new Command(() =>
            {
                if(!IsSelectedFile)
                {
                    Run();
                    return;
                }

                PlayPause();                
            });
        }
    }

    public ICommand btnBackWord
    {
        get
        {
            return new Command(() =>
            {
                if (value - 5 >= 0)
                    value -= 5;
                else
                    value = 0;
            });
        }
    }

    public ICommand btnNextWord
    {
        get
        {
            return new Command(() =>
            {
                if (value + 5 <= max)
                    value += 5;
                else
                    value =max;
            });
        }
    }

    public ICommand btnNext
    {
        get{
            return new Command(() =>
            {
                if (CountOfSelectedFiles < 1)
                    return;

                value = max;
            });
        }
    }

    public ICommand btnBack
    {
        get
        {
            return new Command(() =>
            {
                if (CountOfSelectedFiles < 0)
                    return;

                index -= 2;
                if (index < 0)
                    index = 0;

                value = max;
            });
        }
    }

    public void UpdatePlaybackPosition()
    {
        if (audioPlayer?.IsPlaying is false)
        {
            return;
        }

        dispatcher.DispatchDelayed(
            TimeSpan.FromMilliseconds(16),
            () =>
            {
                //Console.WriteLine($"{CurrentPosition} with duration of {Duration}");

                isPositionChangeSystemDriven = true;

                NotifyPropertyChanged(nameof(value));

                isPositionChangeSystemDriven = false;

                UpdatePlaybackPosition();
            });
    }

}
