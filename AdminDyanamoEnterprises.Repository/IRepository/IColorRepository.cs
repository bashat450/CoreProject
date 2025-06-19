using AdminDyanamoEnterprises.DTOs.Master;

public interface IColorRepository
{
    List<ColorType> GetAllListType();

    string Sp_InsertOrUpdateOrDeleteColor(ColorTypePageViewModel colorType); 

    string DeleteColor(int id); 
}
