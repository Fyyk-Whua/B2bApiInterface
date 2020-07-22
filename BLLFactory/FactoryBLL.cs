using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLLFactory
{
    public class FactoryBLL
    {

        #region CreateCommonBLL
        /// <summary>
        /// CreateCommonBLL
        /// </summary>
        /// <returns></returns>
        public Facade.ICommonBLL CreateCommonBLL()
        {
            string className = string.Format("{0}{1}", Util.DalConst.NamespacePathBLL.Trim(), ".CommonBLL");
            Facade.ICommonBLL ibll = (Facade.ICommonBLL)System.Reflection.Assembly.Load(Util.DalConst.AssemblyPathBLL.Trim()).CreateInstance(className);
            return ibll;
        }
        #endregion

       
      

    }
}
