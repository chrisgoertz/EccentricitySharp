using System;
using System.Collections.Generic;
using System.Numerics;
using FftSharp;

class Program
{
    static void Main()
    {
        int samplesPerRotation = 2048;

        // Beispiel: Simulierte Daten
        var topOuter = GenerateSinusData(samplesPerRotation, 0.1, 0);
        var bottomOuter = GenerateSinusData(samplesPerRotation, 0.2, Math.PI / 4);
        var bottomInner = GenerateSinusData(samplesPerRotation, 0.05, Math.PI / 2);

        Console.WriteLine("Exzentrizitätsanalyse mit FFT (Amplitude und Phase der 1. Harmonischen):\n");

        AnalyzeEccentricity(topOuter, "Oben (Außen)");
        AnalyzeEccentricity(bottomOuter, "Unten (Außen)");
        AnalyzeEccentricity(bottomInner, "Unten (Innen)");
    }

    static List<double> GenerateSinusData(int count, double amplitude, double phase)
    {
        var data = new List<double>(count);
        for (int i = 0; i < count; i++)
        {
            double angle = 2 * Math.PI * i / count;
            data.Add(amplitude * Math.Sin(angle + phase));
        }
        return data;
    }

    static void AnalyzeEccentricity(List<double> data, string label)
    {
        int n = data.Count;

        // FFT durchführen – real → complex
        Complex[] fft = FftSharp.FFT.ForwardReal(data.ToArray());

        // Index 1 = 1. Harmonische (nach DC bei Index 0)
        Complex harmonic1 = fft[1];

        double amplitude = 2.0 * harmonic1.Magnitude / n;
        double phaseRad = harmonic1.Phase;
        double phaseDeg = phaseRad * (180.0 / Math.PI);
        if (phaseDeg < 0) phaseDeg += 360;

        Console.WriteLine($"{label}:");
        Console.WriteLine($"  Exzentrizität (Amplitude) = {amplitude:F4}");
        Console.WriteLine($"  Richtung (Phase) = {phaseDeg:F2}°");
        Console.WriteLine();
    }
}
