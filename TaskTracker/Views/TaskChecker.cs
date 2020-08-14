using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TaskTracker.Domain;
using TaskTracker.Domain.Interface;
using TaskTracker.Models;

namespace TaskTracker
{

    public partial class TaskChecker : Form
    {
        public IDataAccess da;
        public string user;
        public TaskChecker()
        {
            da = new DataAccess(ConfigurationManager.ConnectionStrings["SqlConnection"].ConnectionString);
            user = Environment.UserName;
            InitializeComponent();
            SetUpScreen();
        }

        public void SetUpScreen()
        {
            var list = da.GetTaskData(user);

            var bindingList = new BindingList<CheckItems>(list);
            var source = new BindingSource(bindingList, null);
            dataGridView1.DataSource = source;
            dataGridView1.Columns[1].Width = 500;

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 2)
            {
                this.dataGridView1.CommitEdit(DataGridViewDataErrorContexts.Commit);

                if ((bool)this.dataGridView1.CurrentCell.Value == true)
                {
                    int id = (int)this.dataGridView1.CurrentRow.Cells[0].Value;
                    if (!da.CheckIfExists(id, user))
                    {
                        da.Insert(id, user);
                        MessageBox.Show("Succes");
                    }
                }
            }
        }
    }
}
