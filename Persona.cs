using System;

namespace Reversi
{
    /// <summary>
    /// Clase que representa a un jugador humano
    /// </summary>
    class Persona : Participante
    {
        private Color color;

        private Persona(int numeroJugador)
        {
            if (numeroJugador == 1) color = Color.NEGRO;
            else                    color = Color.BLANCO;
        }

        public static Persona getInstance(int numeroJugador)
        {
            return new Persona(numeroJugador);
        }

        /// <summary>
        /// Obtenemos el color del jugador
        /// </summary>
        /// <returns>Color</returns>
        public Color GetColor()
        {
            return this.color;
        }

        /// <summary>
        /// Método para pedir posición al jugador
        /// </summary>
        /// <param name="tablero">Tablero en su sitúación actual</param>
        public void Movimiento(Tablero tablero)
        {
            Posicion posicion = null;
            Boolean movimientoValido = false, pasa = false;
            int piezasComidas = 0;
            /* Pedimos posición de juego al jugador hasta que la que
            introduzca cumpla con las reglas del juego */
            do
            {
                posicion = ElegirPosicion(tablero);

                if (posicion.Col == 0)
                    if (posicion.Row == 0)
                    {
                        pasa = true;
                        break;
                    }
                movimientoValido = ComprobarJugada(tablero, posicion, color);
            
                if (!movimientoValido)
                    Console.WriteLine("Jugada " +
                        "INCORRECTA. Vuelve a introducir posición.");
            } while (!movimientoValido);

            /* Cambiamos de color las fichas 'comidas' y colocamos la pieza
            en la posición elegida en caso de no pasar turno */
            if (!pasa)
            {
                Reglas.CambioDeColor(tablero, color, posicion.Row,
                        posicion.Col, out piezasComidas);
                ActualizarTablero(tablero, posicion.Row, posicion.Col);
            }
        }

        /// <summary>
        /// Método para actualizar tablero
        /// </summary>
        /// <param name="jugador">Participante que está realizando
        /// movimiento</param>
        /// <param name="row">Fila</param>
        /// <param name="col">Columna</param>
        public void ActualizarTablero(Tablero tablero, int row, int col)
        {
            tablero.SetBoard(row, col, color);
            tablero.MostrarTablero();
        }

        /// <summary>
        /// Hacemos dos comprobaciones, que la posición está dentro del tablero
        /// y que cumple con las reglas del juego.
        /// </summary>
        /// <param name="tablero">Tablero en la situación actual de juego
        /// </param>
        /// <param name="posicion">Posición elegida por el jugador cuya
        /// legalidad queremos comprobar</param>
        /// <param name="color">Color del jugador</param>
        /// <returns>True si la jugada es correcta, false en caso contrario
        /// </returns>
        private Boolean ComprobarJugada(Tablero tablero, Posicion posicion, Color color)
        {
            Boolean movimientoValido;
            
            movimientoValido = Reglas.PosicionDentroTablero(posicion.Row, posicion.Col);
            if (!movimientoValido) return false;
            movimientoValido = Reglas.PosicionLegal(tablero, posicion.Row, posicion.Col, color);
            if (!movimientoValido) return false;

            return true;
        }

        /// <summary>
        /// Elegimos posición con la restricción de que sea un entero
        /// </summary>
        /// <returns>Posición elegida, solo comprobado que es un valor
        /// numérico</returns>
        public Posicion ElegirPosicion(Tablero tablero)
        {
            Posicion posicion = new Posicion();
            String rowString, colString;
            Boolean esEntero;
            int row = 0, col = 0;

            do
            {
                Console.WriteLine("Fila");
                rowString = Console.ReadLine();
                esEntero = Reglas.ComprobarEntero(rowString, out row);
                if (!esEntero) continue;

                Console.WriteLine("columna");
                colString = Console.ReadLine();
                esEntero = Reglas.ComprobarEntero(colString, out col);
            } while (!esEntero);

            posicion.Row = row;
            posicion.Col = col;

            return posicion;
        }
    }
}
