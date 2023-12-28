﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Forms_Withdrawn_New : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (System.Convert.ToInt32(Session["Vis"]) == 0)
            {
                Response.Redirect("../Account/Login.aspx");
            } 
            
            int role = Convert.ToInt32(Session["RolesId"]);

            if (role != 1)
            {
                Response.Redirect("~/Login.aspx");
            }                    
        }
    }


}