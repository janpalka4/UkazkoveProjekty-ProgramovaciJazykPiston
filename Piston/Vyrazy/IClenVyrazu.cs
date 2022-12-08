namespace Piston.Vyrazy;

public interface IClenVyrazu
{

}

public interface IClenVyrazu<T> : IClenVyrazu
{
    public new T Hodnota { get; set; }

}

