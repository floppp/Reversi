using System;

namespace Reversi
{
    /// <summary>
    /// Interface para definir el comportamiento de los participantes en
    /// el juego
    /// </summary>
    interface Participante
    {
        void Movimiento(Tablero tablero);
        Color GetColor();
        Posicion ElegirPosicion(Tablero tablero);
        void ActualizarTablero(Tablero tablero, int row, int col);
    }
}
