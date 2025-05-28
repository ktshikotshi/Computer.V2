namespace Computer.V2.Lib;

public class DummyClass {
    const int SOME_NUMBER = 1;
    public bool ShouldDoSomething(int someNumber)
    {
        if (someNumber == SOME_NUMBER) return true;

        return false;
    }
}