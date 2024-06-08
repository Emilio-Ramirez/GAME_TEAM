using System;
// CREATED
[System.Serializable]
public class SpecialCard : Card
{
    public string specialAbility;

    // Puedes agregar constructores, métodos adicionales o propiedades si lo deseas.
    public SpecialCard(int id, string nombre, int valor, string tipo, int rareza)
    {
        this.id_carta = id;
        this.nombre = nombre;
        this.valor_nutrimental = valor;
        this.tipo = tipo;
        this.specialAbility = "def";
        assign_ability();

    }

    public void assign_ability()
    {
        if (nombre == "Time")
        {
            specialAbility = "MoreTime";
        }
        else if (nombre == "Points")
        {
            specialAbility = "LessPoints";
        }
    }


}

