﻿using System.Collections.Generic;
using System.Threading.Tasks;

namespace v4posme_printer_mobile_services.Services.Repository;

public abstract class RepositoryFacade<T>(DataBase dataBase) : IRepositoryFacade<T> where T : new()
{
    public Task<int> PosMeInsertAll(List<T> list)
    {
        return dataBase.Database.InsertAllAsync(list);
    }

    public Task<int> PosMeInsert(T model)
    {
        return dataBase.Database.InsertAsync(model);
    }

    public Task PosMeUpdate(T model)
    {
       return dataBase.Database.UpdateAsync(model);
    }

    public async Task PosMeUpdateAll(List<T> list)
    {
        await dataBase.Database.UpdateAllAsync(list);
    }

    public async Task<bool> PosMeDelete(T model)
    {
        return await dataBase.Database.DeleteAsync(model)>0;
    }
    public async Task<bool> PosMeDeleteAll()
    {
        return await dataBase.Database.DeleteAllAsync<T>()>0;
    }

    public async Task<List<T>> PosMeFindAll()
    {
        return await dataBase.Database.Table<T>().ToListAsync();
    }

    public async Task<List<T>> PosMeFindStartAndTake(int lastLoadedIndex, int loadBatchSize)
    {
        return await dataBase.Database.Table<T>().Skip(lastLoadedIndex).Take(loadBatchSize).ToListAsync();
    }

    public Task<List<T>> PosMeTake10()
    {
        return dataBase.Database.Table<T>().Take(10).ToListAsync();
    }

    public async Task<int> PosMeCount()
    {
        return await dataBase.Database.Table<T>().CountAsync();
    }

    public Task<T> PosMeFindFirst()
    {
        return dataBase.Database.Table<T>().FirstOrDefaultAsync();
    }
}