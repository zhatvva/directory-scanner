using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using DirectoryScanner.Core.Interfaces;
using DirectoryScanner.Core.Services;
using DirectoryScanner.WPFApplication.Commands;
using DirectoryScanner.WPFApplication.Extensions;
using DirectoryScanner.WPFApplication.ViewModels;

namespace DirectoryScanner.WPFApplication
{
    internal class ViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private BindingList<NodeView> _directoryView;
        public BindingList<NodeView> DirectoryView
        {
            get => _directoryView;
            private set
            {
                _directoryView = value;
                OnPropertyChanged();
            }
        }

        private readonly IScanner _scanner = new Scanner();
        private ICommand _setDirectoryCommand;
        public ICommand SetDirectoryCommand
        {
            get
            {
                _setDirectoryCommand ??= new SetDirectoryCommand(_ => true, async _ =>
                {
                    var dialog = new Ookii.Dialogs.Wpf.VistaFolderBrowserDialog();
                    var isFolderSelected = dialog.ShowDialog().GetValueOrDefault();
                    if (isFolderSelected)
                    {
                        var directoryNode = new DirectoryNodeView("none", 0, 100);
                        DirectoryView = new BindingList<NodeView>() { directoryNode };
                        var scanResult = await _scanner.Scan(dialog.SelectedPath, 100, 
                            directoryNode.ChildAddedTracker);
                        directoryNode.Name = scanResult.Root.Name;
                        directoryNode.SizeInBytes = scanResult.Root.Size;
                        DirectoryView.Add(scanResult.ToViewModel());
                    }
                });
                return _setDirectoryCommand;
            } 
        }

        private ICommand _cancelDirectoryScanningCommand;
        public ICommand CancelDirectoryScanningCommand
        {
            get
            {
                _cancelDirectoryScanningCommand ??= new CancelDirectoryScanningCommand(_ => true, _ =>
                    _scanner?.Cancel());
                return _cancelDirectoryScanningCommand;
            }
        } 
        
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }   
    }
}