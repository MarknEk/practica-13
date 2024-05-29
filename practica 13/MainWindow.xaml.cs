using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace practica_13
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<Question> questions;
        private int currentQuestionIndex;
        private Stopwatch stopwatch;
        private TimeSpan timeLimit = TimeSpan.FromMinutes(5); // Ограничение времени на тестирование

        public MainWindow()
        {
            InitializeComponent();
            questions = new List<Question>(); // Инициализация переменной questions
            stopwatch = new Stopwatch();
            InitializeQuestions();
            StartTest();
        }

        private void InitializeQuestions()
        {
            questions = new List<Question>
            {
                new Question("Что такое динамическое выделение памяти?",
                             new List<string> {"Процесс выделения памяти во время выполнения программы", "Фиксированный размер памяти", "Выделение памяти во время компиляции"},
                             0),
                new Question("Какие функции используются для динамического выделения памяти в C#?",
                             new List<string> {"Allocate", "Marshal.AllocHGlobal", "Dispose"},
                             1),
                new Question("Как освобождается динамически выделенная память в C#?",
                             new List<string> {"Free", "Dispose", "Clear"},
                             1),
                new Question("Чем отличается стековое выделение памяти от динамического?",
                             new List<string> {"Стековое выделение памяти происходит во время компиляции и имеет фиксированный размер",
                                               "Динамическое выделение памяти происходит во время выполнения и может изменять размер",
                                               "Стековое выделение памяти происходит во время выполнения и может изменять размер"},
                             0),
                new Question("Какие проблемы могут возникнуть при неправильном использовании динамической памяти?",
                             new List<string> {"Утечки памяти", "Ошибки доступа к памяти", "Сегментационные ошибки"},
                             0),
                new Question("Что такое утечка памяти и как ее предотвратить?",
                             new List<string> {"Ситуация, когда программное обеспечение не освобождает выделенную память после ее использования",
                                               "Ситуация, когда память переполняется",
                                               "Ситуация, когда программа не может получить достаточно памяти"},
                             0),
                new Question("Какие методы можно использовать для отладки проблем с динамической памятью?",
                             new List<string> {"Отладчики", "Инструменты анализа памяти", "Механизмы проверки памяти"},
                             1),
                new Question("Какие есть инструменты для анализа использования памяти в C# приложениях?",
                             new List<string> {"JetBrains dotMemory", "Visual Studio Memory Profiler", "Оба варианта"},
                             2),
                new Question("Какие средства .NET Framework предоставляют управление динамической памятью?",
                             new List<string> {"Сборщик мусора", "Классы и методы для явного управления памятью", "Оба варианта"},
                             2),
                new Question("Какие практики следует соблюдать при работе с динамической памятью для повышения производительности и безопасности?",
                             new List<string> {"Избегать утечек памяти", "Правильно освобождать память после использования",
                                               "Избегать использования указателей и следовать принципам безопасности ввода/вывода данных"},
                             2)
            };
        }

        private async void StartTest()
        {
            currentQuestionIndex = 0;
            DisplayQuestion();
            stopwatch = Stopwatch.StartNew();
            await Task.Delay(timeLimit);
            FinishTest();
        }

        private void DisplayQuestion()
        {
            if (currentQuestionIndex < questions.Count)
            {
                var question = questions[currentQuestionIndex];
                QuestionLabel.Content = question.Text;
                AnswersStackPanel.Children.Clear();
                foreach (var option in question.Options)
                {
                    var radioButton = new RadioButton()
                    {
                        Content = option,
                        GroupName = "OptionsGroup"
                    };
                    AnswersStackPanel.Children.Add(radioButton);
                }
            }
            else
            {
                FinishTest();
            }
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            currentQuestionIndex++;
            DisplayQuestion();
        }


        private void FinishTest()
        {
            stopwatch.Stop();
            CheckAnswersAndEvaluate();
            Close();
        }

        private void CheckAnswersAndEvaluate()
        {
            int correctAnswersCount = 0;
            for (int i = 0; i < questions.Count && i < AnswersStackPanel.Children.Count; i++)
            {
                var question = questions[i];
                var selectedRadioButton = (RadioButton)AnswersStackPanel.Children[i];
                if (selectedRadioButton.IsChecked == true && i == question.CorrectOptionIndex)
                {
                    correctAnswersCount++;
                }
            }
            double score = (double)correctAnswersCount / questions.Count * 100;
            MessageBox.Show($"Тест завершен!\nПравильных ответов: {correctAnswersCount} из {questions.Count}\nОценка: {score}%");
        }

    }

    public class Question
    {
        public string Text { get; }
        public List<string> Options { get; }
        public int CorrectOptionIndex { get; }

        public Question(string text, List<string> options, int correctOptionIndex)
        {
            Text = text;
            Options = options;
            CorrectOptionIndex = correctOptionIndex;
        }
    }
}
