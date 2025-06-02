using System.Threading.Tasks;
using v4posme_maui.Models;
using v4posme_printer_mobile_services.Services.SystemNames;

namespace v4posme_printer_mobile_services.Services.Repository;

public interface IRepositoryTbParameterSystem : IRepositoryFacade<TbParameterSystem>
{  
    Task<TbParameterSystem> PosMeFindPrinter();
    Task<TbParameterSystem> PosMeFindInterval();
    Task<TbParameterSystem> PosMeFindPrefijo();
    Task<TbParameterSystem> PosMeFindCantidadCopias();
    Task<TbParameterSystem> PosMeFindByName(string name);
}

public class RepositoryTbParameterSystem(DataBase dataBase) : RepositoryFacade<TbParameterSystem>(dataBase), IRepositoryTbParameterSystem
{
    private readonly DataBase dataBase = dataBase;

    public Task<TbParameterSystem> PosMeFindPrinter()
    {
        return dataBase.Database.Table<TbParameterSystem>()
            .FirstOrDefaultAsync(system => system.Name == Constantes.ParametroPrinter);
    }

    public Task<TbParameterSystem> PosMeFindInterval()
    {
        return dataBase.Database.Table<TbParameterSystem>()
            .FirstOrDefaultAsync(system => system.Name == Constantes.ParametroInterval);
    }

    public Task<TbParameterSystem> PosMeFindPrefijo()
    {
        return dataBase.Database.Table<TbParameterSystem>()
            .FirstOrDefaultAsync(system => system.Name == Constantes.ParametroPrefijo);
    }

    public Task<TbParameterSystem> PosMeFindCantidadCopias()
    {
        return dataBase.Database.Table<TbParameterSystem>()
            .FirstOrDefaultAsync(system => system.Name == Constantes.CantidadCopias);
    }

    public Task<TbParameterSystem> PosMeFindByName(string name)
    {
        return dataBase.Database.Table<TbParameterSystem>()
            .FirstOrDefaultAsync(system => system.Name == name);
    }
}