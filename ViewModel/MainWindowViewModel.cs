using Labb3.Commands;
using Labb3.Dialogs;
using Labb3.Model;
using Labb3.Views;
using MongoDB.Driver;
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
        public MongoDb dbHandler {  get; set; }
        private readonly Json jsonHandler;
        public ObservableCollection<Category> Categories { get; set; }
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

        private ObservableCollection<QuestionPackViewModel> _packs;

        public ObservableCollection<QuestionPackViewModel> Packs
        {
            get => _packs;
            set
            {
                _packs = value;
                RaisePropertyChanged(nameof(Packs));
            }
        }

        private QuestionPackViewModel? _newQuestionPack;
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

        
        private QuestionPackViewModel? _activePack;
		public QuestionPackViewModel? ActivePack
		{
			get => _activePack;
			set 
			{ 
				_activePack = value;

                if (_activePack != null && _activePack.Category != null)
                {
                    _activePack.Category = Categories.FirstOrDefault(c => c.Name == _activePack.Category.Name);
                }

				RaisePropertyChanged();
                ConfigurationViewModel?.RaisePropertyChanged("ActivePack");
			}
		}

        public MainWindowViewModel()
        {
            
            string connectionString = "mongodb://localhost:27017/";
            string databaseName = "AnnaKijlstra";
            dbHandler = new MongoDb(connectionString, databaseName);

            Packs = new ObservableCollection<QuestionPackViewModel>();
            PlayerViewModel = new PlayerViewModel(this);
            ConfigurationViewModel = new ConfigurationViewModel(this);
            NewQuestionPack = new QuestionPackViewModel(new QuestionPack());

            LoadDataAsync();

            NewPackWindowCommand = new DelegateCommand(ShowWindow);
            CloseWindowCommand = new DelegateCommand(CloseWindow);
            DeletePackCommand = new DelegateCommand(DeletePack, CanDeletePack);
            CreateNewPackCommand = new DelegateCommand(CreateNewPack);
            ExitCommand = new DelegateCommand(ExitGame);
            SelectPackCommand = new DelegateCommand(SelectPack);
            SelectModeCommand = new DelegateCommand(SelectMode);
            ShowFullscreenCommand = new DelegateCommand(ShowFullscreen);
        }

        private async void LoadDataAsync() 
        {
            var categories = await dbHandler.Categories.Find(_ => true).ToListAsync();
            
            Categories = new ObservableCollection<Category>(
                categories.Select(c => new Category(c.Name)));

            var questionPacks = await dbHandler.QuestionPacks.Find(_ => true).ToListAsync();
            
            Packs = new ObservableCollection<QuestionPackViewModel>(
                questionPacks.Select(p => new QuestionPackViewModel(p)));

            ActivePack = Packs.FirstOrDefault();
        }
        private async Task SaveDataAsync()
        {

            var packsToSave = Packs.Select(p => new QuestionPack(
                p.Name,
                p.Difficulty,
                p.TimeLimitInSeconds,
                dbHandler.Categories
                .Find(c => c.Name == p.Category.Name)
                .FirstOrDefault())
            {
                Questions = p.Questions.ToList()
            }).ToList();
            
            await dbHandler.QuestionPacks.DeleteManyAsync(_ => true);
            await dbHandler.QuestionPacks.InsertManyAsync(packsToSave);
            
        }

        private void SelectMode(object obj)
        {
            if ((string)obj == "PlayMode")
            {
                if (ActivePack.Questions.Count() == 0)
                {
                    MessageBox.Show(
                        "It's hard to play a quiz without questions. Try adding some :)",
                        "Quizinformation",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);
                }
                else
                {
                    ConfigurationViewModel.IsVisible = Visibility.Collapsed;
                    PlayerViewModel.IsVisible = Visibility.Visible;
                    PlayerViewModel.ResultVisibility = Visibility.Hidden;

                    PlayerViewModel.StartQuiz(ActivePack.Questions, ActivePack.TimeLimitInSeconds);
                }
            };
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

        private async void ExitGame(object obj)
        {
            await SaveDataAsync();
            Application.Current.Shutdown();
        }


        private void CreateNewPack(object obj)
        { 
            Packs.Add(new QuestionPackViewModel(new QuestionPack(
                NewQuestionPack.Name, 
                NewQuestionPack.Difficulty, 
                NewQuestionPack.TimeLimitInSeconds,
                NewQuestionPack.Category)));

            ActivePack = Packs.Last();
            RaisePropertyChanged(nameof(Packs));
            RaisePropertyChanged(nameof(ActivePack));

            if (ActivePack.Category.Name != null)
            {
                ActivePack.Category = dbHandler.Categories.Find(c =>  c.Name == ActivePack.Category.Name).First();
            }
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
            ActivePack = Packs.FirstOrDefault();

            RaisePropertyChanged(nameof(ActivePack));
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
