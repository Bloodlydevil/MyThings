using System;

namespace MyThings.Extension
{
    public static class ExtensionUtils
    {
        public static type IfTrue<type>(this type obj,Action<type> toPerform)
        {
            if(obj != null)
                toPerform(obj);
            return obj;
        }
    }
}
