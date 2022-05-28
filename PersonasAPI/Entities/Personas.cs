using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PersonasAPI.Entities
{
    public class Personas
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public string NOMBRE { get; set; }

        public string APELLIDO { get; set; }

        public string DOCUMENTO { get; set; }

        public string TIPO_DOCUMENTO { get; set; }

        public string ESTADO { get; set; }

        public int EDAD { get; set; }
    }
}
