using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Demo.Entity;
using System.Data.SqlClient;
using Demo.Common;
using System.Data;

namespace Demo.Repository
{
    public class RegistrationRepository
    {
        public RegistrationRepository()
        {
            
        }

        public ResponseData<List<Employee>> GetAllEmployee()
        {
            ResponseData<List<Employee>> result = new ResponseData<List<Employee>>()
            {
                ReturnCode = ResponseCode.Successful,
                ReturnMessage = "Success"
            };

            try
            {
                DataTable dt = new DataTable();
                List<Employee> employeeList = new List<Employee>();

              
                using (var con = new SqlConnection { ConnectionString = Utility.GetConnectionString("Conn_DemoDB") })
                {
                    using (var cmd = new SqlCommand { Connection = con, CommandType = CommandType.StoredProcedure })
                    {
                        cmd.CommandText = "dbo.GetAllEmployee";

                        con.Open();

                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(dt);

                        foreach (DataRow dr in dt.Rows)
                        {
                            Employee emp = new Employee()
                            {
                                Id = Convert.ToInt32(dr["Id"].ToString()),
                                FirstName = dr["FirstName"].ToString(),
                                LastName = dr["LastName"].ToString(),
                                Age = Convert.ToInt32(dr["Age"].ToString()),
                                CreatedDate = Convert.ToDateTime(dr["CreatedDate"].ToString())
                            };

                            /* Note: Modified Date is nullable, then check if has value before converting to DateTime
                             */
                            string modifieddate = dr["ModifiedDate"].ToString();
                            if (!string.IsNullOrEmpty(modifieddate))
                            {
                                emp.ModifiedDate = Convert.ToDateTime(modifieddate);
                            }

                            /* Add employee to the list */
                            employeeList.Add(emp);
                        }

                        /* Append the list of employee to the result */
                        result.ReturnData = employeeList;
                    }
                }
            }
            catch (Exception e)
            {
                result.ReturnCode = ResponseCode.Failed;
                result.ReturnMessage = "An error occurred while saving your request";
            }

            return result;
        }

        public ResponseData SaveEmployee(string firstname, string lastname, int age)
        {
            ResponseData result = new ResponseData()
            {
                ReturnCode = ResponseCode.Successful,
                ReturnMessage = "Record successfully saved"
            };

            try
            {
                DataTable dt = new DataTable();
                int code = 0;
                string msg = string.Empty;

                using (var con = new SqlConnection { ConnectionString = Utility.GetConnectionString("Conn_DemoDB") })
                {
                    using (var cmd = new SqlCommand { Connection = con, CommandType = CommandType.StoredProcedure })
                    {
                        cmd.CommandText = "dbo.SaveEmployee";

                        cmd.Parameters.Add(new SqlParameter { ParameterName = "@strFirstName", SqlDbType = SqlDbType.VarChar });
                        cmd.Parameters.Add(new SqlParameter { ParameterName = "@strLastName", SqlDbType = SqlDbType.VarChar });
                        cmd.Parameters.Add(new SqlParameter { ParameterName = "@intAge", SqlDbType = SqlDbType.Int });

                        cmd.Parameters["@strFirstName"].Value = firstname;
                        cmd.Parameters["@strLastName"].Value = lastname;
                        cmd.Parameters["@intAge"].Value = age;

                        con.Open();

                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(dt);

                        foreach (DataRow dr in dt.Rows)
                        {
                            code = Convert.ToInt32(dr["ReturnCode"].ToString());
                            msg = dr["ReturnMessage"].ToString();
                        }
                    }
                }

                if (code != ResponseCode.Successful)
                {
                    result.ReturnCode = ResponseCode.Failed;
                    result.ReturnMessage = msg;
                }
            }
            catch (Exception e)
            {
                result = new ResponseData()
                {
                    ReturnCode = ResponseCode.Failed,
                    ReturnMessage = "An error occurred while saving your request"
                };
            }

            return result;
        }
    }
}
