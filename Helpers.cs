using System;
using System.Numerics;

public class ZylinderRichtung
{
    private readonly Vector3 _punktOben;
    private readonly Vector3 _punktMitte;
    private readonly Vector3 _punktUnten;

    public ZylinderRichtung(
        (double winkelGrad, double amplitudeUm, double höheMm) oben,
                            (double winkelGrad, double amplitudeUm, double höheMm) mitte,
                            (double winkelGrad, double amplitudeUm, double höheMm) unten)
    {
        _punktOben = PolarZuKartesischMitHoehe(oben);
        _punktMitte = PolarZuKartesischMitHoehe(mitte);
        _punktUnten = PolarZuKartesischMitHoehe(unten);
    }

    /// <summary>
    /// Gibt den Richtungswinkel (Grad) und den Korrekturbetrag (µm) zurück,
    /// um den unteren Punkt so zu verschieben, dass die Exzentrizitätslinie geradlinig wird.
    /// </summary>
    public (double richtWinkelGrad, double korrekturBetragUm, Vector2 korrekturVektorUm) BerechneKorrektur()
    {
        var richtung = Vector3.Normalize(_punktOben - _punktMitte);

        var p0 = _punktMitte;
        var v = richtung;
        var u = _punktUnten - p0;

        var projektionsLänge = Vector3.Dot(u, v);
        var projiziert = p0 + projektionsLänge * v;

        var korrektur3D = _punktUnten - projiziert;
        var korrektur2D = new Vector2(korrektur3D.X, korrektur3D.Y);
        var korrekturBetrag = korrektur2D.Length();

        var richtWinkel = Math.Atan2(korrektur2D.Y, korrektur2D.X) * 180.0 / Math.PI;
        if (richtWinkel < 0) richtWinkel += 360.0;

        return (richtWinkel, korrekturBetrag, korrektur2D);
    }

    private static Vector3 PolarZuKartesischMitHoehe((double winkelGrad, double amplitudeUm, double höheMm) daten)
    {
        double winkelRad = daten.winkelGrad * Math.PI / 180.0;
        double x = daten.amplitudeUm * Math.Cos(winkelRad);
        double y = daten.amplitudeUm * Math.Sin(winkelRad);
        return new Vector3((float)x, (float)y, (float)daten.höheMm);
    }
}
