using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using SharedModels;
using HotellAPI;

namespace Hotel.FrontdeskApp
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl = "https://localhost:7131/"; // <-- Husk riktig adresse!


            public ApiService()
            {
            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
            };

            _httpClient = new HttpClient(handler);
        }

            // Hent alle rom
            public async Task<List<Room>> GetRoomsAsync()
            {
                var response = await _httpClient.GetAsync($"{_baseUrl}api/rooms");
                response.EnsureSuccessStatusCode();
                var rooms = await response.Content.ReadFromJsonAsync<List<Room>>();
                return rooms ?? new List<Room>();
            }

            // Hent alle reservasjoner
            public async Task<List<Reservation>> GetReservationsAsync()
            {
                var response = await _httpClient.GetAsync($"{_baseUrl}api/reservations");
                response.EnsureSuccessStatusCode();
                var reservations = await response.Content.ReadFromJsonAsync<List<Reservation>>();
                return reservations ?? new List<Reservation>();
            }

            // Hent alle oppgaver (tasks)
            public async Task<List<SharedModels.Task>> GetTasksAsync()
            {
                var response = await _httpClient.GetAsync($"{_baseUrl}api/tasks");
                response.EnsureSuccessStatusCode();
                var tasks = await response.Content.ReadFromJsonAsync<List<SharedModels.Task>>();
                return tasks ?? new List<SharedModels.Task>();
            }

        // sende en Reservation-modell til API-et med POST
        public async Task<bool> CreateReservationAsync(Reservation newReservation)
        {
            var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}api/reservations", newReservation);
            return response.IsSuccessStatusCode;
        }

        // sende en DELETE forespørsel til API-et for å slette en reservasjon med gitt ID
        public async Task<bool> DeleteReservationAsync(int reservationId)
        {
            var response = await _httpClient.DeleteAsync($"{_baseUrl}api/reservations/{reservationId}");
            return response.IsSuccessStatusCode;
        }

        // sende en Task-modell til API-et med en POST-forespørsel.
        public async Task<bool> CreateTaskAsync(SharedModels.Task newTask)
        {
            var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}api/tasks", newTask);
            return response.IsSuccessStatusCode;
        }

        // Nå kan du hente en Task, endre TaskStatus, og sende en PUT-forespørsel for å oppdatere den!
        public async Task<bool> UpdateTaskStatusAsync(int taskId, string newStatus)
        {
            var existingTask = await _httpClient.GetFromJsonAsync<SharedModels.Task>($"{_baseUrl}api/tasks/{taskId}");

            if (existingTask == null)
                return false;

            existingTask.TaskStatus = newStatus;

            var response = await _httpClient.PutAsJsonAsync($"{_baseUrl}api/tasks/{taskId}", existingTask);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateReservationAsync(Reservation reservation)
        {
            var response = await _httpClient.PutAsJsonAsync($"{_baseUrl}api/reservations/{reservation.ReservationId}", reservation);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateRoomAsync(Room room)
        {
            var response = await _httpClient.PutAsJsonAsync($"{_baseUrl}api/rooms/{room.RoomId}", room);
            return response.IsSuccessStatusCode;
        }
    }

}

