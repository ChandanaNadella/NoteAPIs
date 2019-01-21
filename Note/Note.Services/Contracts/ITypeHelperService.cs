namespace Note.Services.Contracts
{
    public interface ITypeHelperService
    {
        bool TypeHasProperties<T>(string fields);
    }
}
