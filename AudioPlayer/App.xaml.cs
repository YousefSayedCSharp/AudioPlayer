using AudioPlayer.Views;
using System.Reflection.PortableExecutable;

namespace AudioPlayer;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        MainPage = new AppShell();
        //DateTime d1 = new DateTime(2022,11,14);
        //DateTime d2 = new DateTime(DateTime.Now.Year, DateTime.Now.Month,DateTime.Now.Day);
        //if (d2>d1)
        //{
        //    MainPage = new ChackDatePage();
        //}
        //else
        //{
        //    MainPage = new NavigationPage(new Views.HomeView()) { BarTextColor = Colors.White, BarBackgroundColor = Colors.Gray };
        //}
    }
}
