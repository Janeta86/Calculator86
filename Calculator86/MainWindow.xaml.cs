using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Windows;
using Calculator86_Tests;
using CommunityToolkit.Mvvm.Input;

namespace Calculator86
{

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
        }
    }

    public class MainViewModel : INotifyPropertyChanged 
    {
        // Экран на который все выводим
        public string Screen
        {
            get => _screen;
            set
            {
                if (_screen == value) return;
                _screen = value;
                OnPropertyChanged();
            }
        }

        // Обработчик клавиш
        public RelayCommand<string> ButtonPressedCommand { get; } 

        private string _screen = string.Empty; 

        private Stack<decimal> _memory; 

        private const string _dataFileName = "memory.json"; 

        public MainViewModel()  
        {
            try
            {
                Load(); 
            }
            catch
            {
                _memory = new Stack<decimal>(); 
            }

            ButtonPressedCommand = new RelayCommand<string>((arg) =>  
            {
                switch (arg)
                {
                    case "AC":
                        Screen = string.Empty;
                        break;
                    case "=":
                        Calculate();
                        break;
                    case "MC": //очистка из памяти 
                        _memory.Clear();
                        Save();
                        break;
                    case "MS": //сохранение
                        try
                        {
                            _memory.Push(decimal.Parse(Screen, CultureInfo.InvariantCulture)); 
                            Save();
                            Screen = string.Empty; //очистка экрана 
                        }
                        catch 
                        {
                            Screen = "Ошибка";
                        }

                        break;
                    case "MR": //вывод из памяти на дисплей
                        {
                        if (_memory.Count != 0)
                            Screen += _memory.Pop().ToString(CultureInfo.InvariantCulture); 
                        break;
                    }
                    default:
                        Screen += arg;
                        break;
                }
            });
        }

        private void Calculate()
        {
            var expression = new MathExpression(Screen); 
            try
            {
                Screen = expression.Parse().ToString(CultureInfo.InvariantCulture); 
            }
            catch
            {
                Screen = "Ошибка";
            }
        }

        // Чтение памяти из файла
        private void Load()
        {
            var jsonString = File.ReadAllText(_dataFileName); 
            _memory = new Stack<decimal>(JsonSerializer.Deserialize<Stack<decimal>>(jsonString)!); 
        }

        // Сохранение памяти в файл
        private void Save()
        {
            var jsonString = JsonSerializer.Serialize(_memory); 
            File.WriteAllText(_dataFileName, jsonString); 
        }

        // ==== Для паттерна MVVM ==== 
        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }


}
