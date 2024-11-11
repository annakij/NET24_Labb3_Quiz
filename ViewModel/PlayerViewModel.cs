using Labb3.Commands;
using Labb3.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Media;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace Labb3.ViewModel
{
    internal class PlayerViewModel : ViewModelBase
    {
        private readonly MainWindowViewModel? mainWindowViewModel;
        private QuestionPackViewModel? _activePack;
        private DispatcherTimer timer;
        private Visibility _isVisible;
        private Visibility _resultVisibility;
        private int _currentQuestionIndex;
        private int _timeRemaining;
        private ObservableCollection<Question> _shuffledQuestions;
        private Question _currentQuestion;
        private bool? _isAnswerCorrect;
        private Random _random = new();
        public int CurrentQuestionIndex
        {
            get =>_currentQuestionIndex;
            set
            {
                _currentQuestionIndex = value;
                RaisePropertyChanged();
            }
        }
        public QuestionPackViewModel? ActivePack
        {
            get => _activePack;
            set 
            { 
                _activePack = value;
                RaisePropertyChanged(nameof(ActivePack));
            }
        }
        public int TimeRemaining
        {
            get => _timeRemaining;
            set
            {
                _timeRemaining = value;
                RaisePropertyChanged(nameof(TimeRemaining));
            }
        }
        public Visibility IsVisible
        {
            get => _isVisible;
            set
            {
                _isVisible = value;
                RaisePropertyChanged();
            }
        }
        public Visibility ResultVisibility
        {
            get => _resultVisibility;
            set
            {
                _resultVisibility = value;
                RaisePropertyChanged();
            }
        }
        public ObservableCollection<Question> ShuffledQuestions
        {
            get
            {
                return _shuffledQuestions;
            }
            set
            {
                _shuffledQuestions = value;
                RaisePropertyChanged();
            }
        }
        public Question CurrentQuestion
        {
            get
            {
                return _currentQuestion;
            }
            set
            {
                _currentQuestion = value;
                ShuffledAnswers = ShuffleAnswers(CurrentQuestion);
                RaisePropertyChanged(nameof(CurrentQuestion));
                RaisePropertyChanged(nameof(ShuffledAnswers));
            }
        }
        public List<string> ShuffledAnswers { get; set; }
        public int Score { get; set; }
        public int? TotalQuestions => mainWindowViewModel?.ActivePack?.Questions.Count;
        public int DisplayQuestionNumber => CurrentQuestionIndex + 1;
        public string CorrectAnswer => CurrentQuestion.CorrectAnswer;
        public DelegateCommand AnswerHandlerCommand { get; }
        public DelegateCommand PlayAgainCommand { get; }

        private Brush _answer0BorderBrush;
        public Brush Answer0BorderBrush
        {
            get => _answer0BorderBrush;
            set { _answer0BorderBrush = value; RaisePropertyChanged(nameof(Answer0BorderBrush)); }
        }
        private Brush _answer1BorderBrush;
        public Brush Answer1BorderBrush
        {
            get => _answer1BorderBrush;
            set { _answer1BorderBrush = value; RaisePropertyChanged(nameof(Answer1BorderBrush)); }
        }
        private Brush _answer2BorderBrush;
        public Brush Answer2BorderBrush
        {
            get => _answer2BorderBrush;
            set { _answer2BorderBrush = value; RaisePropertyChanged(nameof(Answer2BorderBrush)); }
        }
        private Brush _answer3BorderBrush;
        public Brush Answer3BorderBrush
        {
            get => _answer3BorderBrush;
            set { _answer3BorderBrush = value; RaisePropertyChanged(nameof(Answer3BorderBrush)); }
        }

        public PlayerViewModel(MainWindowViewModel? mainWindowViewModel) 
        {
            this.mainWindowViewModel = mainWindowViewModel;
            ActivePack = this.mainWindowViewModel.ActivePack;

            IsVisible = Visibility.Collapsed;
            ResultVisibility = Visibility.Hidden;
            ShuffledAnswers = new List<string>();

            timer = new DispatcherTimer 
            { 
                Interval = TimeSpan.FromSeconds(1)
            };
            timer.Tick += Timer_Tick;

            AnswerHandlerCommand = new DelegateCommand(HandleAnswer);
            PlayAgainCommand = new DelegateCommand(PlayAgain);
        }

        private void PlayAgain(object obj)
        {
            ResultVisibility = Visibility.Collapsed;
            IsVisible = Visibility.Visible;
            StartQuiz(mainWindowViewModel.ActivePack.Questions, mainWindowViewModel.ActivePack.TimeLimitInSeconds);
            RaisePropertyChanged(nameof(DisplayQuestionNumber));
            RaisePropertyChanged(nameof(TotalQuestions));
            RaisePropertyChanged(nameof(ActivePack));
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            if (TimeRemaining > 0)
            {
                TimeRemaining--;
            }
            else
            {
                timer.Stop();             
                LoadNextQuestion();
            }
        }

        private async void HandleAnswer(object obj)
        {
            ResetBorderBrush();

            string selectedAnswer = (string)obj;
            int selectedIndex = ShuffledAnswers.IndexOf(selectedAnswer);
            int correctIndex = ShuffledAnswers.IndexOf(CurrentQuestion.CorrectAnswer);

            if (selectedAnswer == CurrentQuestion.CorrectAnswer)
            {
                SetBorderBrush(selectedIndex, Brushes.Green);
                Score++;
            }
            else
            {
                SetBorderBrush(selectedIndex, Brushes.Red);
                SetBorderBrush(correctIndex, Brushes.Green);
            }

            RaisePropertyChanged(nameof(Score));
            await Task.Delay(1000);
            timer.Stop();
            ResetBorderBrush();
            LoadNextQuestion();
            
        }

        public void StartQuiz(ObservableCollection<Question> questions, int timeLimitInSeconds)
        {
            if (ShuffledQuestions == null && questions.Count == 0)
            {
                return;
            }
            else
            {
                ShuffledQuestions = new ObservableCollection<Question>(questions.OrderBy(q => _random.Next()).ToList());
                CurrentQuestionIndex = 0;
                CurrentQuestion = ShuffledQuestions[CurrentQuestionIndex];
                Score = 0;
                TimeRemaining = timeLimitInSeconds;
                timer.Start();
                RaisePropertyChanged(nameof(ShuffledAnswers));
                RaisePropertyChanged(nameof(TotalQuestions));
            }
        }
        public void LoadNextQuestion()
        {
            if (CurrentQuestionIndex < TotalQuestions -1)
            {
                CurrentQuestionIndex++;
                CurrentQuestion = ShuffledQuestions[CurrentQuestionIndex];

                RaisePropertyChanged(nameof(CurrentQuestionIndex));
                RaisePropertyChanged(nameof(ShuffledAnswers));
                RaisePropertyChanged(nameof(DisplayQuestionNumber));
                RaisePropertyChanged(nameof(Score));

                TimeRemaining = mainWindowViewModel.ActivePack.TimeLimitInSeconds;
                timer.Start();
            }
            else
            {
                ResultVisibility = Visibility.Visible;
                IsVisible = Visibility.Collapsed;
                timer.Stop();
            }
        }
        private void SetBorderBrush(int index, Brush color)
        {
            switch (index)
            {
                case 0: Answer0BorderBrush = color; break;
                case 1: Answer1BorderBrush = color; break;
                case 2: Answer2BorderBrush = color; break;
                case 3: Answer3BorderBrush = color; break;
            }
        }
        private void ResetBorderBrush()
        {
            Answer0BorderBrush = Brushes.LightBlue;
            Answer1BorderBrush = Brushes.LightBlue;
            Answer2BorderBrush = Brushes.LightBlue;
            Answer3BorderBrush = Brushes.LightBlue;

            RaisePropertyChanged(nameof(Answer0BorderBrush));
            RaisePropertyChanged(nameof(Answer1BorderBrush));
            RaisePropertyChanged(nameof(Answer2BorderBrush));
            RaisePropertyChanged(nameof(Answer3BorderBrush));
        }
        private List<string> ShuffleAnswers(Question q)
        {
            if (q != null)
            {
                var newList = new List<string>
                {
                    q.CorrectAnswer,
                    q.IncorrectAnswers[0],
                    q.IncorrectAnswers[1],
                    q.IncorrectAnswers[2]
                };
            Shuffle(newList);
            return newList;

            }
            return new List<string>();
        }

        private void Shuffle(List<string> list)
        {
            Random random = new Random();

            for (int i = list.Count - 1; i > 0; i-- )
            {
                int n = random.Next(i + 1);
                var value = list[n];
                list[n] = list[i];
                list[i] = value;
            }
        }
    }
}
