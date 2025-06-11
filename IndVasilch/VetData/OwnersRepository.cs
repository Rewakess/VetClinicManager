using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using VetCore;

namespace VetData
{
    public class OwnersRepository
    {
        public readonly string connectionString;

        public OwnersRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public List<OwnerWithPets> GetOwnersWithPets()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var dataSet = new DataSet();

                using (var ownersAdapter = new SqlDataAdapter("SELECT * FROM Owners", connection))
                {
                    ownersAdapter.Fill(dataSet, "Owners");
                }
                using (var visitsAdapter = new SqlDataAdapter(
                    "SELECT DISTINCT OwnerID, PetName, Age, AnimalType, Breed FROM Visits",
                    connection))
                {
                    visitsAdapter.Fill(dataSet, "Pets");
                }

                var owners = from owner in dataSet.Tables["Owners"].AsEnumerable()
                             select new OwnerWithPets
                             {
                                 OwnerID = owner.Field<int>("OwnerId"),
                                 Name = owner.Field<string>("Name"),
                                 Phone = owner.Field<string>("Phone"),
                                 Email = owner.Field<string>("Email"),
                                 Pets = (from pet in dataSet.Tables["Pets"].AsEnumerable()
                                         where pet.Field<int>("OwnerID") == owner.Field<int>("OwnerId")
                                         select new Pet
                                         {
                                             Name = pet.Field<string>("PetName"),
                                             Age = pet.Field<int>("Age"),
                                             AnimalType = pet.Field<string>("AnimalType"),
                                             Breed = pet.Field<string>("Breed")
                                         }).ToList()
                             };

                return owners.ToList();
            }
        }
        public void AddOwner(string name, string phone, string email)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var command = new SqlCommand("INSERT INTO Owners (Name, Phone, Email) VALUES (@Name, @Phone, @Email)", connection);
                command.Parameters.AddWithValue("@Name", name);
                command.Parameters.AddWithValue("@Phone", phone);
                command.Parameters.AddWithValue("@Email", email);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }
}