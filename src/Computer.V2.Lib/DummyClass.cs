namespace Computer.V2.Lib;

public class DummyClass {
    private const int SOME_NUMBER = 1;
    private const int SOME_OTHER_NUMBER = 2;
    public bool ShouldDoSomething(int someNumber)
    {
        var someString = string.Empty;
        if (someNumber == SOME_NUMBER) someString = "1";
        if (someNumber == SOME_OTHER_NUMBER) someString = "2";
        
        return someString == "1";
    }
}