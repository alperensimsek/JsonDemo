using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace JsonDemo
{
    public partial class Form1 : Form
    {
        public JsonPersonRepo PersonRepo { get; set; }

        private readonly string path = $"{AppDomain.CurrentDomain.BaseDirectory}/JsonData";

        public Form1()
        {
            InitializeComponent();

            PersonRepo = new JsonPersonRepo(path);
            
            this.Icon = new Icon($"{AppDomain.CurrentDomain.BaseDirectory}/Images/json-icon.ico");
            
            btnUpdate.Enabled = false;
            btnDelete.Enabled = false;

            RefreshDataGrid();
        }

        #region Methods
        /// <summary>
        /// Refreshes the datagrid.
        /// </summary>
        private void RefreshDataGrid()
        {
            dgwPerson.DataSource = null;
            dgwPerson.DataSource = PersonRepo.Liste;
        }

        /// <summary>
        /// Checks if the textboxes are empty or not.
        /// </summary>
        /// <returns> true if the textboxes are not null otherwise false </returns>
        private bool AreTextBoxesEmpty()
        {
            if (String.IsNullOrEmpty(txbName.Text) || String.IsNullOrEmpty(txbNumber.Text) || String.IsNullOrEmpty(txbCity.Text) || String.IsNullOrEmpty(txbAge.Text))
                return false;
            else
                return true;
        }

        /// <summary>
        /// Reset and clear textboxes.
        /// </summary>
        private void ResetTextBoxes()
        {
            txbName.Text = String.Empty;
            txbNumber.Text = String.Empty;
            txbCity.Text = String.Empty;
            txbAge.Text = String.Empty;
        }

        /// <summary>
        /// Generates ID.
        /// </summary>
        /// <returns>int ID</returns>
        private int CalculateID()
        {
            int returnValue = -1;
            foreach (var person in PersonRepo.Liste)
            {
                if (returnValue <= person.ID)
                    returnValue = person.ID;
            }
            return returnValue + 1;
        }
        #endregion

        #region Events

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (AreTextBoxesEmpty())
            {
                PersonRepo.Liste.Add(new Person
                {
                    ID = CalculateID(),
                    Name = txbName.Text,
                    Number = txbNumber.Text,
                    City = txbCity.Text,
                    Age = Convert.ToUInt16(txbAge.Text)

                });
                PersonRepo.Save();
                RefreshDataGrid();
                ResetTextBoxes();
            }
            else
                MessageBox.Show("A property is empty!");

        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (AreTextBoxesEmpty())
            {
                int id = Convert.ToInt32(dgwPerson.CurrentRow.Cells[0].Value);
                if (PersonRepo.Liste.Find(p => p.ID == id) != null)
                {
                    PersonRepo.Liste.Find(p => p.ID == id).Name = txbName.Text;
                    PersonRepo.Liste.Find(p => p.ID == id).Number = txbNumber.Text;
                    PersonRepo.Liste.Find(p => p.ID == id).City = txbCity.Text;
                    PersonRepo.Liste.Find(p => p.ID == id).Age = Convert.ToUInt16(txbAge.Text);
                    PersonRepo.Save();
                    RefreshDataGrid();
                    ResetTextBoxes();
                    btnUpdate.Enabled = false;
                    btnDelete.Enabled = false;
                }
            }
            else
                MessageBox.Show("A property is empty!");
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(dgwPerson.CurrentRow.Cells[0].Value);
            Person person = PersonRepo.Liste.Find(p => p.ID == id);
            if (person != null)
            {
                PersonRepo.Liste.Remove(person);
                PersonRepo.Save();
                RefreshDataGrid();
                ResetTextBoxes();
                btnUpdate.Enabled = false;
                btnDelete.Enabled = false;
            }
        }

        private void dgwPerson_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            ResetTextBoxes();
            int id = Convert.ToInt32(dgwPerson.CurrentRow.Cells[0].Value);
            Person person = PersonRepo.Liste.Find(p => p.ID == id);
            if (person != null)
            {
                txbName.Text = person.Name;
                txbNumber.Text = person.Number;
                txbCity.Text = person.City;
                txbAge.Text = person.Age.ToString();
                btnUpdate.Enabled = true;
                btnDelete.Enabled = true;
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            dgwPerson.CurrentCell.Selected = false;
            dgwPerson.Rows[0].Cells[0].Selected = true;
            ResetTextBoxes();
        }

        #endregion

    }
}
