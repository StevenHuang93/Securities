using Dapper;
using Exam.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;

namespace Exam
{
    public partial class Form1 : Form
    {
        private static Random rng = new Random();

        int elementAt = 0;

        List<Test> exam = null;

        List<Test> examResult = new List<Test>();

        Test currentExam = new Test();

        int getPointCount = 0;

        int examCount = 50;

        public Form1()
        {
            InitializeComponent();          
        }

        public void LoadExam(string type)
        {
            string sql = "select * from exam where type = @type and year = @year and quarter = @quarter";

            string year = comboBox1.Text;

            string quarter = comboBox2.Text;

            if ((year == "110" && quarter == "4") || (year == "111" && Convert.ToInt16(quarter) != 1))
            {
                return;
            }

            string connectionString = @"Data Source=(localdb)\v2019;Initial Catalog=Securities;Integrated Security=True";

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                var parameters = new { type = type, year = comboBox1.Text, quarter = comboBox2.Text };

                var affectedRows = connection.Query<Test>(sql, parameters);

                //exam = affectedRows.OrderBy(a => rng.Next()).Take(examCount).ToList();

                exam = affectedRows.ToList();
            }

            exam = exam.OrderBy(a => a.ExamID).ToList();

            label1.Text = $"1/{examCount}";
        }

        public void BeginningExam()
        {
            if (exam != null)
            {
                currentExam = exam.ElementAt(elementAt);

                this.richTextBox1.Text = currentExam.Question;


            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ExamProcess("A");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ExamProcess("B");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ExamProcess("C");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            ExamProcess("D");
        }

        private void ExamProcess(string ans)
        {
            currentExam.YourAnswer = ans;

            examResult.Add(currentExam);

            if (currentExam.Answer == ans)
            {
                getPointCount++;
            }

            label1.Text = $"{examResult.Count + 1 }/{examCount}";

            if (examResult.Count == examCount)
            {
                ShowResult();
            }
            else
            {
                elementAt++;

                BeginningExam();
            }
        }

        private void ShowResult()
        {
            //var table = ConvertToDataTable(examResult);

            var table = examResult.Select(x => new
            {
                //ExamID = x.ExamID,
                Question = x.Question,
                Point = x.Point,
                Answer = x.Answer,
                YourAnswer = x.YourAnswer,
                Year = x.Year,
                Quarter = x.Quarter,
                Type = x.Type,
                Note = x.Note
            }).ToList();

            tabControl1.SelectedIndex = 1;

            dataGridView1.DataSource = table;

            dataGridView1.Columns[0].DefaultCellStyle.WrapMode = DataGridViewTriState.True;

            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

            dataGridView1.Refresh();

            label2.Text = $"{getPointCount}/{examCount}";

            label2.AutoSize = true;

            getPointCount = 0;
        }

        public DataTable ConvertToDataTable<T>(IList<T> data)
        {
            PropertyDescriptorCollection properties =
               TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            foreach (PropertyDescriptor prop in properties)
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            foreach (T item in data)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                table.Rows.Add(row);
            }
            return table;

        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.LoadExam("1");

            BeginningExam();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.LoadExam("2");

            BeginningExam();
        }
    }
}
