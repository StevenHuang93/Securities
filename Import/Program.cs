using Dapper;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;

namespace Import
{
    class Program
    {
        // 證券交易相關法規與實務 = 1
        // 證券投資與財務分析 = 2

        static string fileName = "111 年第 1 次證券商業務員資格測驗試題";      

        static int year = 111;

        static int quarter = 1;

        static int type = 2;

        static string ansFile = $"{year}_Q{quarter}_{type}.txt";

        static void Main(string[] args)
        {
            var qq = PDFText($"C:\\Users\\virgo\\Downloads\\_證券商業務員\\{fileName}.pdf");

            qq = qq.Skip(51).Take(50).ToList();

            DB(qq, GetAns());

            Console.WriteLine("Done");

            Console.ReadKey();
        }

        public static List<string> PDFText(string path)
        {
            List<string> question = new List<string>();

            string text = string.Empty;

            using (PdfReader reader = new PdfReader(path))
            {
                for (int page = 1; page <= reader.NumberOfPages; page++)
                {
                    var word = PdfTextExtractor.GetTextFromPage(reader, page).Split('\n').ToList();

                    if (page == 1)
                    {
                        word = word.Skip(4).ToList();
                    }

                    foreach (var item in word)
                    {
                        text += item;

                        if (item.Contains("(D)"))
                        {
                            question.Add(text);

                            text = string.Empty;
                        }
                        //Console.WriteLine(item);
                    }
                }
            }

            return question;
        }

        public static void DB(List<string> qq, Dictionary<int, string> ans)
        {
            string sql = "INSERT INTO Exam ([Question], [Year], [Quarter], [Name], [Answer], [Type]) Values (@Question, @Year, @Quarter, @Name, @Answer, @Type);";

            int i = 1;

            foreach (var item in qq)
            {
                Console.WriteLine(item + "\r\n");

                string answer = ans[i];

                string connectionString = @"Data Source=(localdb)\v2019;Initial Catalog=Securities;Integrated Security=True";

                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    var affectedRows = connection.Execute(sql, new { Question = item, Year = year, Quarter = quarter, Name = fileName, Type = type, Answer = answer });
                }

                i++;

            }
        }

        public static Dictionary<int, string> GetAns()
        {
            var ansText = File.ReadAllLines($"C:\\Users\\virgo\\Downloads\\_證券商業務員\\{ansFile}");

            int index = 1;

            Dictionary<int, string> ans = new Dictionary<int, string>();

            foreach (var item in ansText)
            {
                ans.Add(index++, item);
            }

            return ans;
        }
    }
}
