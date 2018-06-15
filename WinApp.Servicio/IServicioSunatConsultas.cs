namespace WinApp.Servicio
{
    public interface IServicioSunatConsultas : IServicioSunat
    {
        RespuestaSincrono ConsultarConstanciaDeRecepcion(DatosDocumento request);
    }
}
