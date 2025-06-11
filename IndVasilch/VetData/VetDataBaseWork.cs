using System;
using System.Data;
using System.Data.SqlClient;

namespace VetData
{
    public class VetDataBaseWork
    {
        private readonly static string ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;" +
            @"AttachDbFilename=|DataDirectory|\VetDataBase.mdf;" +
            @"Integrated Security=True;Connect Timeout=30";

        private readonly ScheduledVisitsRepository visitsRepository;
        private readonly OwnersRepository ownersRepository;

        public VetDataBaseWork()
        {
            visitsRepository = new ScheduledVisitsRepository(ConnectionString);
            ownersRepository = new OwnersRepository(ConnectionString);
        }



        public ScheduledVisitsRepository VisitsRepository => visitsRepository;
        public OwnersRepository OwnersRepository => ownersRepository;

        public void LoadScheduledVisits()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    SqlDataAdapter visitsAdapter = new SqlDataAdapter("SELECT * FROM Visits ORDER BY VisitTime", connection);

                    SqlDataAdapter ownersAdapter = new SqlDataAdapter("SELECT * FROM Owners", connection);

                    DataSet dataSet = new DataSet();

                    visitsAdapter.Fill(dataSet, "VISITS");
                    ownersAdapter.Fill(dataSet, "OWNERS");

                    visitsRepository.LoadFromDataSet(dataSet);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Ошибка загрузки расписания визитов из базы ", ex);
            }
        }
    }
}
