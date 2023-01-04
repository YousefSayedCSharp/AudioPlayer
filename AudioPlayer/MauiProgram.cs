using Plugin.Maui.Audio;

namespace AudioPlayer;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");

                fonts.AddFont("FA6Brands.otf", "FAB");
                fonts.AddFont("FA6Regular.otf", "FAR");
                fonts.AddFont("FA6Solid.otf", "FAS");
            });
        builder.Services.AddSingleton(AudioManager.Current);
        return builder.Build();
    }
}
