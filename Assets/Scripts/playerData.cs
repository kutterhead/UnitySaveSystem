[System.Serializable]
public class playerData
{
    public int vida;
    public int puntos;
    public int nivel;

    public playerData(int vida, int puntos, int nivel)
    {
        this.vida = vida;
        this.puntos = puntos;
        this.nivel = nivel;
    }
}
