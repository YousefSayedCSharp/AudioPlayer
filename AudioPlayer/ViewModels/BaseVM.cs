using System.ComponentModel;
using System.Runtime.CompilerServices;

public class BaseVM : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;
    public void onProperyChanged([CallerMemberName] string propryName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propryName));
    }

    public void SetValue<T>(ref T backingField, T value, [CallerMemberName] string propryName = null)
    {
        if (EqualityComparer<T>.Default.Equals(backingField, value)) //backingField == value
            return;

        backingField = value;
        onProperyChanged(propryName);
    }

    protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
