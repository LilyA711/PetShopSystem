﻿using Dapper;
using IA.Autlantico.Entity;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace IA.Autlantico.Repository
{
    public class BookingRepository
    {
        string connectionstring = "Data Source = (LocalDb)\\MSSQLLocalDB;";

        public List<Booking> SelectAll()
        {
            try
            {
                List<Booking> bookings = null;

                string query = @"SELECT [Id]
                                       ,[CheckInDate]
                                       ,[CheckOutDate]
                                       ,[IdAnimal]
                                       ,[IdHosting]
                                       ,[DeletedAt]
                                   FROM [dbo].[tbBooking]";

                using (var connection = new SqlConnection(connectionstring))
                {
                    connection.Open();

                    bookings = connection.Query<Booking>(query).ToList();
                }

                return bookings;
            }
            catch
            {
                throw new Exception("Erro ao buscar lista de reservas.");
            }
        }

        public Booking Select(int id)
        {
            try
            {
                Booking booking = null;

                string query = @"SELECT [Id]
                                       ,[CheckInDate]
                                       ,[CheckOutDate]
                                       ,[IdAnimal]
                                       ,[IdHosting]
                                       ,[DeletedAt]
                                   FROM [dbo].[tbBooking]
                                  WHERE [Id] = @Id AND [DeletedAt] IS NULL";

                using (var connection = new SqlConnection(connectionstring))
                {
                    connection.Open();

                    var parameters = new DynamicParameters();
                    parameters.Add("@Id", id);

                    booking = connection.QuerySingleOrDefault<Booking>(query);
                }

                return booking;
            }
            catch
            {
                throw new Exception("Erro ao buscar reserva.");
            }
        }

        public void Save(Booking booking)
        {
            try
            {
                string query = @"INSERT INTO  [dbo].[tbBooking]
                                              ([CheckInDate]
                                              ,[IdAnimal]
                                              ,[IdHosting])
                                 VALUES
                                           (@CheckInDate
                                           ,@IdAnimal
                                           ,@Hosting)";

                using (var connection = new SqlConnection(connectionstring))
                {
                    connection.Open();

                    var parameters = new DynamicParameters();
                    parameters.Add("@CheckInDate", booking.CheckInDate);
                    parameters.Add("@IdAnimal", booking.IdAnimal);
                    parameters.Add("@IdHosting", booking.IdHosting);

                    connection.Execute(query, parameters);
                }
            }
            catch
            {
                throw new Exception("Erro ao salvar reserva.");
            }
        }

        public void Delete(int id)
        {
            try
            {
                string query = @"UPDATE [dbo].[tbBooking]
                                 SET [DeletedAt] = @DeletedAt
                                 WHERE Id = @Id";

                using (var connection = new SqlConnection(connectionstring))
                {
                    connection.Open();

                    var parameters = new DynamicParameters();
                    parameters.Add("@DeletedAt", DateTime.Now);
                    parameters.Add("@Id", id);

                    connection.Execute(query, parameters);
                }
            }
            catch
            {
                throw new Exception("Erro ao excluir reserva.");
            }
        }

        public void CheckOut(Booking booking)
        {
            try
            {
                string query = @"UPDATE [dbo].[tbBooking]
                                 SET [CheckOutDate] = @CheckOutDate
                                 WHERE Id = @Id";

                using (var connection = new SqlConnection(connectionstring))
                {
                    connection.Open();

                    var parameters = new DynamicParameters();
                    parameters.Add("@CheckOutDate", booking.CheckOutDate);
                    parameters.Add("@Id", booking.Id);

                    connection.Execute(query, parameters);
                }
            }
            catch
            {
                throw new Exception("Erro ao realizar checkout.");
            }
        }

    }
}
