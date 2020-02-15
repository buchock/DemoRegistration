using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Demo.Entity;
using Demo.Common;
using Demo.Repository;

namespace Demo
{
    public class RegistrationService
    {
        RegistrationRepository repo;

        public RegistrationService()
        {
            repo = new RegistrationRepository();
        }

        public ResponseData<List<Employee>> GetAllEmployee()
        {
            ResponseData<List<Employee>> result = new ResponseData<List<Employee>>();

            return result = repo.GetAllEmployee();
        }

        public ResponseData SaveRecord(string firstname, string lastname, int age)
        {            

            ResponseData response = new ResponseData()
            {
                ReturnCode = ResponseCode.Successful
            };

            /* Perform another layer of validation here as business logic */
            string errmsg = string.Empty;

            if (string.IsNullOrEmpty(firstname.Trim()))
            {
                errmsg = Messages.REQUIRED_FIRST_NAME;
            }
            else if (string.IsNullOrEmpty(lastname.Trim()))
            {
                errmsg = Messages.REQUIRED_LAST_NAME;
            }
            else 
            {
                if (age <= 17 || age > 99)
                {
                    errmsg = Messages.INVALID_AGE;
                }
            }


            if (string.IsNullOrEmpty(errmsg))
            {
                /* All parameters are valid, proceed to save record */
                response = repo.SaveEmployee(firstname, lastname, age);
            }
            else
            {
                /* Return failed with error message in the validation*/
                response = new ResponseData()
                {
                    ReturnCode = ResponseCode.Failed,
                    ReturnMessage = errmsg
                };
            }

            return response;
        }
    }
}
