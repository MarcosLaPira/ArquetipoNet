using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BancoHipotecario.Delivery.Application.Clientes.GetClienteByFiltro
{
    public class ClienteResponseDto
    {
        public string NombreEmbozadoTarjeta { get; set; }
        public string Estado { get; set; }
        public int IDPieza { get; set; }
        public string NombreTitular { get; set; }
        public string Cuenta { get; set; }
        public string IDPAQUETE { get; set; }
        public string EleccionDistribucion { get; set; }
        public string DestinoDistribucion { get; set; }
        public string TipoProducto { get; set; }
        public string NumeroDocumento { get; set; }
        public string Domicilio { get; set; }
        public string DomicilioCore { get; set; }
        public string CuitCuil { get; set; }
        public string Gestion { get; set; }
        public string Rescate { get; set; }
        public string Sucursal { get; set; }
        public string CodigoNovedad { get; set; }
        public string IdentificadorPieza { get; set; }
        public string Categoria { get; set; }
        public string EstadoTraduccion { get; set; }
        public int IdEstado { get; set; }
        public string IDTraduccion { get; set; }
        public string Ubicacion { get; set; }
        public string FechaEstado { get; set; }
        public string SecuenciaAdministradora { get; set; }
        public string SecuenciaPermisionaria { get; set; }
    }
}
