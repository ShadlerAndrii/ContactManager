using ContactManager.Controllers;
using ContactManager.Data;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;

namespace ContactManager.Repository
{
    public class RepositoryClients
    {
        AppDbContext _dbContext;
        public RepositoryClients(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Client>> GetClientsData()
        {
            return await _dbContext.Clients.ToListAsync();
        }

        public async Task<List<Client>> FilterClientData(   string? ClientName,
                                                            DateTime? ClientDateOfBirth,
                                                            bool? ClientIsMarried,
                                                            string? ClientPhone,
                                                            decimal? ClientSalary)
        {
            var filteredClients = await _dbContext.Clients
                .Where(c => (ClientName == null || c.Name == ClientName)
                && (ClientDateOfBirth == null || c.DateOfBirth == ClientDateOfBirth)
                && (ClientIsMarried == null || c.IsMarried == ClientIsMarried)
                && (ClientPhone == null || c.Phone == ClientPhone)
                && (ClientSalary == null || c.Salary == ClientSalary))
                .ToListAsync();

            return filteredClients;
        }

        public async Task<List<Client>> SortClientData(string rowName)
        {
            var sortedClients = await _dbContext.Clients
                .OrderBy(rowName)
                .ToListAsync();

            // Але, як варіант, switch-case

            return sortedClients;
        }

        public async Task AddClientData(string newClientName,
                                        DateTime newClientDateOfBirth,
                                        bool newClientIsMarried,
                                        string newClientPhone,
                                        decimal newClientSalary)
        {
            Client newClient = new Client()
            {
                Name = newClientName,
                DateOfBirth = newClientDateOfBirth,
                IsMarried = newClientIsMarried,
                Phone = newClientPhone,
                Salary = newClientSalary
            };

            _dbContext.Add(newClient);
            await _dbContext.SaveChangesAsync();
        }

        public async Task AddClientCSVData(IFormFile ClientCSVData)
        {
            List<Client> newClients = Utils.CSVReader.ReadCSV(ClientCSVData);
            _dbContext.AddRange(newClients);
            await _dbContext.SaveChangesAsync();
        }

        public async Task ChangeClientData( int ClientId,
                                            string changedClientName,
                                            DateTime changedClientDateOfBirth,
                                            bool changedClientIsMarried,
                                            string changedClientPhone,
                                            decimal changedClientSalary)
        {
            Client changedClient = new Client()
            {
                Id = ClientId,
                Name = changedClientName,
                DateOfBirth = changedClientDateOfBirth,
                IsMarried = changedClientIsMarried,
                Phone = changedClientPhone,
                Salary = changedClientSalary
            };

            _dbContext.Update(changedClient);
            await _dbContext.SaveChangesAsync();
        }

        public async Task RemoveClientData(int ClientId)
        {
            _dbContext.Remove(_dbContext.Clients.SingleOrDefault(c => c.Id == ClientId));
            await _dbContext.SaveChangesAsync();
        }
    }

    public static class IQueryableExtensions
    {
        public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> source, string propertyName)
        {
            return source.OrderBy(ToLambda<T>(propertyName));
        }

        public static IOrderedQueryable<T> OrderByDescending<T>(this IQueryable<T> source, string propertyName)
        {
            return source.OrderByDescending(ToLambda<T>(propertyName));
        }

        private static Expression<Func<T, object>> ToLambda<T>(string propertyName)
        {
            var parameter = Expression.Parameter(typeof(T));
            var property = Expression.Property(parameter, propertyName);
            var propAsObject = Expression.Convert(property, typeof(object));

            return Expression.Lambda<Func<T, object>>(propAsObject, parameter);
        }
    }
}
