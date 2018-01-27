using System;

namespace Reversi
{
    /// <summary>
    /// IA que elige la pieza que coloca buscando en las distintos escenarios
    /// del juego que pueden darse a partir de las posiciones legales.
    /// Buscamos con una profundidad de 2 por defecto.
    /// En caso de dos posiciones con la misma cantidad de piezas ganadas
    /// elige la primera de ellas
    /// </summary>
    class OrdenadorNivel3 : Ordenador, Participante
    {
        private int[,] fichasComidas;
        private static readonly int PROFUNDIDAD = 1;

        private OrdenadorNivel3(int numeroJugador)
        {
            if (numeroJugador == 1) base.color = Color.NEGRO;
            else                    base.color = Color.BLANCO;

            fichasComidas = new int[10, 10];
        }

        public static OrdenadorNivel3 getInstance(int numeroJugador)
        {
            return new OrdenadorNivel3(numeroJugador);
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
        /// Método para pedir posición al jugador
        /// </summary>
        /// <param name="tablero">Tablero en su situación actutal</param>
        public void Movimiento(Tablero tablero)
        {
            int  piezasComidas;
            valoresElegidos = new Posicion();
            ComprobarValidas(tablero);
            valoresElegidos = ElegirPosicion(tablero);
            Reglas.CambioDeColor(tablero, color, valoresElegidos.Row,
                    valoresElegidos.Col, out piezasComidas);
            ActualizarTablero(tablero, valoresElegidos.Row, valoresElegidos.Col);
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
            Posicion valoresElegidos = new Posicion();
            PonerFichasComidasEnBlanco();
            valoresElegidos = ElegirPosicion_bis(tablero, PROFUNDIDAD);
            return valoresElegidos;
        }

        /// <summary>
        /// Buscamos, con una profundidad definida (por defecto 3) qué mo-
        /// vimiento es más provechoso para el jugador. Una especie de 
        /// Minimax bastante sui-generis
        /// </summary>
        /// <param name="tablero">Tablero del que queremos obtener distintas
        /// opciones de juego</param>
        /// <param name="profundidad">rango de movimientos máximos que podemos
        /// simular en cascada</param>
        /// <returns></returns>
        public Posicion ElegirPosicion_bis(Tablero tablero, int profundidad)
        {
            Console.Read();
            int piezasComidas;
            int row, col;
            Posicion posicionElegida = new Posicion();
            Tablero tableroCopia = new Tablero();
            tableroCopia.Copiar(tablero);

            if (profundidad > 0)
                foreach (Posicion posicion in posicionesValidas)
                {
                    profundidad--;
                    Reglas.CambioDeColor(tableroCopia, color, posicion.Row,
                            posicion.Col, out piezasComidas);
                    row = posicion.Row;
                    col = posicion.Col;
                    ImprimirMatriz();
                    fichasComidas[row, col] += piezasComidas;
                    ElegirPosicion_bis(tableroCopia, profundidad);
                }

            posicionElegida = ComprobarMejorPosicion();

            return posicionElegida;
        }

        private void ImprimirMatriz()
        {
            for (int row = 1; row < 9; row++)
            {
                for (int col = 1; col < 9; col++)
                    Console.Write(fichasComidas[row, col] + " ");
                Console.WriteLine("");
            }
        }


        /// <summary>
        /// Comprobamos la matriz de registro de piezas comidas, y devolvemos
        /// la mejor posición para colocar la pieza
        /// </summary>
        /// <returns>Posición óptima</returns>
        private Posicion ComprobarMejorPosicion()
        {
            Posicion posicion = new Posicion();

            int rowMax = 1, colMax = 1;
            for (int row = 1; row < 9; row++)
                for (int col = 1; col < 9; col++)
                    if (fichasComidas[row, col] > fichasComidas[rowMax, colMax])
                    {
                        rowMax = row;
                        colMax = col;
                    }

            posicion.Row = rowMax;
            posicion.Col = colMax;

            return posicion;
        }

        /// <summary>
        /// Ponemos la matriz que nos dice que jugada es más óptima, con
        /// cual ganamos más piezas al contrario, a 0
        /// </summary>
        private void PonerFichasComidasEnBlanco()
        {
            for (int row = 1; row < 9; row++)
                for (int col = 1; col < 9; col++)
                    this.fichasComidas[row, col] = 0;
        }           
    }
}