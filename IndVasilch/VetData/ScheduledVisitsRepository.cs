using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using VetCore;

namespace VetData
{
    public class ScheduledVisitsRepository
    {
        private List<ScheduledVisit> visits;
        public readonly string connectionString;

        public ScheduledVisitsRepository(string connectionString)
        {
            visits = new List<ScheduledVisit>();
            this.connectionString = connectionString;
        }

        public void LoadFromDataSet(DataSet dataSet)
        {
            try
            {
                if (!dataSet.Tables.Contains("VISITS") || !dataSet.Tables.Contains("OWNERS"))
                    throw new Exception("Отсутствуют необходимые таблицы.");

                var query = from visit in dataSet.Tables["VISITS"].AsEnumerable()
                            join owner in dataSet.Tables["OWNERS"].AsEnumerable()
                            on visit.Field<int>("OwnerID") equals owner.Field<int>("OwnerID")
                            select new ScheduledVisit
                            {
                                ID = visit.Field<int>("ID"),
                                PetName = visit.Field<string>("PetName"),
                                Age = visit.Field<int>("Age"),
                                OwnerID = visit.Field<int>("OwnerID"),
                                VisitTime = visit.Field<DateTime>("VisitTime"),
                                Reason = visit.Field<string>("Reason"),
                                Breed = visit.Field<string>("Breed"),
                                AnimalType = visit.Field<string>("AnimalType"),
                                OwnerName = owner.Field<string>("Name"),
                                OwnerPhone = owner.Field<string>("Phone"),
                                OwnerEmail = owner.Field<string>("Email")
                            };

                visits = query.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Ошибка загрузки визитов из базы.", ex);
            }
        }

        public IEnumerable<ScheduledVisit> GetUpcomingVisits()
        {
            return visits.Where(v => v.VisitTime >= DateTime.Today)
                         .OrderBy(v => v.VisitTime);
        }
        public IEnumerable<ScheduledVisit> GetAllVisits()
        {
            return visits.OrderBy(v => v.VisitTime);
        }

        public void AddVisit(ScheduledVisit visit)
        {
            if (visit == null)
                throw new ArgumentNullException(nameof(visit));

            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();

                var cmd = new SqlCommand(
                    "INSERT INTO Visits (PetName, Age, OwnerID, VisitTime, Reason, Breed, AnimalType) " +
                    "VALUES (@PetName, @Age, @OwnerID, @VisitTime, @Reason, @Breed, @AnimalType)", conn);

                cmd.Parameters.AddWithValue("@PetName", visit.PetName);
                cmd.Parameters.AddWithValue("@Age", visit.Age);
                cmd.Parameters.AddWithValue("@OwnerID", visit.OwnerID);
                cmd.Parameters.AddWithValue("@VisitTime", visit.VisitTime);
                cmd.Parameters.AddWithValue("@Reason", visit.Reason ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Breed", string.IsNullOrWhiteSpace(visit.Breed) ? (object)DBNull.Value : visit.Breed);
                cmd.Parameters.AddWithValue("@AnimalType", visit.AnimalType);

                cmd.ExecuteNonQuery();
            }
            visits.Add(visit);
        }
        public void DeleteVisit(int visitId)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();

                var cmd = new SqlCommand("DELETE FROM Visits WHERE ID = @ID", conn);
                cmd.Parameters.AddWithValue("@ID", visitId);

                int affected = cmd.ExecuteNonQuery();
                if (affected == 0)
                    throw new Exception("Не удалось удалить визит: ID не найден.");
            }

            var visitToRemove = visits.FirstOrDefault(v => v.ID == visitId);
            if (visitToRemove != null)
            {
                visits.Remove(visitToRemove);
            }
        }

    }
}
