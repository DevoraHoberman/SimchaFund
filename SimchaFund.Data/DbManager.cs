using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace SimchaFund.Data
{
    public class DbManager
    {
        public string _connectionString;

        public DbManager(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Simcha> GetSimchas()
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM Simcha";
            var simchas = new List<Simcha>();
            conn.Open();
            var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                int id = (int)reader["Id"];
                simchas.Add(new Simcha
                {
                    SimchaId = (int)reader["Id"],
                    Name = (string)reader["Name"],
                    Date = (DateTime)reader["Date"],
                    ContributorCount = ContributorCount(id),
                    Total = TotalforSimcha(id)
                });
            }

            return simchas;
        }

        public int ContributorCount(int id)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT ISNUll(COUNT(ContributorId), 0) FROM Contribution WHERE SimchaId = @id";
            cmd.Parameters.AddWithValue("@id", id);
            conn.Open();
            return (int)cmd.ExecuteScalar();
        }

        public decimal TotalforSimcha(int id)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"SELECT ISNULL(SUM(Amount), 0) FROM Contribution WHERE SimchaId = @id";
            cmd.Parameters.AddWithValue("@id", id);
            conn.Open();
            return (decimal)cmd.ExecuteScalar();
        }

        public string GetSimchaName(int id)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT Name FROM Simcha WHERE Id = @id";
            cmd.Parameters.AddWithValue("@id", id);
            conn.Open();
            return (string)cmd.ExecuteScalar();
        }

        public DateTime GetSimchaDate(int id)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT Date FROM Simcha WHERE Id = @id";
            cmd.Parameters.AddWithValue("@id", id);
            conn.Open();
            return (DateTime)cmd.ExecuteScalar();
        }

        public string GetContributorName(int contribid)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT (FirstName + ' ' + LastName) AS Name FROM Contributor WHERE ID = @id";
            cmd.Parameters.AddWithValue("@id", contribid);
            conn.Open();
            return (string)cmd.ExecuteScalar();
        }

        public List<Contributor> GetContributors()
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM Contributor";
            var contributors = new List<Contributor>();
            conn.Open();
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                contributors.Add(new Contributor
                {
                    Id = (int)reader["Id"],
                    FirstName = (string)reader["FirstName"],
                    LastName = (string)reader["LastName"],
                    CellNumber = (string)reader["CellNumber"],
                    AlwaysInclude = (bool)reader["AlwaysInclude"],
                    CreatedDate = (DateTime)reader["CreatedDate"]
                });
            }
            return contributors;
        }

        public decimal GetDepositAmount(int id)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"SELECT ISNULL(SUM(Amount), 0) FROM Deposits WHERE ContributorID = @id";
            cmd.Parameters.AddWithValue("@id", id);
            conn.Open();
            return (decimal)cmd.ExecuteScalar(); ;
        }

        public decimal GetAmountGiven(int id)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"SELECT ISNULL(SUM(Amount), 0) FROM Contribution WHERE ContributorId = @id";
            cmd.Parameters.AddWithValue("@id", id);
            conn.Open();
            return (decimal)cmd.ExecuteScalar();
        }

        public decimal GetAmountForSimcha(int simchaId, int id )
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"SELECT ISNULL(SUM(Amount), 0) FROM Contribution WHERE SimchaId = @simchaId AND ContributorId = @id ";
            cmd.Parameters.AddWithValue("@simchaId", simchaId);
            cmd.Parameters.AddWithValue("@id", id);
            conn.Open();
            return (decimal)cmd.ExecuteScalar();
        }

        public decimal GetBalance(int id)
        {
            return GetDepositAmount(id) - GetAmountGiven(id);
        }

        public int ContributorTotal()
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT ISNULL(COUNT(*), 0) FROM Contributor";
            conn.Open();
            return (int)cmd.ExecuteScalar();
        }

        public void NewSimcha(Simcha simcha)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"INSERT INTO Simcha (Name, Date)
                                 VALUES(@name, @date)";
            cmd.Parameters.AddWithValue("@name", simcha.Name);
            cmd.Parameters.AddWithValue("@date", simcha.Date);
            conn.Open();
            cmd.ExecuteNonQuery();
        }

        public void NewDeposit(Deposit deposit)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"INSERT INTO Deposits (ContributorId, Amount, Date)
                                 VALUES(@contributorid, @amount, @date)";
            cmd.Parameters.AddWithValue("@contributorid", deposit.ContributorId);
            cmd.Parameters.AddWithValue("@amount", deposit.Amount);
            cmd.Parameters.AddWithValue("@date", deposit.Date);
            conn.Open();
            cmd.ExecuteNonQuery();
        }

        public int NewContributor(Contributor contributor)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"INSERT INTO Contributor (FirstName, LastName, CellNumber, CreatedDate, AlwaysInclude)
                                 VALUES(@firstname, @lastname, @cellnumber, @createddate, @alwaysinclude) SELECT SCOPE_IDENTITY();";
            cmd.Parameters.AddWithValue("@firstname", contributor.FirstName);
            cmd.Parameters.AddWithValue("@lastname", contributor.LastName);
            cmd.Parameters.AddWithValue("@cellnumber", contributor.CellNumber);
            cmd.Parameters.AddWithValue("@createddate", contributor.CreatedDate);
            cmd.Parameters.AddWithValue("@alwaysinclude", contributor.AlwaysInclude);
            conn.Open();
            return (int)(decimal)cmd.ExecuteScalar();
        }

        public void NewDeposit(int id, int initialDeposit)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"INSERT INTO Deposits (ContributorId, Amount, Date)
                                 VALUES(@contributorid, @amount, GETDATE())";
            cmd.Parameters.AddWithValue("@contributorid", id);
            cmd.Parameters.AddWithValue("@amount", initialDeposit);
            conn.Open();
            cmd.ExecuteNonQuery();
        }

        public void EditContributor(Contributor contributor)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"UPDATE Contributor SET FirstName = @firstName, LastName = @lastName,
                                CellNumber = @cellNumber, AlwaysInclude = @alwaysInclude, 
                                CreatedDate = @createdDate WHERE Id = @id";
            cmd.Parameters.AddWithValue("@id", contributor.Id);
            cmd.Parameters.AddWithValue("@firstName", contributor.FirstName);
            cmd.Parameters.AddWithValue("@lastName", contributor.LastName);
            cmd.Parameters.AddWithValue("@cellNumber", contributor.CellNumber);
            cmd.Parameters.AddWithValue("@alwaysInclude", contributor.AlwaysInclude);
            cmd.Parameters.AddWithValue("@createdDate", contributor.CreatedDate);
            conn.Open();
            cmd.ExecuteNonQuery();

        }

        public void UpdateContributions(List<Contribution> contributions, int simchaId)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"DELETE FROM Contribution WHERE SimchaId = @simchaId";
            cmd.Parameters.AddWithValue("@simchaId", simchaId);
            conn.Open();

            cmd.ExecuteNonQuery();

            cmd.CommandText = @"INSERT INTO Contribution (ContributorId, SimchaId, Amount)
                                VALUES(@contributorId, @simchaId, @amount)";

            foreach (var c in contributions)
            {
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@contributorId", c.ContributorId);
                cmd.Parameters.AddWithValue("@simchaId", c.SimchaId);
                cmd.Parameters.AddWithValue("@amount", c.Amount);                
                cmd.ExecuteNonQuery();
            }
             
        }

        public List<History> GetHistory(int contribid)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT Amount, Date FROM Deposits WHERE ContributorID = @contribid";
            conn.Open();
            cmd.Parameters.AddWithValue("@contribid", contribid);
            List<History> histories = new List<History>();
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                histories.Add(new History
                {
                    Amount = (decimal)reader["amount"],
                    Action = "Deposit", 
                    Date = (DateTime)reader["Date"]
                });
            }
            histories.AddRange(GetContributions(contribid));

            return histories;
        }
        public List<History> GetContributions(int contribid)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT Amount FROM Contribution WHERE ContributorId = @contribid";
            conn.Open();
            cmd.Parameters.AddWithValue("@contribid", contribid);
            List<History> contributionHistory = new List<History>();
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                contributionHistory.Add(new History
                {
                    Action = $"Contribution for the {GetSimchaName(contribid)} Simcha",
                    Date = GetSimchaDate(contribid),
                    Amount = -(decimal)reader["Amount"]
                });
            }
            return contributionHistory;
        }
    }

}
