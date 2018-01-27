using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Reversi
{
    /// <summary>
    /// Clase en vez de struct porque sino no podemos usarlo como tipo
    /// devuelto en la interface Participante.
    /// </summary>
    class Posicion
    {
        public int Row { get; set; }
        public int Col { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("row: ").Append(Row).Append(" col: ").Append(Col);

            return sb.ToString();
        }
    }
}
