using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DirectoryScanner.WPFApplication.ViewModels
{
    internal class NodeView : INotifyPropertyChanged
    {
        private string _name;
        public string Name
        {
            get => _name;
            set => SetField(ref _name, value);
        }

        private long _sizeInBytes;
        public long SizeInBytes
        {
            get => _sizeInBytes;
            set => SetField(ref _sizeInBytes, value);
        }

        private float _sizeInPercents;
        public float SizeInPercents
        {
            get => _sizeInPercents;
            set => SetField(ref _sizeInPercents, value);
        }

        protected NodeView(string name, long sizeInBytes, float sizeInPercents)
        {
            Name = name;
            SizeInBytes = sizeInBytes;
            SizeInPercents = sizeInPercents;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
