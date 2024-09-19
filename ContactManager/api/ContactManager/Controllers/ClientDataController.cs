using ContactManager.Constants;
using ContactManager.Data;
using ContactManager.Repository;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.IO;

namespace ContactManager.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ClientDataController : ControllerBase
    {
        RepositoryClients _repository;
        public ClientDataController(RepositoryClients repository)
        {
            _repository = repository;
        }
                
        [HttpGet]
        public async Task<List<Client>> GetData()
        {
            return await _repository.GetClientsData();
        }

        [HttpGet("filters")]
        public async Task<List<Client>> FilterData( [FromQuery] string? ClientName,
                                                    [FromQuery] DateTime? ClientDateOfBirth,
                                                    [FromQuery] bool? ClientIsMarried,
                                                    [FromQuery] string? ClientPhone,
                                                    [FromQuery] decimal? ClientSalary)
        {
            return await _repository.FilterClientData(  ClientName,
                                                        ClientDateOfBirth,
                                                        ClientIsMarried,
                                                        ClientPhone,
                                                        ClientSalary);
        }

        [HttpGet("sort")]
        public async Task<List<Client>> SortData([FromQuery] string field)
        {
            return await _repository.SortClientData(field);
        }

        [HttpPost]
        public async Task<OkResult> AddData([FromForm] string ClientName,
                                            [FromForm] DateTime ClientDateOfBirth,
                                            [FromForm] bool ClientIsMarried,
                                            [FromForm] string ClientPhone,
                                            [FromForm] decimal ClientSalary)
        {
            await _repository.AddClientData(ClientName,
                                            ClientDateOfBirth,
                                            ClientIsMarried,
                                            ClientPhone,
                                            ClientSalary);

            return Ok();
        }

        [HttpPost("{CSVData}")]
        public async Task<OkResult> AddCSVData(IFormFile CSVData)
        {
            await _repository.AddClientCSVData(CSVData);

            return Ok();
        }

        [HttpPut("{ClientId}")]
        public async Task<OkResult> ChangeData( int ClientId,
                                                [FromForm] string changedClientName,
                                                [FromForm] DateTime changedClientDateOfBirth,
                                                [FromForm] bool changedClientIsMarried,
                                                [FromForm] string changedClientPhone,
                                                [FromForm] decimal changedClientSalary)
        {
            await _repository.ChangeClientData( ClientId,
                                                changedClientName,
                                                changedClientDateOfBirth,
                                                changedClientIsMarried,
                                                changedClientPhone,
                                                changedClientSalary);

            return Ok();
        }

        [HttpDelete("{ClientId}")]
        public async Task<OkResult> RemoveData(int ClientId)
        {
            await _repository.RemoveClientData(ClientId);

            return Ok();
        }
    }
}
