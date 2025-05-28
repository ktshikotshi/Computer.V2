namespace Computer.V2.Lib;

public class DummyClas {
    public DummyClass()
    {
    }

    public bool ShouldDoSomething(int someNumber)
    {
        if (someNumber == 1) return true;
        if (someNumber == 2) return true;
        if (someNumber == 3) return false;

        return false;
    }
}