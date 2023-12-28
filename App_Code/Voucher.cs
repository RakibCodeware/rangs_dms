using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Voucher
/// </summary>
public class Voucher
{
	public Voucher() {}
	//
	// TODO: Add constructor logic here
	//
    public int Id { get; set; }
    public string CustomerContact { get; set; }
    public string InvoiceNo { get; set; }
    public int EarnedVoucherPoint { get; set; }
    public int RedeemVoucherPoint { get; set; }
}