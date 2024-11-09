using Labb3.Commands;
using Labb3.Dialogs;
using Labb3.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace Labb3.ViewModel
{
    internal class ConfigurationViewModel : ViewModelBase
    {
        private readonly MainWindowViewModel? mainWindowViewModel;
        public QuestionPackViewModel? ActivePack { get => mainWindowViewModel.ActivePack; }
        public Visibility EditorVisibility => ActiveQuestion == null ? Visibility.Collapsed : Visibility.Visible;
        
        private Question? _activeQuestion;
        public Question? ActiveQuestion
        {
            get
            {
                return _activeQuestion;
            }
            set
            {
                _activeQuestion = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(EditorVisibility));
            }
        }
        private Visibility _isVisible;
        public Visibility IsVisible
        {
            get => _isVisible;
            set
            {
                _isVisible = value;
                RaisePropertyChanged();
            }
        }
        public ObservableCollection<Difficulty> DifficultyOptions { get; }
        public DelegateCommand PackOptionsWindowCommand { get; }
        public DelegateCommand AddButtonCommand { get; }
        public DelegateCommand RemoveButtonCommand { get; }

        public ConfigurationViewModel(MainWindowViewModel? mainWindowViewModel)
        {
            this.mainWindowViewModel = mainWindowViewModel;
            IsVisible = Visibility.Visible;

            DifficultyOptions = new ObservableCollection<Difficulty>((Difficulty[])Enum.GetValues(typeof(Difficulty)));
            PackOptionsWindowCommand = new DelegateCommand(ShowWindow);
            AddButtonCommand = new DelegateCommand(AddButton);
            RemoveButtonCommand = new DelegateCommand(RemoveButton);

            //ActivePack.Questions.Add(new Question("Vad heter Sveriges huvudstad?", "Stockholm", "London", "Indien", "Kalle"));
            //ActivePack.Questions.Add(new Question("Vem är bäst?", "Du är", "Någon annan", "Zlatan", "Din mamma"));
            //ActivePack.Questions.Add(new Question("Kerstin är vilken ras?", "Griffon", "Råtta", "Blandras", "Irländsk Varghund"));
        }
        private void RemoveButton(object obj)
        {
            ActivePack.Questions.Remove(ActiveQuestion);
            RaisePropertyChanged();
        }
        private void AddButton(object obj)
        {
            var newQuestion = new Question("", "", "", "", "");
            ActivePack.Questions.Add(newQuestion);
            ActiveQuestion = newQuestion;
            RaisePropertyChanged(nameof(ActivePack.Questions));
            RaisePropertyChanged(nameof(ActiveQuestion));
        }
        private void ShowWindow(object obj)
        {
            PackOptionsDialog packOptionsDialog = new PackOptionsDialog
            {
                DataContext = this,
                Owner = Application.Current.MainWindow
            };
            packOptionsDialog.ShowDialog();
        }
    }
}
