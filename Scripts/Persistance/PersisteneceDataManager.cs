using MyThings.ExtendableClass;
using System;
using System.Collections.Generic;

namespace MyThings.Persistance
{

    public class PersisteneceDataManager : Singleton_C<PersisteneceDataManager>
    {
        public Dictionary<Type, object> Data = new Dictionary<Type, object>();
    }

}