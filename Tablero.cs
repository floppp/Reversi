using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reversi
{

    class Tablero
    {
        /* Ancho y alto dos unidades mayor al estrictamente necesario para el
        juego, una posición por cada lado, para facilitar la comprobación de
        las reglas del mismo */
        static readonly int ANCHO = 10;
        static readonly int ALTO  = 10;

        private char[,] board;

        public Tablero()
        {
            this.board = new char[ANCHO, ALTO];
        }

        /// <summary>
        /// Devolvemos el tablero
        /// </summary>
        /// <returns>Tablero con la situación de juego</returns>
        internal char[,] GetBoard()
        {
            return this.board;
        }

        /// <summary>
        /// Modificamos la posición jugada
        /// </summary>
        /// <param name="row">Fila</param>
        /// <param name="col">Columna</param>
        /// <param name="color">Color del jugador que realiza el 
        /// movimiento</param>
        internal void SetBoard(int row, int col, Color color)
        {
            this.board[row, col] = (char) color;
        }

        /// <summary>
        /// Inicializamos el tablero vacío y colocamos las cuatro fichas
        /// iniciales
        /// </summary>
        internal void Inicio()
        {
            for (int i = 0; i < ALTO; i++)
                for (int j = 0; j < ANCHO; j++)
                    this.board[i, j] = (char) Color.VACIO;

            this.board[4, 4] = this.board[5, 5] = (char) Color.BLANCO;
            this.board[5, 4] = this.board[4, 5] = (char) Color.NEGRO;
        }

        /// <summary>
        /// Llamamos al método 'GenerarTablero'
        /// </summary>
        internal void MostrarTablero()
        {
            Console.OutputEncoding = System.Text.Encoding.GetEncoding(1252);

            GenerarTablero();
        }

        /// <summary>
        /// Imprimimos el tablero con la situación actual de juego
        /// </summary>
        private void GenerarTablero()
        {
            /* Imprimimos numeros de columnas */
            Console.Write("      ");
            for (int i = 1; i < ANCHO - 1; i++)
                Console.Write(" {0}  ", i);

            /* Imprimimos tablero en su situación actual */
            Console.Write("\n");
            ImprimirTapaSuperior();

            for (int row = 1; row < ALTO - 2; row++)
            {
                ImprimirLineaTablero(row);
                ImprimirLineaSeparacion();
            }
            ImprimirLineaTablero(8);
            ImprimirTapaInferior();
        }

        /// <summary>
        /// Imprimimos línea a línea del tablero
        /// </summary>
        /// <param name="i">Fila a imprimir</param>
        private void ImprimirLineaTablero(int row)
        {
            Console.Write(" {0}", row);
            for (int col = 1; col < ANCHO; col++)
                Console.Write(" {0} " + (char) 179, this.board[row, col - 1]);
            Console.Write("\n  ");
        }

        /// <summary>
        /// Imprimimos la línea horizontal superior del tablero
        /// </summary>
        private void ImprimirTapaSuperior()
        {
            Console.Write("     " + (char)218);
            for (int j = 1; j < 8; j++)
                Console.Write((char)196 + "" + (char)196 + "" + (char)196
                    + "" + (char)194);

            Console.Write((char)196 + "" + (char)196 + "" + (char)196 +
                    "" + (char)191);
            Console.Write("\n");
        }

        /// <summary>
        /// Imprimimos la línea horizontal inferior del tablero
        /// </summary>
        private void ImprimirTapaInferior()
        {
            Console.Write("   " + (char)192);
            for (int j = 1; j < 8; j++)
                Console.Write((char)196 + "" + (char)196 + "" + (char)196
                    + "" + (char)193);

            Console.Write((char)196 + "" + (char)196 + "" + (char)196 +
                    "" + (char)217);
            Console.Write("\n");
        }

        /// <summary>
        /// Imprimimos lineas de separación entre líneas del tablero para
        /// mejorar la presentación del mismo
        /// </summary>
        private void ImprimirLineaSeparacion()
        {
            Console.Write("   " + (char) 195);
            for (int j = 1; j < 8; j++)
                Console.Write((char) 196 + "" + (char) 196 + "" + (char) 196 
                    + "" + (char) 197);
            
            Console.Write((char) 196 + "" + (char) 196 + "" + (char) 196 +
                    "" + (char)180);
            Console.Write("\n");
        }

        public void Copiar(Tablero tablero)
        {
            for (int row = 1; row < 9; row++)
                for (int col = 1; col < 9; col++)
                    this.board[row, col] = tablero.GetBoard()[row, col];
        }
    }
}
