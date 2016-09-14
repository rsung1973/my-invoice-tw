using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Business.Helper;
using Model.DataEntity;
using Model.Locale;
using Model.Security.MembershipManagement;
using Utility;
using Uxnet.Web.WebUI;

namespace eIVOGo.Module.EIVO
{
    public partial class TrackDataList : System.Web.UI.UserControl
    {
        private UserProfileMember _userProfile;
        protected void Page_Load(object sender, EventArgs e)
        {
            //
            _userProfile = WebPageUtility.UserProfile;
            
        }

        private void initializeData()
        {
            if (_userProfile["year"] != null)
            {
                TrackCodeYear.SelectedValue = (String)_userProfile["year"];
                btnQuery.CommandArgument = "Query";
                _userProfile["year"] = null;
            }
            if (_userProfile["msg"] != null)
            {
                this.AjaxAlert((String)_userProfile["msg"]);
                _userProfile["msg"] = null;
            }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.PreRender += new EventHandler(TrackDataList_PreRender);
            TrackCodeYear.Load += new EventHandler(TrackCodeYear_Load);
        }

        void TrackCodeYear_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                initializeData();
            }
        }

        void TrackDataList_PreRender(object sender, EventArgs e)
        {
            if (btnQuery.CommandArgument == "Query")
            {
                showResult.Visible = true;
                itemList.QueryExpr = i => i.Year == TrackCodeYear.Year;
                itemList.BindData();
            }
        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            btnQuery.CommandArgument = "Query";
            _userProfile["year"] = TrackCodeYear.SelectedValue;
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            Response.Redirect(String.Format("~/SAM/AddInvoiceTrackCode.aspx?year={0}", TrackCodeYear.SelectedValue));
        }

        protected bool CharCheck(String theStr)
        {
            bool bolAns = true;
            char[] theChar;
            theChar = theStr.ToCharArray();

            if (theChar.Length == 2)
            {
                for (int i = 0; i < theChar.Length; i++)
                {
                    if (theChar[i] < 'A' || theChar[i] > 'Z')
                    {
                        bolAns = false;
                        break;
                    }
                }
            }
            else
                bolAns = false;

            return bolAns;
        }

    }
}