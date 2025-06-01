using Unity;

namespace v4posme_printer_mobile_services.Services.SystemNames;

public static class VariablesGlobales
{
    public static readonly UnityContainer UnityContainer;

    static VariablesGlobales()
    {
        UnityContainer      = new UnityContainer();
    }
}