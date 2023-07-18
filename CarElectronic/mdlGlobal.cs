using System;
using System.Collections.Generic;
using System.Text;

namespace CarElectronic
{
    public class mdlTLRP01
    {
        public int IDPARAMETRO { get; set; }
        public string NMONICOPARAMETRO { get; set; }
        public string NMONICOPARAMSEC { get; set; }
        public string SERIE { get; set; }
        public string SECUENCIA { get; set; }
        public string DESCRIPCION { get; set; }
        public string VALSTRING { get; set; }
        public int VALNUMERO { get; set; }
        public decimal VALMONTO { get; set; }
    }
    public class mdlTLRM06
    {
        public int IDDETORDEN { get; set; }
        public int IDORDEN { get; set; }
        public string IDTRBSERV { get; set; }
        public int IDSERVICIO { get; set; }
        public decimal PRECIO { get; set; }
        public int CANTIDAD { get; set; }
        public String COMENTARIO { get; set; }
        public String DIRECCIONDETORDEN { get; set; }
        public String DIRLATITUD { get; set; }
        public String DIRLONGUITUD { get; set; }
    }
}
