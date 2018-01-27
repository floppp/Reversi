using System;
using System.Collections;

namespace Reversi
{
    /// <summary>
    /// IA que elige la pieza que coloca al azar de entre las que cumplen
    /// las reglas del juego
    /// </summary>
    class OrdenadorNivel1 : Ordenador, Participante
    {
        private OrdenadorNivel1(int numeroJugador)
        {
            if (numeroJugador == 1) base.color = Color.NEGRO;
            else                    base.color = Color.BLANCO;
        }

        public static OrdenadorNivel1 getInstance(int numeroJugador)
        {
            return new OrdenadorNivel1(numeroJugador);
        }

        /// <summary>
        /// Obtenemos el color del jugador
        /// </summary>
        /// <returns>Color</returns>
        public Color GetColor()
        {
            return base.color;
        }

        /// <summary>
        /// Método para controlar el movimiento del jugador
        /// </summary>
        /// <param name="tablero">Tablero en su situación actutal</param>
        public void Movimiento(Tablero tablero)
        {
            int piezasComidas;
            valoresElegidos = new Posicion();
            bool pasa = false;
            
            ComprobarValidas(tablero);
            valoresElegidos = ElegirPosicion(tablero);

            if (valoresElegidos.Col == 0)
                if (valoresElegidos.Row == 0)
                    pasa = true;

            /* Cambiamos de color las fichas 'comidas' y colocamos la pieza
            en la posición elegida */
            if (!pasa)
            {
                Reglas.CambioDeColor(tablero, color, valoresElegidos.Row,
                        valoresElegidos.Col, out piezasComidas);
                ActualizarTablero(tablero, valoresElegidos.Row,
                        valoresElegidos.Col);
            }

            
            //Reglas.CambioDeColor(tablero, color, valoresElegidos.Row,
            //        valoresElegidos.Col, out piezasComidas);
            //ActualizarTablero(tablero, valoresElegidos.Row, valoresElegidos.Col);
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
        /// Método donde decidimos la forma de actual de la IA.
        /// En este caso, elgimos de forma aleatoria entre las posiciones
        /// válidas
        /// </summary>
        /// <returns>Posicion elegida</returns>
        public Posicion ElegirPosicion(Tablero tablero)
        {
            Random random = new Random(Guid.NewGuid().GetHashCode());
            Posicion posicion = new Posicion();
            int posicionElegida;
            if (posicionesValidas.Count != 0)
                posicionElegida = random.Next(1, posicionesValidas.Count);
            else
            {
                posicionElegida = 0;
                posicion.Col = 0;
                posicion.Row = 0;
            }
            for (int i = 0; i < posicionElegida; i++)
            {
                posicion = new Posicion();
                posicion = (Posicion)posicionesValidas.Dequeue();
            }

            return posicion;
        }
    }
}