using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serializer
{
    public abstract class BaseSerializer
    {
    public abstract bool SerializeString(String name, ref String value);
    public abstract bool SerializeInt(String name, ref int value);
    public abstract bool SerializeDouble(String name, ref double value);
    public abstract bool PushSection(string name, int order);
    public abstract bool SerializeAttribute(string name, ref string val);
    public abstract void PopSection();
    public abstract void SerializeString(ref string s);
    public bool IsLoading { get; set; }
    }
}
