﻿using DogApi.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DogApi.Repositories
{
    internal class DogRepository
    {
        private const string BaseUrl = "https://6784c8c11ec630ca33a59e65.mockapi.io/dogs";
        private readonly HttpClient _httpClient = new();

        public async Task<List<Dog>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync(BaseUrl);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<Dog>>(json);
            }
            return new List<Dog>();
        }

        //metodo para añadir o actualizar un perro en la lista
        public async Task AddOrUpdateAsync(Dog dog)
        {
            var json = JsonSerializer.Serialize(dog);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            if (string.IsNullOrEmpty(dog.Id))
            {
                await _httpClient.PostAsync(BaseUrl, content); // crear perro nuevo
            }
            else
            {
                await _httpClient.PutAsync($"{BaseUrl}/{dog.Id}", content); // actualizar perro
            }
        }

        //metodo para borrar un perro de la lista
        public async Task DeleteAsync(string id)
        {
            await _httpClient.DeleteAsync($"{BaseUrl}/{id}");
        }

        //metodo para buscar un perro por su id
        public async Task<Dog?> GetByIdAsync(string id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{BaseUrl}/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<Dog>(json);
                }
                else
                {
                    Console.WriteLine($"Error: {response.StatusCode}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Excepción: {ex.Message}");
                return null;
            }
        }

    }
}
