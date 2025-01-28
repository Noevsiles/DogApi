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

        public string SearchId { get; set; } = string.Empty;
        public MainPageViewModel()
        {
            Refresh();
        }

        //comando para añadir o actualizar un perro de la lista
        public ICommand AddOrUpdateCommand => new Command(async () =>
        {
            await _repository.AddOrUpdateAsync(CurrentDog);
            Refresh();
            CurrentDog = new Dog(); // limpiar la vista
        });

        //comando para borrar un perro de la lista
        public ICommand DeleteCommand => new Command(async () =>
        {
            if (!string.IsNullOrEmpty(CurrentDog.Id))
            {
                await _repository.DeleteAsync(CurrentDog.Id);
                Refresh();
                CurrentDog = new Dog(); // limpiar la vista
            }
        });

        //comando getDogById (bucar un perro por su id y actualizar la lista para que solo aparezca ese)
        public ICommand GetDogByIdCommand => new Command<string>(async (id) =>
        {
            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    var dog = await _repository.GetByIdAsync(id);
                    Dogs.Clear();

                    if (dog != null)
                    {
                        Dogs.Add(dog); // si se encuentra el id del perro, se mete a la lista
                        CurrentDog = dog; // actualizar el perro 
                    }
                    else
                    {
                        Console.WriteLine($"No se encontró un perro con el ID: {id}");
                    }
                }
                else
                {
                    Console.WriteLine("El ID proporcionado está vacío.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al buscar el perro por ID: {ex.Message}");
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
