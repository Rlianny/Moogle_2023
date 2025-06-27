using System.Text.RegularExpressions; // Regex 
using System.Text; //NormalizationForm
namespace MoogleEngine;

public class Tools
{
    private static string RemoveAccentsAndPuntuations(string inputString) // método que elimina todos los signos de puntuacion
    {
        return Regex.Replace(inputString.Normalize(NormalizationForm.FormD), @"[^a-zA-z0-9 ]+", " "); 
    }

    public static string[] TxtProcesser(string inputString) // método que procesa el contenido de un archivo y devuelve un array de palabras normalizadas
    {
        inputString = inputString.ToLower(); // llevamos todo el contenido a minusculas
        inputString = RemoveAccentsAndPuntuations(inputString); // removemos todos los signos de puntuacion
        string[] words = inputString.Split(' ', StringSplitOptions.RemoveEmptyEntries); // dividimos en palabras y guardamos en un array

        return words;
    }

    public static string Transform(string value)//metodo que elimina los caracteres innecesarios al leer un txt
    {
        char[] guide = { '\r', '\n', '(', ')', '*', '{', '}', '´', '`' ,',','.',':'};
        foreach (char a in guide)
        {
            value = value.Replace(a, ' ');
        }
        return value;
    }
}