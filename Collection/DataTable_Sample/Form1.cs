using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DataTable_Sample
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            DataTable dt = new DataTable("StudentData");

            dt.Columns.Add("StudentID", typeof(string));
            dt.Columns.Add("StudentName", typeof(string));
            dt.Columns.Add("Math", typeof(double));
            dt.Columns.Add("Eng", typeof(double));

            dt.Columns["StudentID"].MaxLength = 10;//長度
            dt.Columns["StudentID"].AllowDBNull = false;//不能空值
            dt.Columns["StudentID"].Unique = true;//建立唯一性

            //StudnetName
            dt.Columns["StudentName"].MaxLength = 10;
            dt.Columns["StudentName"].AllowDBNull = false;


            dataGridView1.DataSource = dt;


            DataRow row = dt.NewRow();

            Random rd = new Random();

            int G_studentID = 0;

            row["StudentID"] = "S000000000";
            row["StudentName"] = G_studentID;
            row["Math"] = Double.Parse((rd.NextDouble() * 100.0).ToString("0.00"));
            row["Eng"] = Double.Parse((rd.NextDouble() * 100.0).ToString("0.00"));

            dt.Rows.Add(row);
        }
    }
}
