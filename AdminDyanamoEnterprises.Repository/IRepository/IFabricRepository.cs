using AdminDyanamoEnterprises.DTOs.Master;

namespace AdminDyanamoEnterprises.Repository
{
    public interface IFabricRepository
    {
        List<FabricType> GetAllListType();

        string Sp_InsertOrUpdateOrDeleteFabric(FabricTypePageViewModel fabricType);

        string DeleteFabric(int id);
    }
}
