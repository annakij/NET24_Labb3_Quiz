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

        private ObservableCollection<Category> _categories;

        public ObservableCollection<Category> Categories
        {
            get => _categories;
            set
            {
                _categories = value;
                RaisePropertyChanged(nameof(Categories));
            }
        }
        private string _newCategory;
        public string NewCategory
        {
            get => _newCategory;
            set
            {
                _newCategory = value;
                RaisePropertyChanged(nameof(NewCategory));
            }
        }

        private Category? _selectedCategory;
        public Category? SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                _selectedCategory = value;
                RaisePropertyChanged(nameof(SelectedCategory));
            }
        }

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
        public DelegateCommand CategoryWindowCommand { get; }
        public DelegateCommand AddButtonCommand { get; }
        public DelegateCommand RemoveButtonCommand { get; }
        public DelegateCommand AddCategoryCommand { get; }
        public DelegateCommand RemoveCategoryCommand { get; }

        public ConfigurationViewModel(MainWindowViewModel? mainWindowViewModel)
        {
            this.mainWindowViewModel = mainWindowViewModel;
            IsVisible = Visibility.Visible;

            Categories = new ObservableCollection<Category>();
            DifficultyOptions = new ObservableCollection<Difficulty>((Difficulty[])Enum.GetValues(typeof(Difficulty)));

            PackOptionsWindowCommand = new DelegateCommand(ShowWindow);
            CategoryWindowCommand = new DelegateCommand(ShowCategoryWindow);
            AddButtonCommand = new DelegateCommand(AddButton);
            RemoveButtonCommand = new DelegateCommand(RemoveButton);
            AddCategoryCommand = new DelegateCommand(AddCategory);
            RemoveCategoryCommand = new DelegateCommand(RemoveCategory);

        }

        private void RemoveCategory(object obj)
        {
            if (obj is null) return;
            else
            {
                Categories.Remove(SelectedCategory);
            }
        }

        private void AddCategory(object obj)
        {
            var newCategory = new Category(NewCategory);
            Categories.Add(newCategory);

            NewCategory = string.Empty;
            RaisePropertyChanged(nameof(NewCategory));
            RaisePropertyChanged(nameof(Categories));
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
        private void ShowCategoryWindow(object obj)
        {
            CategoryDialog categoryDialog = new CategoryDialog
            {
                DataContext = this,
                Owner = Application.Current.MainWindow
            };
            categoryDialog.ShowDialog();
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
