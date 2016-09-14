using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eIVOGo.MvcHelper
{
    public class ServerTransferResult : ActionResult
    {
        public String TransferPath
        { get; set; }

        public bool PreserveForm
        { get; set; }

        public ServerTransferResult()
        {

        }

        public ServerTransferResult(String path)
        {
            TransferPath = path;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            ((Controller)context.Controller).Server.Transfer(TransferPath, PreserveForm);
        }
    }
}