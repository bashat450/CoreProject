using AdminDyanamoEnterprises.DTOs.Master;
using AdminDyanamoEnterprises.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminDyanamoEnterprises.Repository.Repository
{
    public class AccountRepositary : IAccountRepositary
    {
       

        List<LoginType> IAccountRepositary.GetAllListType()
        {
            return new List<LoginType>();
        }
    }
}
