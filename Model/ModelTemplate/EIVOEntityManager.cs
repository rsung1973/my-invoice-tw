using System;
using System.Data;
using System.Linq;
using System.Data.Linq;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;

using Model.DataEntity;
using Model.Properties;
using Utility;
using DataAccessLayer.basis;




namespace Model.ModelTemplate
{
	/// <summary>
	/// UserManager ªººK­n´y­z¡C
	/// </summary>
    public partial class EIVOGenericManager<TEntity> : GenericManager<EIVOEntityDataContext,TEntity>
        where TEntity:class,new()
	{
        public EIVOGenericManager() : base() { }
        public EIVOGenericManager(GenericManager<EIVOEntityDataContext> manager) : base(manager) { }
    }

   

   
    
}
