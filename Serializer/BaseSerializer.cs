using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serializer
{
    public abstract class BaseSerializer
    {
    public abstract void SerializeString(String name, ref String value);
    public abstract void SerializeInt(String name, ref int value);
    public abstract void SerializeDouble(String name, ref double value);
    public abstract int PushSection(String name, ref String att, ref String value);
    public abstract void PopSection();
    }
}
