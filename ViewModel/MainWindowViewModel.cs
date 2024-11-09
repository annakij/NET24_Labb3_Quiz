using Labb3.Commands;
using Labb3.Dialogs;
using Labb3.Model;
using Labb3.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Labb3.ViewModel
{
    internal class MainWindowViewModel : ViewModelBase
    {
        private QuestionPackViewModel? _newQuestionPack;
        private QuestionPackViewModel? _activePack;
        public ObservableCollection<QuestionPackViewModel> Packs { get; set; }
        public PlayerViewModel PlayerViewModel { get; }
        public ConfigurationViewModel ConfigurationViewModel { get; }
        public DelegateCommand NewPackWindowCommand { get; }
        public DelegateCommand CloseWindowCommand { get; }
        public DelegateCommand CreateNewPackCommand { get; }
        public DelegateCommand DeletePackCommand { get; }
        public DelegateCommand ExitCommand { get; }
        public DelegateCommand SelectPackCommand { get; }
        public DelegateCommand SelectModeCommand { get; }
        public DelegateCommand ShowFullscreenCommand { get; }

        public QuestionPackViewModel? NewQuestionPack
        {
            get
            {
                return _newQuestionPack;
            }
            set
            {
                _newQuestionPack = value;
                RaisePropertyChanged();
            }
        }

        
		public QuestionPackViewModel? ActivePack
		{
			get => _activePack;
			set 
			{ 
				_activePack = value;
				RaisePropertyChanged();
                ConfigurationViewModel?.RaisePropertyChanged("ActivePack");
			}
		}

        public MainWindowViewModel()
        {
            Packs = new ObservableCollection<QuestionPackViewModel>();
            
            ActivePack = new QuestionPackViewModel(new QuestionPack("Random Questions Pack", Difficulty.Hard, 30));
            Packs.Add(ActivePack);
            PlayerViewModel = new PlayerViewModel(this);
            ConfigurationViewModel = new ConfigurationViewModel(this);

            NewQuestionPack = new QuestionPackViewModel(new QuestionPack());

            NewPackWindowCommand = new DelegateCommand(ShowWindow);
            CloseWindowCommand = new DelegateCommand(CloseWindow);
            DeletePackCommand = new DelegateCommand(DeletePack, CanDeletePack);
            CreateNewPackCommand = new DelegateCommand(CreateNewPack);
            ExitCommand = new DelegateCommand(ExitGame);
            SelectPackCommand = new DelegateCommand(SelectPack);
            SelectModeCommand = new DelegateCommand(SelectMode);
            ShowFullscreenCommand = new DelegateCommand(ShowFullscreen);
        }

        private void SelectMode(object obj)
        {
            if ((string)obj == "PlayMode")
            {
                ConfigurationViewModel.IsVisible = Visibility.Collapsed;
                PlayerViewModel.IsVisible = Visibility.Visible;
                PlayerViewModel.ResultVisibility = Visibility.Hidden;

                PlayerViewModel.StartQuiz(ActivePack.Questions, ActivePack.TimeLimitInSeconds);
            }   
            if ((string)obj == "EditMode")
            {
                ConfigurationViewModel.IsVisible = Visibility.Visible;
                PlayerViewModel.IsVisible = Visibility.Collapsed;
                PlayerViewModel.ResultVisibility = Visibility.Hidden;
            }
        }

        private void SelectPack(object obj)
        {
            if (obj is QuestionPackViewModel questionPack)
            {
                ActivePack = questionPack;
                RaisePropertyChanged(nameof(ActivePack));
            }
        }

        private void ExitGame(object obj)
        {
            Application.Current.Shutdown();
        }

        private void CreateNewPack(object obj)
        {
            
            Packs.Add(new QuestionPackViewModel(new QuestionPack(
                NewQuestionPack.Name, 
                NewQuestionPack.Difficulty, 
                NewQuestionPack.TimeLimitInSeconds)));

            ActivePack = Packs.Last();
            RaisePropertyChanged();
            
        }


        private bool CanDeletePack(object? arg)
        {
            if (ActivePack != null)
            {  
                return true; 
            }
            return false;
        }

        private void DeletePack(object obj)
        {
            Packs.Remove(ActivePack);
            RaisePropertyChanged();
        }

        private void CloseWindow(object obj)
        {
            if (obj is CreateNewPackDialog dialog)
            {
                dialog.Close();
            }
        }

        private void ShowWindow(object obj)
        {
            CreateNewPackDialog packDialogWindow = new CreateNewPackDialog
            {
                DataContext = this,
                Owner = Application.Current.MainWindow
            };
            packDialogWindow.ShowDialog();
        }
        private void ShowFullscreen(object obj)
        {
            var mainWindow = Application.Current.MainWindow;

            if (mainWindow.WindowState != WindowState.Maximized || mainWindow.WindowStyle != WindowStyle.None)
            {
                mainWindow.WindowStyle = WindowStyle.None;
                mainWindow.WindowState = WindowState.Maximized;
            }
            else
            {
                mainWindow.WindowStyle = WindowStyle.SingleBorderWindow;
                mainWindow.WindowState = WindowState.Normal;
            }
        }
    }
}
