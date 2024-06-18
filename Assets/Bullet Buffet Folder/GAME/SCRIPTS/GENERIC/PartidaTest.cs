using System.Collections;
using System.Collections.Generic;

public class PartidaTest
{
    public List<JugadorInfo> Equipo1 { get; private set; }
    public List<JugadorInfo> Equipo2 { get; private set; }

    public PartidaTest()
    {
        Equipo1 = new List<JugadorInfo>();
        Equipo2 = new List<JugadorInfo>();
    }

    public void AgregarJugadorAlEquipo1(JugadorInfo jugador)
    {
        Equipo1.Add(jugador);
    }

    public void AgregarJugadorAlEquipo2(JugadorInfo jugador)
    {
        Equipo2.Add(jugador);
    }
}
