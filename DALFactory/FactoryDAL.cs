using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DALFactory
{
    public class FactoryDAL
    {

        #region CreateCommonDAL
        /// <summary>
        /// CreateCommonDAL
        /// </summary>
        /// <returns></returns>
        public IDAL.ICommonDAL CreateCommonDAL()
        {
            string className = Util.DalConst.NamespacePathDAL.Trim() + ".CommonDAL";
            IDAL.ICommonDAL idal = (IDAL.ICommonDAL)System.Reflection.Assembly.Load(Util.DalConst.AssemblyPathDAL.Trim()).CreateInstance(className);
            return idal;
        }
        #endregion

      

    }
}
