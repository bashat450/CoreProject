using AdminDyanamoEnterprises.DTOs;
using AdminDyanamoEnterprises.DTOs.Master;
using System.Collections.Generic;

namespace AdminDyanamoEnterprises.Repository
{
    public interface IMaterialRepository
    {
        List<MaterialType> GetAllListType();

        string Sp_InsertOrUpdateOrDeleteMaterialType(MaterialTypePageViewModel materialType);

        string DeleteMaterial(int id);
    }
}
