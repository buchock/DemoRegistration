using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Demo.Common;
using Demo.Entity;

namespace Demo
{
    public partial class frmMain : Form
    {
        #region Events
        public frmMain()
        {
            InitializeComponent();

            /* Load list of employees */
            this.UpdateGrid();
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            string errmsg = string.Empty;
            if (!this.IsFormValid(out errmsg))
            {
                ShowErrorMessage(errmsg);
            }
            else
            {
                /* Save Data */
                RegistrationService svc = new RegistrationService();
                ResponseData result = new ResponseData();
                result = svc.SaveRecord
                                (
                                    this.txtFirstName.Text.Trim(),
                                    this.txtLastName.Text.Trim(),
                                    Convert.ToInt32(this.txtAge.Text.Trim())
                                );

                if (result.ReturnCode == ResponseCode.Successful)
                {
                    this.ClearForm();
                    this.UpdateGrid();
                    ShowInfoMessage("Record successfully saved");
                }
                else
                {
                    ShowErrorMessage(result.ReturnMessage);
                }
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            this.ClearForm();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            this.UpdateGrid();
        }

        #endregion

        #region Method

        private void ClearForm()
        {
            this.txtFirstName.Text = string.Empty;
            this.txtLastName.Text = string.Empty;
            this.txtAge.Text = string.Empty;

            this.txtFirstName.Focus();
        }

        private void UpdateGrid()
        {
            List<Employee> employeesList = new List<Employee>();
            
            /* Get list of employees from backend */
            {
                RegistrationService svc = new RegistrationService();
                ResponseData<List<Employee>> data = new ResponseData<List<Employee>>();
                data = svc.GetAllEmployee();

                if (data.ReturnCode == ResponseCode.Successful)
                {
                    employeesList = data.ReturnData;
                }
            }

            /* Bind list to Grid */
            this.dgvEmployee.DataSource = employeesList;
        }
        private void ShowErrorMessage(string msg)
        {
            MessageBox.Show(msg, "Demo", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void ShowInfoMessage(string msg)
        {
            MessageBox.Show(msg, "Demo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private bool IsFormValid(out string errmsg)
        {
            errmsg = string.Empty;
            bool isValid = true;


            if (string.IsNullOrEmpty(this.txtFirstName.Text.Trim())) {
                this.txtFirstName.Text = string.Empty;
                errmsg = Messages.REQUIRED_FIRST_NAME;
                isValid = false;
            } else if (string.IsNullOrEmpty(this.txtLastName.Text.Trim())) {
                this.txtLastName.Text = string.Empty;
                errmsg = Messages.REQUIRED_LAST_NAME;
                isValid = false;
            }
            else if (string.IsNullOrEmpty(this.txtAge.Text.Trim()))
            {
                this.txtAge.Text = string.Empty;
                errmsg = Messages.REQUIRED_AGE;
                isValid = false;
            }
            else
            {
                int age = 0;
                if (int.TryParse(this.txtAge.Text.Trim(), out age))
                {

                    if (age <= 17 || age > 99)
                    {
                        errmsg = Messages.INVALID_AGE;
                        isValid = false;
                    }
                }
                else
                {
                    this.txtAge.Text = string.Empty;
                    errmsg = Messages.INVALID_AGE;
                    isValid = false;
                }
            }

            return isValid;
        }

        #endregion



    }
}
