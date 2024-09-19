using ContactManager.Data;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Reflection.Metadata.Ecma335;

namespace ContactManager.Utils
{
    public class CSVReader
    {
        public static List<Client> ReadCSV(IFormFile CSV)
        {
            StreamReader reader = null;

            if (CSV != null)
            {
                reader = new StreamReader(CSV.OpenReadStream());
                List<Client> clients = new List<Client>();

                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    List<string> data = line.Split(',').ToList();

                    Client newClient = new Client() 
                    {
                        Name = data[0],
                        DateOfBirth = DateTime.Parse(data[1]),
                        IsMarried = bool.Parse(data[2]),
                        Phone = data[3],
                        Salary = decimal.Parse(data[4]),
                    };
                    clients.Add(newClient);
                }
                                
                reader.Close();
                return clients;
            }
            else
            {      
                return new List<Client>();
            }
        }
    }
}
