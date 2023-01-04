using Plugin.Maui.Audio;

namespace AudioPlayer;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
        code();
    }

    async void code()
    {
        PermissionStatus StatusRead = await Permissions.RequestAsync<Permissions.StorageRead>();
        IEnumerable<FileResult> result = await FilePicker.Default.PickMultipleAsync();
        
        if (result != null)
        {
            List<FileResult> lr = result.ToList().OrderBy(i=>i.FileName).ToList();
            int index = 0;
            IAudioPlayer audioPlayer;
            async void run()
            {
                lbl.Text += lr[index].FileName+"\n";
                audioPlayer = AudioManager.Current.CreatePlayer(await lr[index].OpenReadAsync());
                audioPlayer.Play();
                audioPlayer.PlaybackEnded += delegate
                {
                    if (index >= lr.Count - 1)
                        return;

                    index++;
                    run();
                };
            }
            run();
            //foreach (var item in result)
            //{
            //    audioPlayer = AudioManager.Current.CreatePlayer(await item.OpenReadAsync());
            //    audioPlayer.Play();
            //}
            //string dir = Path.GetDirectoryName(result.FullPath);
            //await DisplayAlert("",Path.GetFileNameWithoutExtension(item),"OK");
        }
    }

}