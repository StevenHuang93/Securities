using System.Data.SqlClient;
using System.Text.Json.Serialization;
using Dapper;
using Newtonsoft.Json;

namespace Export
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string connectionString = @"Data Source=(localdb)\v2019;Initial Catalog=Securities;Integrated Security=True";

            int year = 107;

            int quarter = 1;

            int type = 1;

            using (var connection = new SqlConnection(connectionString))
            {
                string sql =
                    "select * From [dbo].[Exam] where year = @Year AND Quarter = @quarter and type = @type order by [ExamID]";

                while (year <= 111)
                {
                    while (quarter <= 4)
                    {
                        while (type <= 2)
                        {
                            if ((year == 110 && quarter == 4))
                                break;

                            var affectedRows = connection.Query<Test>(sql, new { Year = year, Quarter = quarter, Type = type });

                            if (affectedRows != null)
                            {
                                var data = JsonConvert.SerializeObject(affectedRows);

                                string jsonFile = $"{year}_Q{quarter}_{type}";

                                File.WriteAllText($"C:\\Users\\virgo\\Downloads\\證券商業務員\\{jsonFile}.json", data);

                                type++;
                            }
                            else
                                break;
                        }

                        if (quarter >= 5)
                            break;
                        else
                        {
                            type = 1;
                            quarter++;
                        }
                    }

                    if (year >= 112)
                        break;
                    else
                    {
                        quarter = 1;
                        year++;
                    }
                }
            }
        }
    }
}