using DogApi.MVVM.Model;
using DogApi.Repositories;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DogApi.MVVM.ViewModel
{
    [AddINotifyPropertyChangedInterface]
    public class MainPageViewModel
    {
        private readonly DogRepository _repository = new();
        public ObservableCollection<Dog> Dogs { get; set; } = new ObservableCollection<Dog>();
        public Dog CurrentDog { get; set; } = new Dog();

        public MainPageViewModel()
        {
            Refresh();
        }

        public ICommand AddOrUpdateCommand => new Command(async () =>
        {
            await _repository.AddOrUpdateAsync(CurrentDog);
            Refresh();
            CurrentDog = new Dog(); // limpiar la vista
        });

        public ICommand DeleteCommand => new Command(async () =>
        {
            if (!string.IsNullOrEmpty(CurrentDog.Id))
            {
                await _repository.DeleteAsync(CurrentDog.Id);
                Refresh();
                CurrentDog = new Dog(); // limpiar la vista
            }
        });

        private async void Refresh()
        {
            Dogs.Clear();
            var dogsFromApi = await _repository.GetAllAsync();
            foreach (var dog in dogsFromApi)
            {
                Dogs.Add(dog);
            }
        }
    }
}
