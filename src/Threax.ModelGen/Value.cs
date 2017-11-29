using System;
using System.Collections.Generic;
using System.Text;

//Define your type here
public class Value
{
    public String Name { get; set; }
}

//Keep this class/funciton, it will return the type to the caller
public static class Recovery
{
    public static Type GetType()
    {
        return typeof(Value);
    }
}