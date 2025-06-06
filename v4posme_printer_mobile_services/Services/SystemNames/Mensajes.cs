﻿namespace v4posme_printer_mobile_services.Services.SystemNames;

public static class Mensajes
{
    public const string MensajeCredencialesInvalida             = "Credenciales incorrectas o nombre de compañía no existe. Inténtalo nuevamente.";
    public const string MensajeSinDatosTabla                    = "No hay datos ingresados en el celular para buscar usuario, conectarse a internet para descargar datos";
    public const string MensajeDownloadError                    = "No fue posible descargar los datos, revise su conexion a internet e intente nuevamente.";
    public const string MensajeDownloadSuccess                  = "Se han descargado los datos de forma correcta.";
    public const string MensajeDownloadCantidadTransacciones    = "No puede realizar la descarga sin antes subir la información pendiente.";
    public const string MensajeUploadCantidadTransacciones      = "No puede realizar la subida de datos ya que no hay datos a subir.";
    public const string MensajeUploadError                      = "No fue posible realizar la subida de datos debido a un error interno o perdida de conexión a internet.";
    public const string MensajeUploadSuccess                    = "Se han subido los datos de forma correcta al servidor.";
    public const string MensajeParametrosGuardar                = "Se han guardado los parametros de forma correcta";
    public const string MensajeDocumentCreditCustomerVacio      = "No hay datos de facturación con el cliente seleccionado.";
    public const string MensajeDocumentCreditAmortizationVacio  = "No hay datos de detalle para abono de factura con el documento seleccionado";
    public const string MnesajeCountadoDeAbonoMalFormado        = "El countador de los abonos tiene un formato incorrecto, ABC-#";
	public const string MensajeCountadorDeVisitaMalFormado      = "El countador de las visitas tiene un formato incorrecto, VST-#";
	public const string AnularAbonoValidacion                   = "No puede eliminar este abono, intente nuevamente";
    public const string MensajeMontoMenorIgualCero              = "Debe especificar un monto mayor a 0";
    public const string MensajeSaldoNegativo                    = "No se puede ingresar un saldo negativo";
    public const string MensajeValorZero                        = "No puede ingresar un valor en 0";
    public const string MensajeBluetoothState                   = "El Bluetooth se encuentra desactivado";
    public const string MensajeCompartirError                   = "No fue posible realizar la captura de los datos para compartir";
    public const string MensajeDispositivoNoConectado           = "No está conectado el dispositivo al celular o nombre es incorrecto";
    public const string MensajeCampoRequerido                   = "Todos los campos son requeridos, intente nuevamente.";
    public const string MensajeCompartirComprobante             = "Compartir Comprobante de Factura";
    public const string MensajeSeleccionarMoneda                = "Seleccione una moneda para continuar";
    public const string MensajeSeleccionarFrecuenciaPago        = "Seleccione una frecuencia de pago";
    public const string MensajeSeleccionarTipoDocumento         = "Seleccione un tipo de documento para continuar";
    public const string MensajeCompartirComprobanteAbono        = "Compartir Comprobante de abono";
    public const string MensajeEspecificarDatosGuardar          = "Debe especificar los datos a guardar";
    public const string MensajeCantidadImprimir                 = "Debe ser mayor a cero la cantidad a imprimir";
    public const string MensajeSinDatos                         = "No hay registros con el filtro indicado";
    public const string MensajeSeleccionarTipoPago              = "Debe seleccionar un tipo de pago para continuar";
    public const string MensajeSeleccionarProductos             = "Seleccione productos para continuar";
    public const string MonedaCordoba                           = "NIO";
    public const string MonedaDolar                             = "USD";
    public const string AuthTokenError                          = "No fue posible generar el token";
    public const string MensajeCompania                         = "Debe especificar nombre de compañia";
    public const string MessagePermisosEscrituraNotConcedidos   = "Permiso de escritura no concedido";
    public const string MessageCommentEmpty                     = "El comentario no puede estar vacio";
    public const string MessageVisitTipificationEmpty           = "Seleccionar una tipificacion";
    public const string MessageDateMoreThan                     = "La fecha no puede ser menor al dia de hoy";
    public const string MessageErrorVisita                      = "Ha ocurrido un error al guardar la visita";
	public const string MessageOkVisita                         = "Se ha registrado la visita correctamente";
    public const string MessagePermissionDenied                 = "El permisio es requerido para el funcionanamiento de la apliacion";
	public const string MessagePermissionShouldAccept           = "El permiso debe otorgarse manualmente (Permiso de Localizacion y Dispositivo Bluetooth) para obtener los dispositivos cercanos";
	public const string MessagePermission                       = "El permiso {name} es necesario para continuar. Asigne el permiso {name} de forma manual, de lo contrario no podrá ingresar al sistema.";
	public const string MessagePermissionAll                    = "No es posible ingresar al sistema, es necesario conceder todos los permisos, concederlos de forma manual.";
	public const string ExisteItem                              = "Ya existe el item con el código de barra";
	public const string ExisteCustomerIdentificacion            = "Ya existe el cliente con la identificación";
	public const string ParametroImpresora                      = "No hay un parametro para conectar la impresora, establezca primero el parametro para proceder a imprimir";
	public const string UsuarioNoExiste                         = "No existe el usuario en la aplicaicón";
	public const string CompanyNoExiste                         = "No existe la compañía o no se ha inicializado";
	public const string ArchivoNoExiste                         = "El archivo no existe o no fue posible crearlo";
	public const string AnularFacturaCorrectamente              = "Factura anulada correctamente";
	public const string AnularAbonoCorrectamente                = "Abono anulado correctamente";
	public const string UsuarioNoPermitido                      = "No se permite el usuario en la aplicación ya que hay datos por subir. Envie los datos antes de cambiar de usuario";
}