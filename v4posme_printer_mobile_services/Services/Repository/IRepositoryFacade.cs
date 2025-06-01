﻿using System.Collections.Generic;
using System.Threading.Tasks;

namespace v4posme_printer_mobile_services.Services.Repository;

public interface IRepositoryFacade<T>
{
    Task<int> PosMeInsertAll(List<T> list);
    
    Task<int> PosMeInsert(T model);

    Task PosMeUpdate(T model);

    Task PosMeUpdateAll(List<T> list);
    
    Task<bool> PosMeDelete(T model);
    
    Task<bool> PosMeDeleteAll();
    
    Task<List<T>> PosMeFindAll();

    Task<List<T>> PosMeFindStartAndTake(int start, int take);

    Task<List<T>> PosMeTake10();

    Task<int> PosMeCount();

    Task<T> PosMeFindFirst();
}